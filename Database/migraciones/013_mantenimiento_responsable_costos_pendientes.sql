-- ============================================================
-- 013 · Mantenimiento: responsable, desglose de costos y pendientes
--
--   1. Responsable
--      Proveedor (nuevo catálogo, baja lógica si ya tiene historial)
--      Mantenimiento + TipoServicio ('Interno' | 'Externo'),
--                    EmpleadoId (interno) / ProveedorId (externo),
--                    RegistradoPorId / CerradoPorId (usuarios, auditoría)
--      RealizadoPor queda solo como texto histórico de los registros viejos
--
--   2. Costos
--      Mantenimiento + CostoMateriales, CostoManoObra, EnGarantia,
--                    FolioFactura, NotasCierre, Resultado ('Reparada' | 'Baja')
--      Costo pasa a ser el total (materiales + mano de obra) y lo calcula el SP.
--      Las notas de cierre ya no sobrescriben la descripción de apertura.
--      Reglas: la mano de obra interna no se registra (ya la cubre el salario),
--              en garantía no hay costo para la empresa,
--              el folio de factura solo aplica a servicios externos.
--
--   3. Pendientes
--      fn_UnidadReporteDanio: último reporte de daño (devolución) aún sin atender
--      sp_Mantenimiento_UnidadesElegibles: unidades Disponibles o Dañadas con ese reporte
--      Mantenimiento + PrestamoDetalleId: liga el mantenimiento con la devolución
--                                         que reportó el daño
--      sp_Mantenimiento_RegistrarEntrada acepta unidades de varias herramientas
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 013_mantenimiento_responsable_costos_pendientes.sql
-- ============================================================
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
GO

-- ════════════════════════════════════════════════════════════
-- 1. RESPONSABLE
-- ════════════════════════════════════════════════════════════
CREATE TABLE Proveedor (
    ProveedorId INT IDENTITY(1,1) CONSTRAINT PK_Proveedor PRIMARY KEY,
    Nombre      VARCHAR(120) NOT NULL CONSTRAINT UQ_Proveedor_Nombre UNIQUE,
    Telefono    VARCHAR(30)  NULL,
    Descripcion VARCHAR(250) NULL,
    Activo      BIT          NOT NULL CONSTRAINT DF_Proveedor_Activo DEFAULT 1
);
GO

ALTER TABLE Mantenimiento ADD
    TipoServicio    VARCHAR(10) NOT NULL CONSTRAINT DF_Mantenimiento_TipoServicio DEFAULT 'Interno',
    EmpleadoId      INT NULL CONSTRAINT FK_Mantenimiento_Empleado      REFERENCES Empleado (EmpleadoId),
    ProveedorId     INT NULL CONSTRAINT FK_Mantenimiento_Proveedor     REFERENCES Proveedor (ProveedorId),
    RegistradoPorId INT NULL CONSTRAINT FK_Mantenimiento_RegistradoPor REFERENCES Usuario (UsuarioId),
    CerradoPorId    INT NULL CONSTRAINT FK_Mantenimiento_CerradoPor    REFERENCES Usuario (UsuarioId);
GO

ALTER TABLE Mantenimiento ADD
    CONSTRAINT CK_Mantenimiento_TipoServicio CHECK (TipoServicio IN ('Interno', 'Externo')),
    -- Un servicio interno no lleva proveedor y uno externo no lleva empleado
    CONSTRAINT CK_Mantenimiento_Responsable CHECK (
        (TipoServicio = 'Interno' AND ProveedorId IS NULL) OR
        (TipoServicio = 'Externo' AND EmpleadoId  IS NULL));
GO

-- ── Proveedores ──────────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Proveedor_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProveedorId,
           p.Nombre,
           p.Telefono,
           p.Descripcion,
           (SELECT COUNT(*) FROM Mantenimiento m WHERE m.ProveedorId = p.ProveedorId) AS Mantenimientos
    FROM   Proveedor p
    WHERE  p.Activo = 1
    ORDER  BY p.Nombre ASC;
END
GO

-- Si el nombre pertenece a un proveedor dado de baja, se reactiva con los datos nuevos
CREATE OR ALTER PROCEDURE sp_Proveedor_Insertar
    @Nombre      VARCHAR(120),
    @Telefono    VARCHAR(30)  = NULL,
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = @Nombre AND Activo = 1)
        THROW 50000, 'Ya existe un proveedor con ese nombre.', 1;

    IF EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = @Nombre)
        UPDATE Proveedor
        SET    Activo = 1, Telefono = @Telefono, Descripcion = @Descripcion
        WHERE  Nombre = @Nombre;
    ELSE
        INSERT INTO Proveedor (Nombre, Telefono, Descripcion)
        VALUES (@Nombre, @Telefono, @Descripcion);
END
GO

CREATE OR ALTER PROCEDURE sp_Proveedor_Actualizar
    @ProveedorId INT,
    @Nombre      VARCHAR(120),
    @Telefono    VARCHAR(30)  = NULL,
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE ProveedorId = @ProveedorId AND Activo = 1)
        THROW 50000, 'El proveedor especificado no existe.', 1;

    IF EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = @Nombre AND ProveedorId <> @ProveedorId)
        THROW 50000, 'Ya existe otro proveedor con ese nombre.', 1;

    UPDATE Proveedor
    SET    Nombre = @Nombre, Telefono = @Telefono, Descripcion = @Descripcion
    WHERE  ProveedorId = @ProveedorId;
END
GO

-- Con mantenimientos registrados solo se desactiva, para conservar el historial
CREATE OR ALTER PROCEDURE sp_Proveedor_Eliminar
    @ProveedorId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Mantenimiento WHERE ProveedorId = @ProveedorId)
        UPDATE Proveedor SET Activo = 0 WHERE ProveedorId = @ProveedorId;
    ELSE
        DELETE FROM Proveedor WHERE ProveedorId = @ProveedorId;
END
GO

-- ════════════════════════════════════════════════════════════
-- 2. COSTOS
-- ════════════════════════════════════════════════════════════
ALTER TABLE Mantenimiento ADD
    CostoMateriales DECIMAL(10,2) NULL,
    CostoManoObra   DECIMAL(10,2) NULL,
    EnGarantia      BIT           NOT NULL CONSTRAINT DF_Mantenimiento_EnGarantia DEFAULT 0,
    FolioFactura    VARCHAR(50)   NULL,
    NotasCierre     VARCHAR(400)  NULL,
    Resultado       VARCHAR(10)   NULL;
GO

ALTER TABLE Mantenimiento ADD
    CONSTRAINT CK_Mantenimiento_CostoMateriales CHECK (CostoMateriales IS NULL OR CostoMateriales >= 0),
    CONSTRAINT CK_Mantenimiento_CostoManoObra   CHECK (CostoManoObra   IS NULL OR CostoManoObra   >= 0),
    CONSTRAINT CK_Mantenimiento_Resultado       CHECK (Resultado IS NULL OR Resultado IN ('Reparada', 'Baja'));
GO

-- ════════════════════════════════════════════════════════════
-- 3. PENDIENTES
-- ════════════════════════════════════════════════════════════
ALTER TABLE Mantenimiento ADD
    PrestamoDetalleId INT NULL CONSTRAINT FK_Mantenimiento_PrestamoDetalle
                               REFERENCES PrestamoDetalle (PrestamoDetalleId);
GO

-- Último reporte de daño de la unidad que todavía no se atiende: la devolución "Dañado"
-- posterior al inicio de su último mantenimiento. Solo aplica a unidades en estado Dañada.
CREATE OR ALTER FUNCTION fn_UnidadReporteDanio (@UnidadId INT)
RETURNS TABLE
AS
RETURN
    SELECT TOP (1)
           pd.PrestamoDetalleId,
           pd.FechaDevuelta         AS FechaReporte,
           e.Nombre                 AS ReportadoPor,
           pd.ObservacionDevolucion AS Nota
    FROM   PrestamoDetalle pd
    INNER  JOIN Prestamo p ON p.PrestamoId = pd.PrestamoId
    INNER  JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
    WHERE  pd.UnidadId         = @UnidadId
      AND  pd.EstadoDevolucion = 'Dañado'
      AND  pd.FechaDevuelta    > ISNULL((SELECT MAX(m.FechaInicio) FROM Mantenimiento m
                                         WHERE  m.UnidadId = @UnidadId), '19000101')
    ORDER  BY pd.FechaDevuelta DESC;
GO

-- Unidades que pueden entrar a mantenimiento (Disponibles o Dañadas de herramientas activas).
-- Las Dañadas son la bandeja de pendientes: van primero, con quién y cuándo reportó el daño.
CREATE OR ALTER PROCEDURE sp_Mantenimiento_UnidadesElegibles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UnidadId,
           u.HerramientaId,
           u.CodigoUnidad,
           h.Nombre  AS Herramienta,
           c.Nombre  AS Categoria,
           ub.Nombre AS Ubicacion,
           u.Estado,
           r.PrestamoDetalleId,
           r.FechaReporte,
           r.ReportadoPor,
           CASE WHEN u.Estado = 'Dañada' THEN COALESCE(r.Nota, u.Observaciones) END AS NotaReporte,
           DATEDIFF(DAY, r.FechaReporte, GETDATE()) AS DiasEsperando
    FROM   vw_HerramientaUnidad u
    INNER  JOIN Herramienta          h  ON h.HerramientaId = u.HerramientaId
    LEFT   JOIN CategoriaHerramienta c  ON c.CategoriaId   = h.CategoriaId
    LEFT   JOIN Ubicacion            ub ON ub.UbicacionId  = h.UbicacionId
    OUTER  APPLY (SELECT * FROM fn_UnidadReporteDanio(u.UnidadId) WHERE u.Estado = 'Dañada') r
    WHERE  h.Activa = 1
      AND  u.Estado IN ('Disponible', 'Dañada')
    ORDER  BY CASE WHEN u.Estado = 'Dañada' THEN 0 ELSE 1 END,
              r.FechaReporte, h.Nombre, u.Numero;
END
GO

-- ════════════════════════════════════════════════════════════
-- PROCEDIMIENTOS DE MANTENIMIENTO
-- ════════════════════════════════════════════════════════════

-- Abre un mantenimiento por unidad.
--   @UnidadIds = '3,7'  → esas unidades; pueden ser de distintas herramientas
--                         (todas deben estar Disponibles o Dañadas)
--   @UnidadIds = NULL   → todas las unidades Disponibles o Dañadas de @HerramientaId
--   Interno → @EmpleadoId obligatorio · Externo → @ProveedorId obligatorio
CREATE OR ALTER PROCEDURE sp_Mantenimiento_RegistrarEntrada
    @HerramientaId      INT          = NULL,
    @UnidadIds          VARCHAR(MAX) = NULL,
    @TipoMantenimiento  VARCHAR(50),
    @Descripcion        VARCHAR(400) = NULL,
    @TipoServicio       VARCHAR(10)  = 'Interno',
    @EmpleadoId         INT          = NULL,
    @ProveedorId        INT          = NULL,
    @RegistradoPorId    INT          = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @TipoMantenimiento NOT IN ('Preventivo', 'Correctivo', 'Calibración')
        THROW 50000, 'Tipo de mantenimiento no válido. Use: Preventivo, Correctivo o Calibración.', 1;

    IF @TipoServicio NOT IN ('Interno', 'Externo')
        THROW 50000, 'Tipo de servicio no válido. Use: Interno o Externo.', 1;

    IF @TipoServicio = 'Interno'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Empleado WHERE EmpleadoId = @EmpleadoId AND Activo = 1)
            THROW 50000, 'Seleccione el empleado que realizará el mantenimiento.', 1;
        SET @ProveedorId = NULL;
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE ProveedorId = @ProveedorId AND Activo = 1)
            THROW 50000, 'Seleccione el proveedor que realizará el mantenimiento.', 1;
        SET @EmpleadoId = NULL;
    END

    IF @HerramientaId IS NULL AND @UnidadIds IS NULL
        THROW 50000, 'Indique la herramienta o las unidades a enviar a mantenimiento.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Objetivo TABLE (UnidadId INT PRIMARY KEY, HerramientaId INT, Estado VARCHAR(20));

        IF @UnidadIds IS NULL
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
                THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

            INSERT INTO @Objetivo (UnidadId, HerramientaId, Estado)
            SELECT UnidadId, HerramientaId, Estado
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

            INSERT INTO @Objetivo (UnidadId, HerramientaId, Estado)
            SELECT u.UnidadId, u.HerramientaId, u.Estado
            FROM   HerramientaUnidad u WITH (UPDLOCK, HOLDLOCK)
            INNER  JOIN @Pedidas    p ON p.UnidadId      = u.UnidadId
            INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
            WHERE  h.Activa = 1
              AND  (@HerramientaId IS NULL OR u.HerramientaId = @HerramientaId)
              AND  u.Estado IN ('Disponible', 'Dañada');

            IF NOT EXISTS (SELECT 1 FROM @Pedidas)
               OR (SELECT COUNT(*) FROM @Objetivo) <> (SELECT COUNT(*) FROM @Pedidas)
                THROW 50000, 'Solo pueden entrar a mantenimiento unidades Disponibles o Dañadas de herramientas activas.', 1;
        END

        INSERT INTO Mantenimiento (
            HerramientaId, UnidadId, FechaInicio, TipoMantenimiento, Descripcion,
            TipoServicio, EmpleadoId, ProveedorId, RegistradoPorId, PrestamoDetalleId
        )
        SELECT o.HerramientaId, o.UnidadId, GETDATE(), @TipoMantenimiento, @Descripcion,
               @TipoServicio, @EmpleadoId, @ProveedorId, @RegistradoPorId, r.PrestamoDetalleId
        FROM   @Objetivo o
        OUTER  APPLY (SELECT * FROM fn_UnidadReporteDanio(o.UnidadId) WHERE o.Estado = 'Dañada') r;

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

-- Cierra un mantenimiento activo.
--   @Resultado: 'Disponible' (reparada) o 'Baja' (irreparable, sale del stock)
--   Costo = materiales + mano de obra
CREATE OR ALTER PROCEDURE sp_Mantenimiento_RegistrarSalida
    @MantenimientoId INT,
    @NotasCierre     VARCHAR(400)  = NULL,
    @CostoMateriales DECIMAL(10,2) = NULL,
    @CostoManoObra   DECIMAL(10,2) = NULL,
    @EnGarantia      BIT           = 0,
    @FolioFactura    VARCHAR(50)   = NULL,
    @Resultado       VARCHAR(20)   = 'Disponible',
    @CerradoPorId    INT           = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Resultado NOT IN ('Disponible', 'Baja')
        THROW 50000, 'Resultado no válido. Use: Disponible o Baja.', 1;

    IF ISNULL(@CostoMateriales, 0) < 0 OR ISNULL(@CostoManoObra, 0) < 0
        THROW 50000, 'Los costos no pueden ser negativos.', 1;

    IF @EnGarantia = 1 AND ISNULL(@CostoMateriales, 0) + ISNULL(@CostoManoObra, 0) > 0
        THROW 50000, 'Un mantenimiento en garantía no tiene costo para la empresa.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UnidadId INT, @HerramientaId INT, @TipoServicio VARCHAR(10);

        SELECT @UnidadId      = UnidadId,
               @HerramientaId = HerramientaId,
               @TipoServicio  = TipoServicio
        FROM   Mantenimiento WITH (UPDLOCK)
        WHERE  MantenimientoId = @MantenimientoId
          AND  FechaFin        IS NULL;

        IF @UnidadId IS NULL
            THROW 50000, 'El mantenimiento no existe o ya fue cerrado.', 1;

        IF @TipoServicio = 'Interno' AND ISNULL(@CostoManoObra, 0) > 0
            THROW 50000, 'La mano de obra de un servicio interno no se registra: ya la cubre el salario del empleado.', 1;

        UPDATE Mantenimiento
        SET    FechaFin        = GETDATE(),
               NotasCierre     = NULLIF(@NotasCierre, ''),
               CostoMateriales = @CostoMateriales,
               CostoManoObra   = CASE WHEN @TipoServicio = 'Externo' THEN @CostoManoObra END,
               Costo           = ISNULL(@CostoMateriales, 0) + ISNULL(@CostoManoObra, 0),
               EnGarantia      = @EnGarantia,
               FolioFactura    = CASE WHEN @TipoServicio = 'Externo' THEN NULLIF(@FolioFactura, '') END,
               Resultado       = CASE WHEN @Resultado = 'Baja' THEN 'Baja' ELSE 'Reparada' END,
               CerradoPorId    = @CerradoPorId
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

CREATE OR ALTER PROCEDURE sp_Mantenimiento_ObtenerActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.MantenimientoId,
           m.FechaInicio,
           m.TipoMantenimiento,
           m.Descripcion,
           m.TipoServicio,
           COALESCE(e.Nombre, p.Nombre, m.RealizadoPor) AS RealizadoPor,
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
    LEFT   JOIN Empleado             e  ON m.EmpleadoId    = e.EmpleadoId
    LEFT   JOIN Proveedor            p  ON m.ProveedorId   = p.ProveedorId
    LEFT   JOIN CategoriaHerramienta c  ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca                mk ON h.MarcaId       = mk.MarcaId
    LEFT   JOIN Ubicacion            u  ON h.UbicacionId   = u.UbicacionId
    WHERE  m.FechaFin IS NULL
    ORDER  BY m.FechaInicio ASC;
END
GO

-- Historial de una herramienta. Los alias son los encabezados de la ventana de historial.
CREATE OR ALTER PROCEDURE sp_Mantenimiento_ObtenerPorHerramienta
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.CodigoUnidad                                 AS Unidad,
           m.FechaInicio                                  AS Inicio,
           m.FechaFin                                     AS Cierre,
           DATEDIFF(DAY, m.FechaInicio, ISNULL(m.FechaFin, GETDATE())) AS [Días],
           m.TipoMantenimiento                            AS Tipo,
           m.TipoServicio                                 AS Servicio,
           COALESCE(e.Nombre, p.Nombre, m.RealizadoPor)   AS [Realizado por],
           m.Descripcion                                  AS [Descripción],
           m.NotasCierre                                  AS [Notas de cierre],
           CASE WHEN m.FechaFin IS NULL THEN 'En curso' ELSE ISNULL(m.Resultado, 'Finalizado') END AS Resultado,
           m.CostoMateriales                              AS Materiales,
           m.CostoManoObra                                AS [Mano de obra],
           m.Costo                                        AS [Costo total],
           CASE WHEN m.EnGarantia = 1 THEN 'Sí' ELSE 'No' END AS [Garantía],
           m.FolioFactura                                 AS Factura,
           de.Nombre                                      AS [Daño reportado por]
    FROM   Mantenimiento m
    INNER  JOIN vw_HerramientaUnidad u  ON u.UnidadId            = m.UnidadId
    LEFT   JOIN Empleado             e  ON e.EmpleadoId          = m.EmpleadoId
    LEFT   JOIN Proveedor            p  ON p.ProveedorId         = m.ProveedorId
    LEFT   JOIN PrestamoDetalle      pd ON pd.PrestamoDetalleId  = m.PrestamoDetalleId
    LEFT   JOIN Prestamo             pr ON pr.PrestamoId         = pd.PrestamoId
    LEFT   JOIN Empleado             de ON de.EmpleadoId         = pr.EmpleadoId
    WHERE  m.HerramientaId = @HerramientaId
    ORDER  BY m.FechaInicio DESC;
END
GO

-- @Estado: 'Activo' (sin cerrar) | 'Cerrado' | NULL
CREATE OR ALTER PROCEDURE sp_Reporte_HistorialMantenimiento
    @FechaDesde DATE        = NULL,
    @FechaHasta DATE        = NULL,
    @Tipo       VARCHAR(50) = NULL,
    @Estado     VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.MantenimientoId,
           m.HerramientaId,
           h.Nombre             AS Herramienta,
           u.CodigoUnidad,
           m.TipoMantenimiento  AS Tipo,
           m.TipoServicio       AS Servicio,
           COALESCE(e.Nombre, p.Nombre, m.RealizadoPor) AS RealizadoPor,
           m.FechaInicio,
           m.FechaFin,
           m.CostoMateriales,
           m.CostoManoObra,
           m.Costo,
           m.EnGarantia,
           m.Resultado,
           m.Descripcion,
           DATEDIFF(DAY, m.FechaInicio, ISNULL(m.FechaFin, GETDATE())) AS Duracion,
           CASE WHEN m.FechaFin IS NULL THEN 'Activo' ELSE 'Cerrado' END AS Estado
    FROM   Mantenimiento m
    INNER  JOIN Herramienta          h ON h.HerramientaId = m.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = m.UnidadId
    LEFT   JOIN Empleado             e ON e.EmpleadoId    = m.EmpleadoId
    LEFT   JOIN Proveedor            p ON p.ProveedorId   = m.ProveedorId
    WHERE  (@FechaDesde IS NULL OR m.FechaInicio >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR m.FechaInicio <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@Tipo IS NULL OR m.TipoMantenimiento = @Tipo)
      AND  (@Estado IS NULL
            OR (@Estado = 'Activo'  AND m.FechaFin IS NULL)
            OR (@Estado = 'Cerrado' AND m.FechaFin IS NOT NULL))
    ORDER  BY m.FechaInicio DESC;
END
GO

COMMIT TRANSACTION;
GO
