-- ============================================================
-- 014 · Foto por herramienta
-- La imagen vive en la carpeta Fotos\ junto al .exe; la BD solo guarda el nombre del archivo.
-- ============================================================
USE PROMACO_Herramientas;
GO

IF COL_LENGTH('dbo.Herramienta', 'FotoNombre') IS NULL
    ALTER TABLE Herramienta ADD FotoNombre NVARCHAR(200) NULL;
GO

-- ── Consultas: incluyen FotoNombre ─────────────────────────────

CREATE OR ALTER PROCEDURE sp_Herramienta_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre,
           h.Caracteristicas,
           CASE WHEN h.PrestamoHabilitado = 1 THEN 'Habilitado' ELSE 'Suspendido' END AS Prestamo,
           s.StockTotal,
           s.StockDisponible,
           s.StockPrestado,
           s.StockMantenimiento,
           s.StockDañado,
           h.Activa,
           h.PrestamoHabilitado,
           c.Nombre       AS Categoria,
           m.NombreMarca  AS Marca,
           u.Nombre       AS Ubicacion,
           h.CategoriaId,
           h.MarcaId,
           h.UbicacionId,
           h.FotoNombre
    FROM   Herramienta    h
    INNER  JOIN vw_HerramientaStock    s ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca                m ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            u ON h.UbicacionId   = u.UbicacionId
    WHERE  h.Activa = 1
    ORDER  BY h.Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE sp_Herramienta_ObtenerPorId
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre,
           h.Caracteristicas,
           CASE WHEN h.PrestamoHabilitado = 1 THEN 'Habilitado' ELSE 'Suspendido' END AS Prestamo,
           s.StockTotal,
           s.StockDisponible,
           s.StockPrestado,
           s.StockMantenimiento,
           s.StockDañado,
           s.StockPerdido,
           s.StockBaja,
           h.Activa,
           h.PrestamoHabilitado,
           c.Nombre       AS Categoria,
           m.NombreMarca  AS Marca,
           u.Nombre       AS Ubicacion,
           h.CategoriaId,
           h.MarcaId,
           h.UbicacionId,
           h.FotoNombre
    FROM   Herramienta    h
    INNER  JOIN vw_HerramientaStock    s ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca                m ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            u ON h.UbicacionId   = u.UbicacionId
    WHERE  h.HerramientaId = @HerramientaId;
END
GO

CREATE OR ALTER PROCEDURE sp_Herramienta_ObtenerCatalogoPrestamo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre,
           c.Nombre              AS Categoria,
           m.NombreMarca         AS Marca,
           ub.Nombre             AS Ubicacion,
           ISNULL(s.StockDisponible, 0) AS StockDisponible,
           ISNULL(s.StockTotal, 0)      AS StockTotal,
           h.FotoNombre
    FROM   Herramienta               h
    LEFT   JOIN vw_HerramientaStock  s  ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c  ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca                m  ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            ub ON h.UbicacionId   = ub.UbicacionId
    WHERE  h.Activa             = 1
      AND  h.PrestamoHabilitado = 1
    ORDER  BY h.Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE sp_Devolucion_ObtenerDetallePendiente
    @PrestamoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT pd.PrestamoDetalleId,
           pd.HerramientaId,
           pd.UnidadId,
           u.CodigoUnidad AS CodigoHerramienta,
           h.Nombre       AS Herramienta,
           m.NombreMarca  AS Marca,
           c.Nombre       AS Categoria,
           pd.EstadoDevolucion,
           h.FotoNombre
    FROM   PrestamoDetalle          pd
    INNER  JOIN Herramienta          h ON pd.HerramientaId = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON pd.UnidadId      = u.UnidadId
    LEFT   JOIN Marca                m ON h.MarcaId        = m.MarcaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId    = c.CategoriaId
    WHERE  pd.PrestamoId    = @PrestamoId
      AND  pd.FechaDevuelta IS NULL
    ORDER  BY h.Nombre ASC, u.Numero ASC;
END
GO

-- ── Inserción / actualización: @FotoNombre opcional ───────────

CREATE OR ALTER PROCEDURE sp_Herramienta_Insertar
    @Nombre          VARCHAR(120),
    @Caracteristicas VARCHAR(400) = NULL,
    @CategoriaId     INT          = NULL,
    @MarcaId         INT          = NULL,
    @UbicacionId     INT          = NULL,
    @StockTotal      INT          = 1,
    @FotoNombre      NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Herramienta (
            Nombre, Caracteristicas,
            CategoriaId, MarcaId, UbicacionId, Activa, PrestamoHabilitado, FotoNombre
        )
        VALUES (
            @Nombre, @Caracteristicas,
            @CategoriaId, @MarcaId, @UbicacionId, 1, 1, @FotoNombre
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
-- @FotoNombre NULL = conservar la foto actual (la foto se cambia con sp_Herramienta_ActualizarFoto).
CREATE OR ALTER PROCEDURE sp_Herramienta_Actualizar
    @HerramientaId   INT,
    @Nombre          VARCHAR(120),
    @Caracteristicas VARCHAR(400) = NULL,
    @CategoriaId     INT          = NULL,
    @MarcaId         INT          = NULL,
    @UbicacionId     INT          = NULL,
    @StockTotal      INT          = NULL,
    @FotoNombre      NVARCHAR(200) = NULL
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
               UbicacionId     = @UbicacionId,
               FotoNombre      = ISNULL(@FotoNombre, FotoNombre)
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

-- Solo la foto (botón "Cambiar foto" de FrmHerramientas). NULL = quitar la foto.
CREATE OR ALTER PROCEDURE sp_Herramienta_ActualizarFoto
    @HerramientaId INT,
    @FotoNombre    NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Herramienta SET FotoNombre = @FotoNombre WHERE HerramientaId = @HerramientaId;

    IF @@ROWCOUNT = 0
        THROW 50000, 'La herramienta especificada no existe.', 1;
END
GO

-- ── Fotos ya existentes (una sola vez) ─────────────────────────
-- Los .webp/.avif originales se convirtieron a .jpg: GDI+ (System.Drawing) no los lee.
UPDATE h SET FotoNombre = f.Archivo
FROM   Herramienta h
JOIN  (VALUES
        ('HER-0001', 'taladro_percutor.jpg'),
        ('HER-0002', 'esmeriladora.png'),
        ('HER-0003', 'sierra_circular.jpg'),
        ('HER-0004', 'rotomartillo.jpg'),
        ('HER-0005', 'lijadora_orbital.jpeg'),
        ('HER-0006', 'atornillador_impacto.jpg'),
        ('HER-0007', 'taladro_inalambrico.jpeg'),
        ('HER-0008', 'sierra_caladora.jpg'),
        ('HER-0009', 'desarmadores_truper.jpg'),
        ('HER-0010', 'martillo_unia.jpeg'),
        ('HER-0011', 'llave_stilson.jpg'),
        ('HER-0012', 'llaves_combinadas.jpg'),
        ('HER-0013', 'pinza_presion.jpg'),
        ('HER-0014', 'flexometro.jpg'),
        ('HER-0015', 'nivel_laser.jpeg'),
        ('HER-0016', 'nivel_aluminio.jpeg'),
        ('HER-0017', 'pistola_neumatica.jpg'),
        ('HER-0018', 'podadora.jpg'),
        ('HER-0019', 'tijera_podar.jpg'),
        ('HER-0020', 'careta_soldar.jpg')
      ) AS f (Codigo, Archivo) ON f.Codigo = h.Codigo
WHERE  h.FotoNombre IS NULL;
GO
