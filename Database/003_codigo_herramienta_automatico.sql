-- ============================================================
-- 003 · Código de herramienta automático
--
-- Antes: el administrador escribía el Código a mano.
-- Ahora: Herramienta.Codigo es una columna calculada a partir del
-- HerramientaId (IDENTITY), con formato HER-0001, HER-0002, ...
-- (a partir de 9999 simplemente crece: HER-10000).
-- sp_Herramienta_Insertar / _Actualizar ya no reciben @Codigo.
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 003_codigo_herramienta_automatico.sql
-- ============================================================
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
GO

-- ── Quitar la restricción UNIQUE (nombre autogenerado) y la columna ──
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += N'ALTER TABLE Herramienta DROP CONSTRAINT ' + QUOTENAME(kc.name) + N';'
FROM   sys.key_constraints kc
JOIN   sys.index_columns ic ON ic.object_id = kc.parent_object_id AND ic.index_id = kc.unique_index_id
JOIN   sys.columns c        ON c.object_id = ic.object_id AND c.column_id = ic.column_id
WHERE  kc.parent_object_id = OBJECT_ID('Herramienta')
  AND  kc.type = 'UQ'
  AND  c.name = 'Codigo';
EXEC (@sql);

ALTER TABLE Herramienta DROP COLUMN Codigo;
GO

ALTER TABLE Herramienta ADD Codigo AS
    CAST('HER-' + RIGHT('0000' + CAST(HerramientaId AS VARCHAR(10)),
                        IIF(HerramientaId > 9999, LEN(CAST(HerramientaId AS VARCHAR(10))), 4))
         AS VARCHAR(20)) PERSISTED NOT NULL;

ALTER TABLE Herramienta ADD CONSTRAINT UQ_Herramienta_Codigo UNIQUE (Codigo);
GO

-- Si el catálogo está vacío, que el primer código sea HER-0001
IF NOT EXISTS (SELECT 1 FROM Herramienta)
    DBCC CHECKIDENT ('Herramienta', RESEED, 0);
GO

-- ── Procedimientos ───────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Herramienta_Insertar
    @Nombre          VARCHAR(120),
    @Caracteristicas VARCHAR(400) = NULL,
    @CategoriaId     INT          = NULL,
    @MarcaId         INT          = NULL,
    @UbicacionId     INT          = NULL,
    @StockTotal      INT          = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Herramienta (
            Nombre, Caracteristicas,
            CategoriaId, MarcaId, UbicacionId, Activa, PrestamoHabilitado
        )
        VALUES (
            @Nombre, @Caracteristicas,
            @CategoriaId, @MarcaId, @UbicacionId, 1, 1
        );

        DECLARE @HerramientaId INT = SCOPE_IDENTITY();

        EXEC sp_Herramienta_AgregarUnidades @HerramientaId, @StockTotal;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT HerramientaId, Codigo FROM Herramienta WHERE HerramientaId = @HerramientaId;
END
GO

-- @StockTotal mayor al actual agrega unidades nuevas.
-- Para reducir el stock hay que dar de baja (o marcar perdidas) unidades concretas.
CREATE OR ALTER PROCEDURE sp_Herramienta_Actualizar
    @HerramientaId   INT,
    @Nombre          VARCHAR(120),
    @Caracteristicas VARCHAR(400) = NULL,
    @CategoriaId     INT          = NULL,
    @MarcaId         INT          = NULL,
    @UbicacionId     INT          = NULL,
    @StockTotal      INT          = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId)
        THROW 50000, 'La herramienta especificada no existe.', 1;

    DECLARE @StockActual INT = (SELECT StockTotal FROM vw_HerramientaStock WHERE HerramientaId = @HerramientaId);

    IF @StockTotal IS NOT NULL AND @StockTotal < @StockActual
        THROW 50000, 'Para reducir el stock, dé de baja o marque como perdidas las unidades concretas desde "Unidades".', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Herramienta
        SET    Nombre          = @Nombre,
               Caracteristicas = @Caracteristicas,
               CategoriaId     = @CategoriaId,
               MarcaId         = @MarcaId,
               UbicacionId     = @UbicacionId
        WHERE  HerramientaId  = @HerramientaId;

        IF @StockTotal > @StockActual
        BEGIN
            DECLARE @Nuevas INT = @StockTotal - @StockActual;
            EXEC sp_Herramienta_AgregarUnidades @HerramientaId, @Nuevas;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

COMMIT TRANSACTION;
GO
