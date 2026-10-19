-- ============================================================
-- 019 · Categorías, marcas y ubicaciones se desactivan en lugar de borrarse
--   Antes, eliminar uno de estos registros dejaba sus herramientas sin categoría, marca
--   o ubicación, y los reportes históricos cambiaban. Ahora siguen la misma regla que
--   los proveedores:
--     · Si alguna herramienta (activa o dada de baja) lo usa, se desactiva: ya no se
--       ofrece para herramientas nuevas, pero las que lo tienen lo conservan.
--     · Si nadie lo usa, se borra.
--   Agregar un nombre que pertenece a un registro desactivado lo reactiva.
--
-- Ejecutar con:  sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i 019_catalogos_desactivables.sql
-- ============================================================
SET XACT_ABORT ON;
GO

IF COL_LENGTH('dbo.CategoriaHerramienta', 'Activo') IS NULL
    ALTER TABLE CategoriaHerramienta ADD Activo BIT NOT NULL CONSTRAINT DF_CategoriaHerramienta_Activo DEFAULT (1);
IF COL_LENGTH('dbo.Marca', 'Activo') IS NULL
    ALTER TABLE Marca ADD Activo BIT NOT NULL CONSTRAINT DF_Marca_Activo DEFAULT (1);
IF COL_LENGTH('dbo.Ubicacion', 'Activo') IS NULL
    ALTER TABLE Ubicacion ADD Activo BIT NOT NULL CONSTRAINT DF_Ubicacion_Activo DEFAULT (1);
GO

-- ── Categorías ───────────────────────────────────────────────
-- Devuelve también las desactivadas (Activo = 0): las herramientas que las tienen las
-- siguen mostrando y los reportes pueden filtrar por ellas. La pantalla decide cuáles ofrecer.
CREATE OR ALTER PROCEDURE sp_Categoria_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CategoriaId,
           c.Nombre,
           c.Descripcion,
           c.Activo,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.CategoriaId = c.CategoriaId AND h.Activa = 1) AS Herramientas
    FROM   CategoriaHerramienta c
    ORDER  BY c.Nombre ASC;
END
GO

-- Si el nombre pertenece a una categoría desactivada, se reactiva con los datos nuevos
CREATE OR ALTER PROCEDURE sp_Categoria_Insertar
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE Nombre = @Nombre AND Activo = 1)
    BEGIN
        RAISERROR('Ya existe una categoría con ese nombre.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE Nombre = @Nombre)
    BEGIN
        UPDATE CategoriaHerramienta
        SET    Activo = 1, Descripcion = @Descripcion
        WHERE  Nombre = @Nombre;

        SELECT CategoriaId FROM CategoriaHerramienta WHERE Nombre = @Nombre;
        RETURN;
    END

    INSERT INTO CategoriaHerramienta (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);

    SELECT SCOPE_IDENTITY() AS CategoriaId;
END
GO

CREATE OR ALTER PROCEDURE sp_Categoria_Actualizar
    @CategoriaId INT,
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE CategoriaId = @CategoriaId AND Activo = 1)
    BEGIN
        RAISERROR('La categoría especificada no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE Nombre = @Nombre AND CategoriaId <> @CategoriaId)
    BEGIN
        RAISERROR('Ya existe otra categoría con ese nombre.', 16, 1);
        RETURN;
    END

    UPDATE CategoriaHerramienta
    SET    Nombre      = @Nombre,
           Descripcion = @Descripcion
    WHERE  CategoriaId = @CategoriaId;
END
GO

-- Con herramientas que la usen (activas o dadas de baja) solo se desactiva, para conservar el historial
CREATE OR ALTER PROCEDURE sp_Categoria_Eliminar
    @CategoriaId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Herramienta WHERE CategoriaId = @CategoriaId)
        UPDATE CategoriaHerramienta SET Activo = 0 WHERE CategoriaId = @CategoriaId;
    ELSE
        DELETE FROM CategoriaHerramienta WHERE CategoriaId = @CategoriaId;
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
           m.Activo,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.MarcaId = m.MarcaId AND h.Activa = 1) AS Herramientas
    FROM   Marca m
    ORDER  BY m.NombreMarca ASC;
END
GO

-- Si el nombre pertenece a una marca desactivada, se reactiva con los datos nuevos
CREATE OR ALTER PROCEDURE sp_Marca_Insertar
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Marca WHERE NombreMarca = @Nombre AND Activo = 1)
    BEGIN
        RAISERROR('Ya existe una marca con ese nombre.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Marca WHERE NombreMarca = @Nombre)
    BEGIN
        UPDATE Marca
        SET    Activo = 1, Descripcion = @Descripcion
        WHERE  NombreMarca = @Nombre;

        SELECT MarcaId FROM Marca WHERE NombreMarca = @Nombre;
        RETURN;
    END

    INSERT INTO Marca (NombreMarca, Descripcion)
    VALUES (@Nombre, @Descripcion);

    SELECT SCOPE_IDENTITY() AS MarcaId;
END
GO

CREATE OR ALTER PROCEDURE sp_Marca_Actualizar
    @MarcaId     INT,
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Marca WHERE MarcaId = @MarcaId AND Activo = 1)
    BEGIN
        RAISERROR('La marca especificada no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Marca WHERE NombreMarca = @Nombre AND MarcaId <> @MarcaId)
    BEGIN
        RAISERROR('Ya existe otra marca con ese nombre.', 16, 1);
        RETURN;
    END

    UPDATE Marca
    SET    NombreMarca      = @Nombre,
           Descripcion = @Descripcion
    WHERE  MarcaId     = @MarcaId;
END
GO

-- Con herramientas que la usen (activas o dadas de baja) solo se desactiva, para conservar el historial
CREATE OR ALTER PROCEDURE sp_Marca_Eliminar
    @MarcaId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Herramienta WHERE MarcaId = @MarcaId)
        UPDATE Marca SET Activo = 0 WHERE MarcaId = @MarcaId;
    ELSE
        DELETE FROM Marca WHERE MarcaId = @MarcaId;
END
GO

-- ── Ubicaciones ──────────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Ubicacion_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UbicacionId,
           u.Nombre,
           u.Descripcion,
           u.Activo,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.UbicacionId = u.UbicacionId AND h.Activa = 1) AS Herramientas
    FROM   Ubicacion u
    ORDER  BY u.Nombre ASC;
END
GO

-- Si el nombre pertenece a una ubicación desactivada, se reactiva con los datos nuevos
CREATE OR ALTER PROCEDURE sp_Ubicacion_Insertar
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = @Nombre AND Activo = 1)
    BEGIN
        RAISERROR('Ya existe una ubicación con ese nombre.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = @Nombre)
    BEGIN
        UPDATE Ubicacion
        SET    Activo = 1, Descripcion = @Descripcion
        WHERE  Nombre = @Nombre;

        SELECT UbicacionId FROM Ubicacion WHERE Nombre = @Nombre;
        RETURN;
    END

    INSERT INTO Ubicacion (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);

    SELECT SCOPE_IDENTITY() AS UbicacionId;
END
GO

CREATE OR ALTER PROCEDURE sp_Ubicacion_Actualizar
    @UbicacionId INT,
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Ubicacion WHERE UbicacionId = @UbicacionId AND Activo = 1)
    BEGIN
        RAISERROR('La ubicación especificada no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = @Nombre AND UbicacionId <> @UbicacionId)
    BEGIN
        RAISERROR('Ya existe otra ubicación con ese nombre.', 16, 1);
        RETURN;
    END

    UPDATE Ubicacion
    SET    Nombre      = @Nombre,
           Descripcion = @Descripcion
    WHERE  UbicacionId = @UbicacionId;
END
GO

-- Con herramientas que la usen (activas o dadas de baja) solo se desactiva, para conservar el historial
CREATE OR ALTER PROCEDURE sp_Ubicacion_Eliminar
    @UbicacionId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Herramienta WHERE UbicacionId = @UbicacionId)
        UPDATE Ubicacion SET Activo = 0 WHERE UbicacionId = @UbicacionId;
    ELSE
        DELETE FROM Ubicacion WHERE UbicacionId = @UbicacionId;
END
GO

-- ── Herramientas: no se asignan catálogos desactivados ───────
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

    IF EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE CategoriaId = @CategoriaId AND Activo = 0)
        THROW 50000, 'La categoría elegida está desactivada. Elija otra.', 1;
    IF EXISTS (SELECT 1 FROM Marca WHERE MarcaId = @MarcaId AND Activo = 0)
        THROW 50000, 'La marca elegida está desactivada. Elija otra.', 1;
    IF EXISTS (SELECT 1 FROM Ubicacion WHERE UbicacionId = @UbicacionId AND Activo = 0)
        THROW 50000, 'La ubicación elegida está desactivada. Elija otra.', 1;

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
-- Una categoría, marca o ubicación desactivada se puede conservar, pero no asignar de nuevo.
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

    IF EXISTS (SELECT 1 FROM CategoriaHerramienta c INNER JOIN Herramienta h ON h.HerramientaId = @HerramientaId
               WHERE c.CategoriaId = @CategoriaId AND c.Activo = 0 AND ISNULL(h.CategoriaId, 0) <> @CategoriaId)
        THROW 50000, 'La categoría elegida está desactivada. Elija otra.', 1;
    IF EXISTS (SELECT 1 FROM Marca m INNER JOIN Herramienta h ON h.HerramientaId = @HerramientaId
               WHERE m.MarcaId = @MarcaId AND m.Activo = 0 AND ISNULL(h.MarcaId, 0) <> @MarcaId)
        THROW 50000, 'La marca elegida está desactivada. Elija otra.', 1;
    IF EXISTS (SELECT 1 FROM Ubicacion u INNER JOIN Herramienta h ON h.HerramientaId = @HerramientaId
               WHERE u.UbicacionId = @UbicacionId AND u.Activo = 0 AND ISNULL(h.UbicacionId, 0) <> @UbicacionId)
        THROW 50000, 'La ubicación elegida está desactivada. Elija otra.', 1;

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
