-- ============================================================
-- 021 · Daños y pérdidas marcados en almacén
--   Marcar una unidad como Dañada o Perdida desde "Unidades" (sin préstamo de por medio,
--   por ejemplo por desgaste normal o al revisar el almacén) solo cambiaba su estado: no
--   quedaba cuándo, quién lo registró ni la nota, y no aparecía en el reporte de dañadas
--   y perdidas, que solo leía devoluciones.
--     · UnidadIncidente guarda cada daño o pérdida marcado en almacén.
--     · sp_Unidad_CambiarEstado lo registra (@RegistradoPorId = usuario en sesión).
--     · sp_Reporte_DanadasPerdidas los incluye con Origen = 'Almacén' y sin responsable.
--     · El reporte agrega la situación actual de cada caso (sigue dañada, en reparación,
--       reparada, dada de baja...) y el filtro @Situacion, para distinguir lo vigente
--       del historial.
--     · fn_UnidadReporteDanio: la bandeja de pendientes muestra desde cuándo espera.
--
-- Ejecutar con:  sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i 021_incidentes_en_almacen.sql
-- ============================================================
SET XACT_ABORT ON;
GO

IF OBJECT_ID('dbo.UnidadIncidente') IS NULL
BEGIN
    CREATE TABLE UnidadIncidente (
        IncidenteId     INT IDENTITY(1,1) CONSTRAINT PK_UnidadIncidente PRIMARY KEY,
        UnidadId        INT           NOT NULL,
        HerramientaId   INT           NOT NULL,
        Tipo            VARCHAR(20)   NOT NULL CONSTRAINT CK_UnidadIncidente_Tipo CHECK (Tipo IN ('Dañada', 'Perdida')),
        Fecha           DATETIME2(0)  NOT NULL CONSTRAINT DF_UnidadIncidente_Fecha DEFAULT (SYSDATETIME()),
        Nota            NVARCHAR(250) NULL,
        RegistradoPorId INT           NULL CONSTRAINT FK_UnidadIncidente_Usuario REFERENCES Usuario (UsuarioId),
        CONSTRAINT FK_UnidadIncidente_Unidad FOREIGN KEY (UnidadId, HerramientaId)
            REFERENCES HerramientaUnidad (UnidadId, HerramientaId)
    );
    CREATE INDEX IX_UnidadIncidente_UnidadId ON UnidadIncidente (UnidadId) INCLUDE (Fecha);
END
GO

-- Último reporte de daño de la unidad que todavía no se atiende, posterior al inicio de su
-- último mantenimiento: la devolución "Dañado" (ReportadoPor = quien la devolvió) o el daño
-- marcado en almacén sin préstamo de por medio (UnidadIncidente; ReportadoPor = NULL).
-- Solo aplica a unidades en estado Dañada.
CREATE OR ALTER FUNCTION fn_UnidadReporteDanio (@UnidadId INT)
RETURNS TABLE
AS
RETURN
    SELECT TOP (1) x.PrestamoDetalleId, x.FechaReporte, x.ReportadoPor, x.Nota
    FROM  (SELECT pd.PrestamoDetalleId,
                  pd.FechaDevuelta         AS FechaReporte,
                  e.Nombre                 AS ReportadoPor,
                  pd.ObservacionDevolucion AS Nota
           FROM   PrestamoDetalle pd
           INNER  JOIN Prestamo p ON p.PrestamoId = pd.PrestamoId
           INNER  JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
           WHERE  pd.UnidadId = @UnidadId AND pd.EstadoDevolucion = 'Dañado'
           UNION  ALL
           SELECT NULL, CAST(i.Fecha AS DATETIME2(7)), NULL, i.Nota
           FROM   UnidadIncidente i
           WHERE  i.UnidadId = @UnidadId AND i.Tipo = 'Dañada') x
    WHERE  x.FechaReporte > ISNULL((SELECT MAX(m.FechaInicio) FROM Mantenimiento m
                                    WHERE  m.UnidadId = @UnidadId), '19000101')
    ORDER  BY x.FechaReporte DESC;
GO

-- Cambia el estado de unidades de una herramienta.
--   @UnidadIds = '3,7,8'  → solo esas unidades (todas deben poder cambiar)
--   @UnidadIds = NULL     → todo el grupo (se omiten las que no pueden cambiar)
-- Estados manuales: Disponible, Dañada, Perdida, Baja.
-- Prestada / En Mantenimiento solo cambian por préstamo, devolución o mantenimiento.
-- Dar de baja el grupo completo además desactiva la herramienta.
-- Marcar Dañada o Perdida deja constancia en UnidadIncidente (cuándo, quién lo registró y la nota),
-- para que un daño o pérdida sin préstamo de por medio aparezca en reportes.
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Unidad_CambiarEstado
    @HerramientaId   INT,
    @UnidadIds       VARCHAR(MAX)  = NULL,
    @NuevoEstado     VARCHAR(20),
    @Observacion     NVARCHAR(250) = NULL,
    @RegistradoPorId INT           = NULL
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

        DECLARE @Cambiadas TABLE (UnidadId INT PRIMARY KEY, HerramientaId INT);

        UPDATE u
        SET    Estado        = @NuevoEstado,
               FechaBaja     = CASE WHEN @NuevoEstado = 'Baja' THEN SYSDATETIME() END,
               Observaciones = ISNULL(NULLIF(@Observacion, N''), u.Observaciones)
        OUTPUT inserted.UnidadId, inserted.HerramientaId INTO @Cambiadas
        FROM   HerramientaUnidad u
        INNER  JOIN @Objetivo    o ON o.UnidadId = u.UnidadId
        WHERE  o.Estado NOT IN ('Prestada', 'En Mantenimiento', 'Baja')
          AND  o.Estado <> @NuevoEstado;

        DECLARE @Afectadas INT = @@ROWCOUNT;

        IF @NuevoEstado IN ('Dañada', 'Perdida')
            INSERT INTO UnidadIncidente (UnidadId, HerramientaId, Tipo, Nota, RegistradoPorId)
            SELECT UnidadId, HerramientaId, @NuevoEstado, NULLIF(@Observacion, N''), @RegistradoPorId
            FROM   @Cambiadas;

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

-- @Condicion: 'Dañada' | 'Perdida' | NULL
-- Incluye las devoluciones dañadas o perdidas (con el empleado responsable) y los daños o
-- pérdidas marcados en almacén sin préstamo de por medio (Origen = 'Almacén', sin responsable).
-- Al filtrar por empleado solo quedan las devoluciones.
--
-- Situacion: qué pasó después con la pieza, para distinguir lo vigente del historial.
--   Se mira lo primero que le ocurrió después del reporte: un mantenimiento o un nuevo préstamo.
--     · mantenimiento → 'En reparación' (abierto), 'Reparada' o 'Dada de baja'
--     · nuevo préstamo sin mantenimiento antes → 'Resuelta' (o 'Recuperada' si estaba perdida)
--     · nada todavía → según su estado actual: 'Sigue dañada', 'Sigue perdida', 'Dada de baja'
--       o 'Resuelta' / 'Recuperada' (se marcó otra vez en buen estado)
-- @Situacion: 'SinResolver' (sigue dañada, en reparación o sigue perdida) | 'Resuelta' | NULL
CREATE OR ALTER PROCEDURE sp_Reporte_DanadasPerdidas
    @FechaDesde DATE        = NULL,
    @FechaHasta DATE        = NULL,
    @Condicion  VARCHAR(20) = NULL,
    @EmpleadoId INT         = NULL,
    @Situacion  VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT x.*, st.Situacion
    FROM  (SELECT h.Nombre        AS Herramienta,
                  u.CodigoUnidad,
                  CASE pd.EstadoDevolucion WHEN 'Dañado' THEN 'Dañada' ELSE 'Perdida' END AS Condicion,
                  pd.ObservacionDevolucion AS Nota,
                  'Devolución'    AS Origen,
                  p.PrestamoId,
                  p.EmpleadoId,
                  e.Codigo        AS CodigoEmpleado,
                  e.Nombre        AS Empleado,
                  d.Nombre        AS Departamento,
                  CAST(NULL AS NVARCHAR(120)) AS RegistradoPor,
                  pd.FechaDevuelta AS FechaDevolucion,
                  pd.UnidadId
           FROM   PrestamoDetalle pd
           INNER  JOIN Prestamo             p ON p.PrestamoId     = pd.PrestamoId
           INNER  JOIN Empleado             e ON e.EmpleadoId     = p.EmpleadoId
           LEFT   JOIN Departamento         d ON d.DepartamentoId = e.DepartamentoId
           INNER  JOIN Herramienta          h ON h.HerramientaId  = pd.HerramientaId
           INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId       = pd.UnidadId
           WHERE  pd.EstadoDevolucion IN ('Dañado', 'Perdido')
           UNION  ALL
           SELECT h.Nombre, u.CodigoUnidad, i.Tipo, i.Nota, 'Almacén',
                  NULL, NULL, NULL, NULL, NULL,
                  re.Nombre,
                  CAST(i.Fecha AS DATETIME2(7)),
                  i.UnidadId
           FROM   UnidadIncidente i
           INNER  JOIN Herramienta          h  ON h.HerramientaId = i.HerramientaId
           INNER  JOIN vw_HerramientaUnidad u  ON u.UnidadId      = i.UnidadId
           LEFT   JOIN Usuario              us ON us.UsuarioId    = i.RegistradoPorId
           LEFT   JOIN Empleado             re ON re.EmpleadoId   = us.EmpleadoId) x
    INNER  JOIN HerramientaUnidad hu ON hu.UnidadId = x.UnidadId
    OUTER  APPLY (SELECT TOP (1) m.FechaInicio, m.FechaFin, m.Resultado
                  FROM   Mantenimiento m
                  WHERE  m.UnidadId = x.UnidadId AND m.FechaInicio >= x.FechaDevolucion
                  ORDER  BY m.FechaInicio) m
    OUTER  APPLY (SELECT MIN(p2.FechaPrestamo) AS Fecha
                  FROM   PrestamoDetalle d2
                  INNER  JOIN Prestamo p2 ON p2.PrestamoId = d2.PrestamoId
                  WHERE  d2.UnidadId = x.UnidadId AND p2.FechaPrestamo > x.FechaDevolucion) sig
    CROSS  APPLY (SELECT CASE
                      WHEN m.FechaInicio IS NOT NULL AND (sig.Fecha IS NULL OR m.FechaInicio <= sig.Fecha)
                          THEN CASE WHEN m.FechaFin IS NULL THEN 'En reparación'
                                    WHEN m.Resultado = 'Baja' THEN 'Dada de baja'
                                    ELSE 'Reparada' END
                      WHEN sig.Fecha IS NOT NULL
                          THEN CASE WHEN x.Condicion = 'Perdida' THEN 'Recuperada' ELSE 'Resuelta' END
                      WHEN hu.Estado = 'Dañada'  THEN 'Sigue dañada'
                      WHEN hu.Estado = 'Perdida' THEN 'Sigue perdida'
                      WHEN hu.Estado = 'Baja'    THEN 'Dada de baja'
                      WHEN x.Condicion = 'Perdida' THEN 'Recuperada'
                      ELSE 'Resuelta' END AS Situacion) st
    WHERE  (@FechaDesde IS NULL OR x.FechaDevolucion >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR x.FechaDevolucion <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@Condicion  IS NULL OR x.Condicion = @Condicion)
      AND  (@EmpleadoId IS NULL OR x.EmpleadoId = @EmpleadoId)
      AND  (@Situacion  IS NULL
            OR (@Situacion = 'SinResolver' AND st.Situacion IN ('Sigue dañada', 'En reparación', 'Sigue perdida'))
            OR (@Situacion = 'Resuelta'    AND st.Situacion NOT IN ('Sigue dañada', 'En reparación', 'Sigue perdida')))
    ORDER  BY x.FechaDevolucion DESC;
END
GO
