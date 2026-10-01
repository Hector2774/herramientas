-- ============================================================
-- 002 · Stock por unidades individuales
--
-- Antes: cada fila de Herramienta era un TIPO de herramienta con
-- contadores (StockTotal / StockDisponible) y un único Estado para
-- todo el grupo. Marcar una pieza como dañada bloqueaba el grupo.
--
-- Ahora:
--   Herramienta        = el grupo / tipo (catálogo)
--       Activa             0 = grupo dado de baja (oculto)
--       PrestamoHabilitado 0 = el grupo no se presta (suspendido)
--   HerramientaUnidad  = cada pieza física, con su propio Estado:
--       Disponible | Prestada | En Mantenimiento | Dañada | Perdida | Baja
--   El stock se calcula contando unidades (vw_HerramientaStock).
--   Préstamos y mantenimientos apuntan a una unidad concreta.
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 002_stock_por_unidades.sql
-- ============================================================
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
GO

-- ── Herramienta: quitar estado y contadores del grupo ────────
ALTER TABLE Herramienta DROP CONSTRAINT CK_Herramienta_Estado, CK_Herramienta_StockTotal, CK_Herramienta_StockDisponible;
DROP INDEX IX_Herramienta_Estado ON Herramienta;

DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += N'ALTER TABLE Herramienta DROP CONSTRAINT ' + QUOTENAME(dc.name) + N';'
FROM   sys.default_constraints dc
JOIN   sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
WHERE  dc.parent_object_id = OBJECT_ID('Herramienta')
  AND  c.name IN ('Estado', 'StockTotal', 'StockDisponible');
EXEC (@sql);

ALTER TABLE Herramienta DROP COLUMN Estado, StockTotal, StockDisponible;
ALTER TABLE Herramienta ADD PrestamoHabilitado BIT NOT NULL
    CONSTRAINT DF_Herramienta_PrestamoHabilitado DEFAULT 1;
GO

-- ── Unidades físicas ─────────────────────────────────────────
CREATE TABLE HerramientaUnidad (
    UnidadId       INT IDENTITY(1,1) CONSTRAINT PK_HerramientaUnidad PRIMARY KEY,
    HerramientaId  INT          NOT NULL CONSTRAINT FK_Unidad_Herramienta REFERENCES Herramienta(HerramientaId),
    Numero         INT          NOT NULL,
    Estado         VARCHAR(20)  NOT NULL CONSTRAINT DF_Unidad_Estado DEFAULT 'Disponible',
    FechaAlta      DATETIME2    NOT NULL CONSTRAINT DF_Unidad_FechaAlta DEFAULT SYSDATETIME(),
    FechaBaja      DATETIME2    NULL,
    Observaciones  NVARCHAR(250) NULL,
    CONSTRAINT UQ_Unidad_Numero UNIQUE (HerramientaId, Numero),
    -- Permite FKs compuestas que garantizan que la unidad pertenece a la herramienta
    CONSTRAINT UQ_Unidad_Herramienta UNIQUE (UnidadId, HerramientaId),
    CONSTRAINT CK_Unidad_Numero CHECK (Numero > 0),
    CONSTRAINT CK_Unidad_Estado CHECK (Estado IN ('Disponible', 'Prestada', 'En Mantenimiento', 'Dañada', 'Perdida', 'Baja')),
    CONSTRAINT CK_Unidad_FechaBaja CHECK ((Estado = 'Baja' AND FechaBaja IS NOT NULL) OR (Estado <> 'Baja' AND FechaBaja IS NULL))
);
CREATE INDEX IX_Unidad_Herramienta_Estado ON HerramientaUnidad(HerramientaId, Estado);
CREATE INDEX IX_Unidad_Estado ON HerramientaUnidad(Estado) INCLUDE (HerramientaId);
GO

-- ── PrestamoDetalle: ahora por unidad (se permiten varias del mismo tipo) ──
ALTER TABLE PrestamoDetalle DROP CONSTRAINT UQ_Prestamo_Herramienta;
ALTER TABLE PrestamoDetalle ADD UnidadId INT NOT NULL;
GO
ALTER TABLE PrestamoDetalle ADD
    CONSTRAINT FK_Detalle_Unidad FOREIGN KEY (UnidadId, HerramientaId) REFERENCES HerramientaUnidad(UnidadId, HerramientaId),
    CONSTRAINT UQ_Detalle_Prestamo_Unidad UNIQUE (PrestamoId, UnidadId);
CREATE INDEX IX_Detalle_Unidad ON PrestamoDetalle(UnidadId) INCLUDE (FechaDevuelta);
GO

-- ── Mantenimiento: por unidad ────────────────────────────────
ALTER TABLE Mantenimiento ADD UnidadId INT NOT NULL;
GO
ALTER TABLE Mantenimiento ADD
    CONSTRAINT FK_Mantenimiento_Unidad FOREIGN KEY (UnidadId, HerramientaId) REFERENCES HerramientaUnidad(UnidadId, HerramientaId);
-- Una unidad solo puede tener un mantenimiento abierto
CREATE UNIQUE INDEX UQ_Mantenimiento_UnidadAbierta ON Mantenimiento(UnidadId) WHERE FechaFin IS NULL;
GO

COMMIT TRANSACTION;
GO

-- ============================================================
-- VISTAS
-- ============================================================
CREATE OR ALTER VIEW vw_HerramientaUnidad
AS
SELECT u.UnidadId,
       u.HerramientaId,
       u.Numero,
       h.Codigo + N'-' + CASE WHEN u.Numero < 100
                              THEN RIGHT('0' + CAST(u.Numero AS VARCHAR(10)), 2)
                              ELSE CAST(u.Numero AS VARCHAR(10)) END AS CodigoUnidad,
       u.Estado,
       u.FechaAlta,
       u.FechaBaja,
       u.Observaciones
FROM   HerramientaUnidad u
INNER  JOIN Herramienta  h ON h.HerramientaId = u.HerramientaId;
GO

-- StockTotal = unidades que siguen siendo de la empresa (no perdidas ni dadas de baja)
CREATE OR ALTER VIEW vw_HerramientaStock
AS
SELECT h.HerramientaId,
       SUM(CASE WHEN u.Estado NOT IN ('Baja', 'Perdida') THEN 1 ELSE 0 END) AS StockTotal,
       SUM(CASE WHEN u.Estado = 'Disponible' THEN 1 ELSE 0 END) AS StockDisponible,
       SUM(CASE WHEN u.Estado = 'Prestada' THEN 1 ELSE 0 END) AS StockPrestado,
       SUM(CASE WHEN u.Estado = 'En Mantenimiento' THEN 1 ELSE 0 END) AS StockMantenimiento,
       SUM(CASE WHEN u.Estado = 'Dañada' THEN 1 ELSE 0 END) AS StockDañado,
       SUM(CASE WHEN u.Estado = 'Perdida' THEN 1 ELSE 0 END) AS StockPerdido,
       SUM(CASE WHEN u.Estado = 'Baja' THEN 1 ELSE 0 END) AS StockBaja
FROM   Herramienta h
LEFT   JOIN HerramientaUnidad u ON u.HerramientaId = h.HerramientaId
GROUP  BY h.HerramientaId;
GO

-- ============================================================
-- HERRAMIENTAS (grupo)
-- ============================================================
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
           h.UbicacionId
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
           h.UbicacionId
    FROM   Herramienta    h
    INNER  JOIN vw_HerramientaStock    s ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca                m ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            u ON h.UbicacionId   = u.UbicacionId
    WHERE  h.HerramientaId = @HerramientaId;
END
GO

CREATE OR ALTER PROCEDURE sp_Herramienta_Buscar
    @Termino VARCHAR(120)
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
           h.UbicacionId
    FROM   Herramienta    h
    INNER  JOIN vw_HerramientaStock    s ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca                m ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            u ON h.UbicacionId   = u.UbicacionId
    WHERE  h.Activa = 1
      AND (h.Nombre  LIKE '%' + @Termino + '%'
        OR h.Codigo  LIKE '%' + @Termino + '%')
    ORDER  BY h.Nombre ASC;
END
GO

-- Crea @Cantidad unidades nuevas (Disponible) numeradas a continuación de la última.
-- Si se llama dentro de otra transacción, queda anidada en ella.
CREATE OR ALTER PROCEDURE sp_Herramienta_AgregarUnidades
    @HerramientaId INT,
    @Cantidad      INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Cantidad IS NULL OR @Cantidad < 1 OR @Cantidad > 999
        THROW 50000, 'La cantidad de unidades a agregar debe estar entre 1 y 999.', 1;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
        THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Ultimo INT = ISNULL((
            SELECT MAX(Numero)
            FROM   HerramientaUnidad WITH (UPDLOCK, HOLDLOCK)
            WHERE  HerramientaId = @HerramientaId), 0);

        INSERT INTO HerramientaUnidad (HerramientaId, Numero)
        SELECT @HerramientaId, @Ultimo + g.value
        FROM   GENERATE_SERIES(1, @Cantidad) g;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_Herramienta_Insertar
    @Codigo          VARCHAR(30),
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

    IF EXISTS (SELECT 1 FROM Herramienta WHERE Codigo = @Codigo)
        THROW 50000, 'Ya existe una herramienta con ese código.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Herramienta (
            Codigo, Nombre, Caracteristicas,
            CategoriaId, MarcaId, UbicacionId, Activa, PrestamoHabilitado
        )
        VALUES (
            @Codigo, @Nombre, @Caracteristicas,
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

    SELECT @HerramientaId AS HerramientaId;
END
GO

-- @StockTotal mayor al actual agrega unidades nuevas.
-- Para reducir el stock hay que dar de baja (o marcar perdidas) unidades concretas.
CREATE OR ALTER PROCEDURE sp_Herramienta_Actualizar
    @HerramientaId   INT,
    @Codigo          VARCHAR(30),
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

    IF EXISTS (SELECT 1 FROM Herramienta WHERE Codigo = @Codigo AND HerramientaId <> @HerramientaId)
        THROW 50000, 'Ya existe otra herramienta con ese código.', 1;

    DECLARE @StockActual INT = (SELECT StockTotal FROM vw_HerramientaStock WHERE HerramientaId = @HerramientaId);

    IF @StockTotal IS NOT NULL AND @StockTotal < @StockActual
        THROW 50000, 'Para reducir el stock, dé de baja o marque como perdidas las unidades concretas desde "Unidades".', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Herramienta
        SET    Codigo          = @Codigo,
               Nombre          = @Nombre,
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

-- Suspende o reanuda el préstamo de TODO el grupo (las unidades conservan su estado).
CREATE OR ALTER PROCEDURE sp_Herramienta_HabilitarPrestamo
    @HerramientaId INT,
    @Habilitar     BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
        THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

    UPDATE Herramienta
    SET    PrestamoHabilitado = @Habilitar
    WHERE  HerramientaId = @HerramientaId;
END
GO

CREATE OR ALTER PROCEDURE sp_Herramienta_ObtenerUnidades
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UnidadId,
           u.CodigoUnidad,
           u.Estado,
           pr.Empleado      AS PrestadaA,
           pr.FechaDevolucionEsperada,
           u.FechaAlta,
           u.FechaBaja,
           u.Observaciones
    FROM   vw_HerramientaUnidad u
    OUTER  APPLY (
        SELECT TOP 1 e.Nombre AS Empleado, p.FechaDevolucionEsperada
        FROM   PrestamoDetalle pd
        INNER  JOIN Prestamo   p ON p.PrestamoId = pd.PrestamoId
        INNER  JOIN Empleado   e ON e.EmpleadoId = p.EmpleadoId
        WHERE  pd.UnidadId = u.UnidadId
          AND  pd.FechaDevuelta IS NULL
    ) pr
    WHERE  u.HerramientaId = @HerramientaId
    ORDER  BY u.Numero;
END
GO

-- ============================================================
-- Cambia el estado de unidades de una herramienta.
--   @UnidadIds = '3,7,8'  → solo esas unidades (todas deben poder cambiar)
--   @UnidadIds = NULL     → todo el grupo (se omiten las que no pueden cambiar)
-- Estados manuales: Disponible, Dañada, Perdida, Baja.
-- Prestada / En Mantenimiento solo cambian por préstamo, devolución o mantenimiento.
-- Dar de baja el grupo completo además desactiva la herramienta.
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Unidad_CambiarEstado
    @HerramientaId INT,
    @UnidadIds     VARCHAR(MAX)  = NULL,
    @NuevoEstado   VARCHAR(20),
    @Observacion   NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @NuevoEstado NOT IN ('Disponible', 'Dañada', 'Perdida', 'Baja')
        THROW 50000, 'Estado no válido. Use: Disponible, Dañada, Perdida o Baja.', 1;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
        THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Objetivo TABLE (UnidadId INT PRIMARY KEY, Estado VARCHAR(20));

        IF @UnidadIds IS NULL
        BEGIN
            INSERT INTO @Objetivo (UnidadId, Estado)
            SELECT UnidadId, Estado
            FROM   HerramientaUnidad WITH (UPDLOCK, HOLDLOCK)
            WHERE  HerramientaId = @HerramientaId
              AND  Estado <> 'Baja';

            IF @NuevoEstado = 'Baja'
               AND EXISTS (SELECT 1 FROM @Objetivo WHERE Estado IN ('Prestada', 'En Mantenimiento'))
                THROW 50000, 'No se puede dar de baja todo el grupo: hay unidades prestadas o en mantenimiento.', 1;
        END
        ELSE
        BEGIN
            DECLARE @Pedidas TABLE (UnidadId INT PRIMARY KEY);
            INSERT INTO @Pedidas (UnidadId)
            SELECT DISTINCT TRY_CAST(value AS INT)
            FROM   STRING_SPLIT(@UnidadIds, ',')
            WHERE  TRY_CAST(value AS INT) IS NOT NULL;

            INSERT INTO @Objetivo (UnidadId, Estado)
            SELECT u.UnidadId, u.Estado
            FROM   HerramientaUnidad u WITH (UPDLOCK, HOLDLOCK)
            INNER  JOIN @Pedidas p ON p.UnidadId = u.UnidadId
            WHERE  u.HerramientaId = @HerramientaId;

            IF NOT EXISTS (SELECT 1 FROM @Pedidas)
               OR (SELECT COUNT(*) FROM @Objetivo) <> (SELECT COUNT(*) FROM @Pedidas)
                THROW 50000, 'Una o más unidades no pertenecen a esta herramienta.', 1;

            IF EXISTS (SELECT 1 FROM @Objetivo WHERE Estado IN ('Prestada', 'En Mantenimiento', 'Baja'))
                THROW 50000, 'Hay unidades prestadas, en mantenimiento o ya dadas de baja. Registre primero la devolución o cierre el mantenimiento.', 1;
        END

        UPDATE u
        SET    Estado        = @NuevoEstado,
               FechaBaja     = CASE WHEN @NuevoEstado = 'Baja' THEN SYSDATETIME() END,
               Observaciones = ISNULL(NULLIF(@Observacion, N''), u.Observaciones)
        FROM   HerramientaUnidad u
        INNER  JOIN @Objetivo    o ON o.UnidadId = u.UnidadId
        WHERE  o.Estado NOT IN ('Prestada', 'En Mantenimiento', 'Baja')
          AND  o.Estado <> @NuevoEstado;

        DECLARE @Afectadas INT = @@ROWCOUNT;

        IF @UnidadIds IS NULL AND @NuevoEstado = 'Baja'
            UPDATE Herramienta
            SET    Activa = 0, PrestamoHabilitado = 0
            WHERE  HerramientaId = @HerramientaId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT @Afectadas                                  AS Afectadas,
           (SELECT COUNT(*) FROM @Objetivo) - @Afectadas AS Omitidas;
END
GO

-- Baja del grupo completo (todas sus unidades). Para una sola unidad usar sp_Unidad_CambiarEstado.
CREATE OR ALTER PROCEDURE sp_Herramienta_DarDeBaja
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;
    EXEC sp_Unidad_CambiarEstado @HerramientaId = @HerramientaId, @UnidadIds = NULL, @NuevoEstado = 'Baja';
END
GO

-- Una fila por unidad disponible para préstamo.
CREATE OR ALTER PROCEDURE sp_Herramienta_ObtenerDisponibles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UnidadId,
           h.HerramientaId,
           u.CodigoUnidad  AS Codigo,
           h.Nombre,
           c.Nombre        AS Categoria,
           m.NombreMarca   AS Marca,
           ub.Nombre       AS Ubicacion
    FROM   vw_HerramientaUnidad u
    INNER  JOIN Herramienta          h  ON h.HerramientaId = u.HerramientaId
    LEFT   JOIN CategoriaHerramienta c  ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca                m  ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            ub ON h.UbicacionId   = ub.UbicacionId
    WHERE  u.Estado             = 'Disponible'
      AND  h.Activa             = 1
      AND  h.PrestamoHabilitado = 1
    ORDER  BY h.Nombre ASC, u.Numero ASC;
END
GO

-- ============================================================
-- PRÉSTAMOS
--   @Herramientas: '<herramientas><item id="UnidadId"/>...</herramientas>'
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Prestamo_Registrar
    @EmpleadoId              INT,
    @AprobadoPorId           INT,
    @FechaDevolucionEsperada DATETIME,
    @Observaciones           VARCHAR(400) = NULL,
    @Herramientas            XML
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM Empleado WHERE EmpleadoId = @EmpleadoId AND Activo = 1)
        THROW 50000, 'El empleado no existe o no está activo.', 1;

    IF NOT EXISTS (SELECT 1 FROM Empleado WHERE EmpleadoId = @AprobadoPorId AND Activo = 1)
        THROW 50000, 'El aprobador no existe o no está activo.', 1;

    IF @FechaDevolucionEsperada <= GETDATE()
        THROW 50000, 'La fecha de devolución esperada debe ser posterior a hoy.', 1;

    DECLARE @Items TABLE (UnidadId INT PRIMARY KEY);

    INSERT INTO @Items (UnidadId)
    SELECT DISTINCT x.item.value('@id', 'INT')
    FROM   @Herramientas.nodes('/herramientas/item') AS x(item);

    IF NOT EXISTS (SELECT 1 FROM @Items)
        THROW 50000, 'Debe seleccionar al menos una herramienta.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Todas las unidades deben existir, estar Disponibles y su grupo habilitado para préstamo
        IF (SELECT COUNT(*)
            FROM   @Items i
            INNER  JOIN HerramientaUnidad u WITH (UPDLOCK, HOLDLOCK) ON u.UnidadId = i.UnidadId
            INNER  JOIN Herramienta       h ON h.HerramientaId = u.HerramientaId
            WHERE  u.Estado             = 'Disponible'
              AND  h.Activa             = 1
              AND  h.PrestamoHabilitado = 1) <> (SELECT COUNT(*) FROM @Items)
            THROW 50000, 'Una o más herramientas ya no están disponibles para préstamo.', 1;

        INSERT INTO Prestamo (
            EmpleadoId, AprobadoPorId,
            FechaPrestamo, FechaDevolucionEsperada,
            Estado, Observaciones
        )
        VALUES (
            @EmpleadoId, @AprobadoPorId,
            GETDATE(), @FechaDevolucionEsperada,
            'Activo', @Observaciones
        );

        DECLARE @PrestamoId INT = SCOPE_IDENTITY();

        INSERT INTO PrestamoDetalle (PrestamoId, HerramientaId, UnidadId, EstadoDevolucion)
        SELECT @PrestamoId, u.HerramientaId, u.UnidadId, 'Pendiente'
        FROM   @Items i
        INNER  JOIN HerramientaUnidad u ON u.UnidadId = i.UnidadId;

        UPDATE HerramientaUnidad
        SET    Estado = 'Prestada'
        WHERE  UnidadId IN (SELECT UnidadId FROM @Items);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT @PrestamoId AS PrestamoId;
END
GO

CREATE OR ALTER PROCEDURE sp_Prestamo_ObtenerDetalle
    @PrestamoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PrestamoId,
           p.FechaPrestamo,
           p.FechaDevolucionEsperada,
           p.FechaCierre,
           p.Estado,
           p.Observaciones,
           e.EmpleadoId,
           e.Nombre   AS Empleado,
           e.Codigo   AS CodigoEmpleado,
           a.Nombre   AS AprobadoPor,
           d.Nombre   AS Departamento
    FROM   Prestamo p
    INNER  JOIN Empleado    e ON p.EmpleadoId    = e.EmpleadoId
    LEFT   JOIN Empleado    a ON p.AprobadoPorId = a.EmpleadoId
    LEFT   JOIN Departamento d ON e.DepartamentoId = d.DepartamentoId
    WHERE  p.PrestamoId = @PrestamoId;

    SELECT pd.PrestamoDetalleId,
           pd.HerramientaId,
           pd.UnidadId,
           u.CodigoUnidad        AS CodigoHerramienta,
           h.Nombre              AS Herramienta,
           pd.EstadoDevolucion,
           pd.FechaDevuelta,
           pd.ObservacionDevolucion,
           CASE WHEN pd.FechaDevuelta IS NULL THEN 'Pendiente' ELSE 'Devuelta' END AS Situacion
    FROM   PrestamoDetalle pd
    INNER  JOIN Herramienta          h ON pd.HerramientaId = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON pd.UnidadId      = u.UnidadId
    WHERE  pd.PrestamoId = @PrestamoId
    ORDER  BY h.Nombre ASC, u.Numero ASC;
END
GO

-- ============================================================
-- DEVOLUCIONES  (solo afectan a la unidad devuelta)
--   Bueno → Disponible · Dañado → Dañada · Perdido → Perdida
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Devolucion_RegistrarUna
    @PrestamoDetalleId       INT,
    @EstadoDevolucion        VARCHAR(20),
    @ObservacionDevolucion   VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @EstadoDevolucion NOT IN ('Bueno', 'Dañado', 'Perdido')
        THROW 50000, 'Estado de devolución no válido. Use: Bueno, Dañado o Perdido.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UnidadId INT, @PrestamoId INT;

        SELECT @UnidadId   = UnidadId,
               @PrestamoId = PrestamoId
        FROM   PrestamoDetalle WITH (UPDLOCK)
        WHERE  PrestamoDetalleId = @PrestamoDetalleId
          AND  FechaDevuelta     IS NULL;

        IF @UnidadId IS NULL
            THROW 50000, 'Este artículo ya fue devuelto o no existe.', 1;

        UPDATE PrestamoDetalle
        SET    FechaDevuelta          = GETDATE(),
               EstadoDevolucion       = @EstadoDevolucion,
               ObservacionDevolucion  = @ObservacionDevolucion
        WHERE  PrestamoDetalleId      = @PrestamoDetalleId;

        UPDATE HerramientaUnidad
        SET    Estado = CASE @EstadoDevolucion
                            WHEN 'Bueno'  THEN 'Disponible'
                            WHEN 'Dañado' THEN 'Dañada'
                            ELSE               'Perdida'
                        END
        WHERE  UnidadId = @UnidadId;

        EXEC sp_Prestamo_CerrarSiCompleto @PrestamoId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT 'OK' AS Resultado, @PrestamoId AS PrestamoId;
END
GO

CREATE OR ALTER PROCEDURE sp_Devolucion_RegistrarTodas
    @PrestamoId            INT,
    @EstadoDevolucion      VARCHAR(20),
    @ObservacionDevolucion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @EstadoDevolucion NOT IN ('Bueno', 'Dañado', 'Perdido')
        THROW 50000, 'Estado de devolución no válido. Use: Bueno, Dañado o Perdido.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1 FROM Prestamo WITH (UPDLOCK)
            WHERE  PrestamoId = @PrestamoId
              AND  Estado     <> 'Cerrado'
        )
            THROW 50000, 'El préstamo no existe o ya está cerrado.', 1;

        DECLARE @Pendientes TABLE (PrestamoDetalleId INT PRIMARY KEY, UnidadId INT);

        INSERT INTO @Pendientes
        SELECT PrestamoDetalleId, UnidadId
        FROM   PrestamoDetalle
        WHERE  PrestamoId    = @PrestamoId
          AND  FechaDevuelta IS NULL;

        UPDATE PrestamoDetalle
        SET    FechaDevuelta         = GETDATE(),
               EstadoDevolucion      = @EstadoDevolucion,
               ObservacionDevolucion = @ObservacionDevolucion
        WHERE  PrestamoDetalleId IN (SELECT PrestamoDetalleId FROM @Pendientes);

        UPDATE HerramientaUnidad
        SET    Estado = CASE @EstadoDevolucion
                            WHEN 'Bueno'  THEN 'Disponible'
                            WHEN 'Dañado' THEN 'Dañada'
                            ELSE               'Perdida'
                        END
        WHERE  UnidadId IN (SELECT UnidadId FROM @Pendientes);

        EXEC sp_Prestamo_CerrarSiCompleto @PrestamoId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT 'OK' AS Resultado, @PrestamoId AS PrestamoId;
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
           pd.EstadoDevolucion
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

    IF @TipoMantenimiento NOT IN ('Preventivo', 'Correctivo')
        THROW 50000, 'Tipo de mantenimiento no válido. Use: Preventivo o Correctivo.', 1;

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

-- @Resultado: 'Disponible' (reparada) o 'Baja' (irreparable, sale del stock)
CREATE OR ALTER PROCEDURE sp_Mantenimiento_RegistrarSalida
    @MantenimientoId INT,
    @Descripcion     VARCHAR(400)  = NULL,
    @Costo           DECIMAL(10,2) = NULL,
    @Resultado       VARCHAR(20)   = 'Disponible'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Resultado NOT IN ('Disponible', 'Baja')
        THROW 50000, 'Resultado no válido. Use: Disponible o Baja.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UnidadId INT, @HerramientaId INT;

        SELECT @UnidadId      = UnidadId,
               @HerramientaId = HerramientaId
        FROM   Mantenimiento WITH (UPDLOCK)
        WHERE  MantenimientoId = @MantenimientoId
          AND  FechaFin        IS NULL;

        IF @UnidadId IS NULL
            THROW 50000, 'El mantenimiento no existe o ya fue cerrado.', 1;

        UPDATE Mantenimiento
        SET    FechaFin    = GETDATE(),
               Descripcion = ISNULL(NULLIF(@Descripcion, ''), Descripcion),
               Costo       = @Costo
        WHERE  MantenimientoId = @MantenimientoId;

        UPDATE HerramientaUnidad
        SET    Estado    = @Resultado,
               FechaBaja = CASE WHEN @Resultado = 'Baja' THEN SYSDATETIME() END
        WHERE  UnidadId  = @UnidadId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT 'OK' AS Resultado, @HerramientaId AS HerramientaId;
END
GO

CREATE OR ALTER PROCEDURE sp_Mantenimiento_ObtenerPorHerramienta
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.MantenimientoId,
           u.CodigoUnidad AS Unidad,
           m.FechaInicio,
           m.FechaFin,
           m.TipoMantenimiento,
           m.Descripcion,
           m.RealizadoPor,
           m.Costo,
           CASE
               WHEN m.FechaFin IS NULL THEN 'En curso'
               ELSE 'Finalizado'
           END AS Estado,
           DATEDIFF(DAY, m.FechaInicio, ISNULL(m.FechaFin, GETDATE())) AS DiasDuracion
    FROM   Mantenimiento m
    INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId = m.UnidadId
    WHERE  m.HerramientaId = @HerramientaId
    ORDER  BY m.FechaInicio DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Mantenimiento_ObtenerActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.MantenimientoId,
           m.FechaInicio,
           m.TipoMantenimiento,
           m.Descripcion,
           m.RealizadoPor,
           h.HerramientaId,
           m.UnidadId,
           un.CodigoUnidad AS CodigoHerramienta,
           h.Nombre        AS Herramienta,
           c.Nombre        AS Categoria,
           mk.NombreMarca  AS Marca,
           u.Nombre        AS Ubicacion,
           DATEDIFF(DAY, m.FechaInicio, GETDATE()) AS DiasEnMantenimiento
    FROM   Mantenimiento          m
    INNER  JOIN Herramienta          h  ON m.HerramientaId = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad un ON m.UnidadId      = un.UnidadId
    LEFT   JOIN CategoriaHerramienta c  ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca                mk ON h.MarcaId       = mk.MarcaId
    LEFT   JOIN Ubicacion            u  ON h.UbicacionId   = u.UbicacionId
    WHERE  m.FechaFin IS NULL
    ORDER  BY m.FechaInicio ASC;
END
GO

-- ============================================================
-- REPORTES
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Reporte_HistorialPrestamos
    @EmpleadoId    INT          = NULL,
    @HerramientaId INT          = NULL,
    @FechaDesde    DATETIME     = NULL,
    @FechaHasta    DATETIME     = NULL,
    @Estado        VARCHAR(20)  = NULL,
    @DepartamentoId INT         = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.PrestamoId,
        p.FechaPrestamo,
        p.FechaDevolucionEsperada,
        p.FechaCierre,
        p.Estado                        AS EstadoPrestamo,
        e.Codigo                        AS CodigoEmpleado,
        e.Nombre                        AS Empleado,
        d.Nombre                        AS Departamento,
        ap.Nombre                       AS AprobadoPor,
        un.CodigoUnidad                 AS CodigoHerramienta,
        h.Nombre                        AS Herramienta,
        c.Nombre                        AS Categoria,
        m.NombreMarca                   AS Marca,
        pd.EstadoDevolucion,
        pd.FechaDevuelta,
        pd.ObservacionDevolucion,
        DATEDIFF(DAY, p.FechaPrestamo,
            ISNULL(pd.FechaDevuelta, GETDATE()))    AS DiasPrestado,
        CASE
            WHEN pd.FechaDevuelta IS NULL
             AND p.FechaDevolucionEsperada < GETDATE()
            THEN DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE())
            WHEN pd.FechaDevuelta IS NOT NULL
             AND pd.FechaDevuelta > p.FechaDevolucionEsperada
            THEN DATEDIFF(DAY, p.FechaDevolucionEsperada, pd.FechaDevuelta)
            ELSE 0
        END                             AS DiasAtraso
    FROM   Prestamo               p
    INNER  JOIN PrestamoDetalle   pd ON p.PrestamoId      = pd.PrestamoId
    INNER  JOIN Empleado          e  ON p.EmpleadoId      = e.EmpleadoId
    LEFT   JOIN Empleado          ap ON p.AprobadoPorId   = ap.EmpleadoId
    LEFT   JOIN Departamento      d  ON e.DepartamentoId  = d.DepartamentoId
    INNER  JOIN Herramienta       h  ON pd.HerramientaId  = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad un ON pd.UnidadId    = un.UnidadId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca             m  ON h.MarcaId         = m.MarcaId
    WHERE  (@EmpleadoId     IS NULL OR p.EmpleadoId      = @EmpleadoId)
      AND  (@HerramientaId  IS NULL OR pd.HerramientaId  = @HerramientaId)
      AND  (@FechaDesde     IS NULL OR p.FechaPrestamo   >= @FechaDesde)
      AND  (@FechaHasta     IS NULL OR p.FechaPrestamo   <= DATEADD(DAY, 1, @FechaHasta))
      AND  (@Estado         IS NULL OR p.Estado          = @Estado)
      AND  (@DepartamentoId IS NULL OR e.DepartamentoId  = @DepartamentoId)
    ORDER  BY p.FechaPrestamo DESC, h.Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE sp_Reporte_PrestamosVencidos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.PrestamoId,
        p.FechaPrestamo,
        p.FechaDevolucionEsperada,
        DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE()) AS DiasAtraso,
        e.Codigo    AS CodigoEmpleado,
        e.Nombre    AS Empleado,
        d.Nombre    AS Departamento,
        (
            SELECT STRING_AGG(h2.Nombre + N' (' + u2.CodigoUnidad + N')', ', ')
            FROM   PrestamoDetalle pd2
            INNER  JOIN Herramienta          h2 ON pd2.HerramientaId = h2.HerramientaId
            INNER  JOIN vw_HerramientaUnidad u2 ON pd2.UnidadId      = u2.UnidadId
            WHERE  pd2.PrestamoId   = p.PrestamoId
              AND  pd2.FechaDevuelta IS NULL
        )           AS HerramientasPendientes,
        (
            SELECT COUNT(*)
            FROM   PrestamoDetalle pd3
            WHERE  pd3.PrestamoId   = p.PrestamoId
              AND  pd3.FechaDevuelta IS NULL
        )           AS CantidadPendiente
    FROM   Prestamo      p
    INNER  JOIN Empleado     e ON p.EmpleadoId     = e.EmpleadoId
    LEFT   JOIN Departamento d ON e.DepartamentoId = d.DepartamentoId
    WHERE  p.Estado IN ('Activo', 'Vencido')
      AND  p.FechaDevolucionEsperada < GETDATE()
      AND  EXISTS (
               SELECT 1 FROM PrestamoDetalle pd
               WHERE  pd.PrestamoId   = p.PrestamoId
                 AND  pd.FechaDevuelta IS NULL
           )
    ORDER  BY DiasAtraso DESC;
END
GO

-- Una fila por unidad dañada, perdida o en mantenimiento correctivo
CREATE OR ALTER PROCEDURE sp_Reporte_HerramientasDañadas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.HerramientaId,
        u.UnidadId,
        u.CodigoUnidad            AS CodigoHerramienta,
        h.Nombre                  AS Herramienta,
        u.Estado,
        c.Nombre                  AS Categoria,
        m.NombreMarca             AS Marca,
        lp.FechaPrestamo          AS FechaUltimoPrestamo,
        lp.FechaDevolucionEsperada,
        pd.FechaDevuelta          AS FechaReporteDaño,
        pd.ObservacionDevolucion  AS ObservacionDaño,
        le.Nombre                 AS EmpleadoResponsable,
        ld.Nombre                 AS DepartamentoResponsable,
        CASE WHEN ma.MantenimientoId IS NOT NULL
             THEN 'Sí' ELSE 'No'
        END                       AS EnMantenimiento,
        ma.FechaInicio            AS FechaInicioMantenimiento
    FROM   vw_HerramientaUnidad   u
    INNER  JOIN Herramienta       h  ON h.HerramientaId    = u.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId    = c.CategoriaId
    LEFT   JOIN Marca             m  ON h.MarcaId          = m.MarcaId
    OUTER  APPLY (
        SELECT TOP 1 pd2.PrestamoId,
                     pd2.FechaDevuelta,
                     pd2.ObservacionDevolucion
        FROM   PrestamoDetalle pd2
        WHERE  pd2.UnidadId          = u.UnidadId
          AND  pd2.EstadoDevolucion  IN ('Dañado', 'Perdido')
        ORDER  BY pd2.FechaDevuelta  DESC
    ) pd
    LEFT   JOIN Prestamo      lp ON lp.PrestamoId      = pd.PrestamoId
    LEFT   JOIN Empleado      le ON lp.EmpleadoId      = le.EmpleadoId
    LEFT   JOIN Departamento  ld ON le.DepartamentoId  = ld.DepartamentoId
    LEFT   JOIN Mantenimiento ma ON ma.UnidadId        = u.UnidadId
                                AND ma.FechaFin        IS NULL
    WHERE  h.Activa = 1
      AND (u.Estado IN ('Dañada', 'Perdida')
        OR (u.Estado = 'En Mantenimiento' AND ma.TipoMantenimiento = 'Correctivo'))
    ORDER  BY u.Estado ASC, h.Nombre ASC, u.Numero ASC;
END
GO
