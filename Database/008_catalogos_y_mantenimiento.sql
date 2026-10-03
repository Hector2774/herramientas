-- ============================================================
-- 008 · Catálogos con conteo, eliminación por SP y tipo "Calibración"
--
--   sp_Categoria_ObtenerTodas / sp_Marca_ObtenerTodas
--       + columna Herramientas (cuántas herramientas activas la usan)
--       sp_Marca_ObtenerTodas devuelve el nombre como "Nombre" (antes
--       "NombreMarca", que el formulario no encontraba)
--   sp_Categoria_Eliminar / sp_Marca_Eliminar
--       la misma lógica que tenía el formulario en SQL suelto: desliga
--       las herramientas y borra el registro, ahora en una transacción
--   Mantenimiento: nuevo tipo 'Calibración'
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 008_catalogos_y_mantenimiento.sql
-- ============================================================
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
GO

-- ── Categorías ───────────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Categoria_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CategoriaId,
           c.Nombre,
           c.Descripcion,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.CategoriaId = c.CategoriaId AND h.Activa = 1) AS Herramientas
    FROM   CategoriaHerramienta c
    ORDER  BY c.Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE sp_Categoria_Eliminar
    @CategoriaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Herramienta SET CategoriaId = NULL WHERE CategoriaId = @CategoriaId;
        DELETE FROM CategoriaHerramienta WHERE CategoriaId = @CategoriaId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ── Marcas ───────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Marca_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.MarcaId,
           m.NombreMarca AS Nombre,
           m.Descripcion,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.MarcaId = m.MarcaId AND h.Activa = 1) AS Herramientas
    FROM   Marca m
    ORDER  BY m.NombreMarca ASC;
END
GO

CREATE OR ALTER PROCEDURE sp_Marca_Eliminar
    @MarcaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Herramienta SET MarcaId = NULL WHERE MarcaId = @MarcaId;
        DELETE FROM Marca WHERE MarcaId = @MarcaId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ── Mantenimiento: tipo Calibración ──────────────────────────
ALTER TABLE Mantenimiento DROP CONSTRAINT CK_Mantenimiento_Tipo;
ALTER TABLE Mantenimiento ADD CONSTRAINT CK_Mantenimiento_Tipo
    CHECK (TipoMantenimiento IN ('Preventivo', 'Correctivo', 'Calibración'));
GO

-- ============================================================
-- MANTENIMIENTO (por unidad)
--   @UnidadIds = '3,7'  → esas unidades (todas deben estar Disponibles o Dañadas)
--   @UnidadIds = NULL   → todas las unidades Disponibles o Dañadas del grupo
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Mantenimiento_RegistrarEntrada
    @HerramientaId      INT,
    @UnidadIds          VARCHAR(MAX) = NULL,
    @TipoMantenimiento  VARCHAR(50),
    @Descripcion        VARCHAR(400) = NULL,
    @RealizadoPor       VARCHAR(120) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @TipoMantenimiento NOT IN ('Preventivo', 'Correctivo', 'Calibración')
        THROW 50000, 'Tipo de mantenimiento no válido. Use: Preventivo, Correctivo o Calibración.', 1;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
        THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Objetivo TABLE (UnidadId INT PRIMARY KEY);

        IF @UnidadIds IS NULL
        BEGIN
            INSERT INTO @Objetivo (UnidadId)
            SELECT UnidadId
            FROM   HerramientaUnidad WITH (UPDLOCK, HOLDLOCK)
            WHERE  HerramientaId = @HerramientaId
              AND  Estado IN ('Disponible', 'Dañada');

            IF NOT EXISTS (SELECT 1 FROM @Objetivo)
                THROW 50000, 'El grupo no tiene unidades disponibles o dañadas para enviar a mantenimiento.', 1;
        END
        ELSE
        BEGIN
            DECLARE @Pedidas TABLE (UnidadId INT PRIMARY KEY);
            INSERT INTO @Pedidas (UnidadId)
            SELECT DISTINCT TRY_CAST(value AS INT)
            FROM   STRING_SPLIT(@UnidadIds, ',')
            WHERE  TRY_CAST(value AS INT) IS NOT NULL;

            INSERT INTO @Objetivo (UnidadId)
            SELECT u.UnidadId
            FROM   HerramientaUnidad u WITH (UPDLOCK, HOLDLOCK)
            INNER  JOIN @Pedidas p ON p.UnidadId = u.UnidadId
            WHERE  u.HerramientaId = @HerramientaId
              AND  u.Estado IN ('Disponible', 'Dañada');

            IF NOT EXISTS (SELECT 1 FROM @Pedidas)
               OR (SELECT COUNT(*) FROM @Objetivo) <> (SELECT COUNT(*) FROM @Pedidas)
                THROW 50000, 'Solo pueden entrar a mantenimiento unidades Disponibles o Dañadas de esta herramienta.', 1;
        END

        INSERT INTO Mantenimiento (
            HerramientaId, UnidadId, FechaInicio,
            TipoMantenimiento, Descripcion, RealizadoPor
        )
        SELECT @HerramientaId, UnidadId, GETDATE(),
               @TipoMantenimiento, @Descripcion, @RealizadoPor
        FROM   @Objetivo;

        DECLARE @Unidades INT = @@ROWCOUNT;

        UPDATE HerramientaUnidad
        SET    Estado = 'En Mantenimiento'
        WHERE  UnidadId IN (SELECT UnidadId FROM @Objetivo);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT @Unidades AS Unidades;
END
GO

COMMIT TRANSACTION;
GO
