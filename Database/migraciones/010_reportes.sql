-- ============================================================
-- 010 · Reportes rediseñados (FrmReportes)
--
-- Los SPs sp_Reporte_* anteriores se conservan sin cambios.
-- Criterio de "vencido" en estos reportes: préstamo abierto cuya fecha
-- límite es anterior a HOY (igual que la pantalla de Devoluciones).
--
--   sp_Reporte_KPIs                    tiles superiores + cambios de hoy
--   sp_Reporte_HistorialPorPrestamo    una fila por préstamo, herramientas concatenadas
--   sp_Reporte_Vencidos                préstamos abiertos vencidos (pendientes)
--   sp_Reporte_PrestamosPorEmpleado    resumen por empleado en un período
--   sp_Reporte_RankingHerramientas     herramientas más usadas
--   sp_Reporte_HistorialMantenimiento  mantenimientos (activos y cerrados)
--   sp_Reporte_DanadasPerdidas         unidades devueltas dañadas o perdidas
--   sp_Reporte_InventarioCategoria     snapshot de stock por herramienta
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 010_reportes.sql
-- ============================================================
SET XACT_ABORT ON;
GO

CREATE OR ALTER PROCEDURE sp_Reporte_KPIs
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy  DATE = CAST(GETDATE() AS DATE);
    DECLARE @Ayer DATE = DATEADD(DAY, -1, @Hoy);

    ;WITH Abiertos AS (
        SELECT p.PrestamoId, p.FechaPrestamo, CAST(p.FechaDevolucionEsperada AS DATE) AS Limite
        FROM   Prestamo p
        WHERE  p.Estado <> 'Cerrado'
          AND  EXISTS (SELECT 1 FROM PrestamoDetalle pd
                       WHERE pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL)
    ),
    Unidades AS (
        SELECT u.Estado
        FROM   HerramientaUnidad u
        INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
        WHERE  h.Activa = 1
    )
    SELECT
        (SELECT COUNT(*) FROM Abiertos WHERE Limite <  @Hoy)                    AS Vencidos,
        (SELECT COUNT(*) FROM Abiertos WHERE Limite =  @Ayer)                   AS VencidosHoy,
        (SELECT COUNT(*) FROM Abiertos)                                         AS Activos,
        (SELECT COUNT(*) FROM Abiertos WHERE CAST(FechaPrestamo AS DATE) = @Hoy) AS ActivosHoy,
        (SELECT COUNT(*) FROM Unidades WHERE Estado = 'Disponible')             AS Disponibles,
        (SELECT COUNT(*) FROM PrestamoDetalle
         WHERE  CAST(FechaDevuelta AS DATE) = @Hoy AND EstadoDevolucion = 'Bueno') AS DevueltasHoy,
        (SELECT COUNT(*) FROM Unidades WHERE Estado = 'En Mantenimiento')       AS EnMantenimiento,
        (SELECT COUNT(*) FROM Mantenimiento WHERE CAST(FechaInicio AS DATE) = @Hoy) AS MantenimientoHoy,
        (SELECT COUNT(*) FROM Unidades WHERE Estado = 'Dañada')                 AS Danadas,
        (SELECT COUNT(*) FROM PrestamoDetalle
         WHERE  CAST(FechaDevuelta AS DATE) = @Hoy AND EstadoDevolucion = 'Dañado') AS DanadasHoy;
END
GO

-- @Estado: 'Activo' | 'Cerrado' | 'Vencido' | NULL
CREATE OR ALTER PROCEDURE sp_Reporte_HistorialPorPrestamo
    @EmpleadoId    INT         = NULL,
    @HerramientaId INT         = NULL,
    @FechaDesde    DATE        = NULL,
    @FechaHasta    DATE        = NULL,
    @Estado        VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Base AS (
        SELECT p.PrestamoId,
               e.Codigo   AS CodigoEmpleado,
               e.Nombre   AS Empleado,
               d.Nombre   AS Departamento,
               p.FechaPrestamo,
               p.FechaDevolucionEsperada,
               p.FechaCierre,
               ap.Nombre  AS AprobadoPor,
               CASE WHEN p.Estado = 'Cerrado' THEN 'Cerrado'
                    WHEN CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencido'
                    ELSE 'Activo' END AS Estado,
               (SELECT STRING_AGG(h.Nombre + N' ' + u.CodigoUnidad, N', ')
                       WITHIN GROUP (ORDER BY h.Nombre, u.Numero)
                FROM   PrestamoDetalle pd
                INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
                INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = pd.UnidadId
                WHERE  pd.PrestamoId = p.PrestamoId) AS Herramientas
        FROM   Prestamo p
        INNER  JOIN Empleado     e  ON e.EmpleadoId     = p.EmpleadoId
        LEFT   JOIN Departamento d  ON d.DepartamentoId = e.DepartamentoId
        LEFT   JOIN Empleado     ap ON ap.EmpleadoId    = p.AprobadoPorId
        WHERE  (@EmpleadoId IS NULL OR p.EmpleadoId = @EmpleadoId)
          AND  (@HerramientaId IS NULL OR EXISTS (SELECT 1 FROM PrestamoDetalle pd
                                                  WHERE pd.PrestamoId = p.PrestamoId
                                                    AND pd.HerramientaId = @HerramientaId))
          AND  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
          AND  (@FechaHasta IS NULL OR p.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))
    )
    SELECT * FROM Base
    WHERE  (@Estado IS NULL OR Estado = @Estado)
    ORDER  BY FechaPrestamo DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Reporte_Vencidos
    @EmpleadoId  INT = NULL,
    @CategoriaId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PrestamoId,
           e.Codigo   AS CodigoEmpleado,
           e.Nombre   AS Empleado,
           d.Nombre   AS Departamento,
           p.FechaDevolucionEsperada,
           DATEDIFF(DAY, CAST(p.FechaDevolucionEsperada AS DATE), CAST(GETDATE() AS DATE)) AS DiasVencido,
           (SELECT STRING_AGG(h.Nombre + N' ' + u.CodigoUnidad, N', ')
                   WITHIN GROUP (ORDER BY h.Nombre, u.Numero)
            FROM   PrestamoDetalle pd
            INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
            INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = pd.UnidadId
            WHERE  pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL) AS Herramientas
    FROM   Prestamo p
    INNER  JOIN Empleado     e ON e.EmpleadoId     = p.EmpleadoId
    LEFT   JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    WHERE  p.Estado <> 'Cerrado'
      AND  CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE)
      AND  EXISTS (SELECT 1 FROM PrestamoDetalle pd
                   WHERE pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL)
      AND  (@EmpleadoId IS NULL OR p.EmpleadoId = @EmpleadoId)
      AND  (@CategoriaId IS NULL OR EXISTS (
               SELECT 1 FROM PrestamoDetalle pd
               INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
               WHERE  pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL
                 AND  h.CategoriaId = @CategoriaId))
    ORDER  BY DiasVencido DESC, p.PrestamoId;
END
GO

CREATE OR ALTER PROCEDURE sp_Reporte_PrestamosPorEmpleado
    @FechaDesde     DATE = NULL,
    @FechaHasta     DATE = NULL,
    @DepartamentoId INT  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.EmpleadoId,
           e.Codigo AS CodigoEmpleado,
           e.Nombre AS Empleado,
           d.Nombre AS Departamento,
           COUNT(*) AS TotalPrestamos,
           SUM(CASE WHEN p.Estado <> 'Cerrado' THEN 1 ELSE 0 END) AS Activos,
           SUM(CASE WHEN p.Estado <> 'Cerrado'
                     AND CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE)
                    THEN 1 ELSE 0 END) AS Vencidos,
           SUM(CASE WHEN p.Estado = 'Cerrado' THEN 1 ELSE 0 END) AS Devueltos,
           (SELECT COUNT(DISTINCT pd.HerramientaId)
            FROM   PrestamoDetalle pd
            INNER  JOIN Prestamo p2 ON p2.PrestamoId = pd.PrestamoId
            WHERE  p2.EmpleadoId = e.EmpleadoId
              AND  (@FechaDesde IS NULL OR p2.FechaPrestamo >= @FechaDesde)
              AND  (@FechaHasta IS NULL OR p2.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))) AS HerramientasUnicas
    FROM   Prestamo p
    INNER  JOIN Empleado     e ON e.EmpleadoId     = p.EmpleadoId
    LEFT   JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    WHERE  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR p.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@DepartamentoId IS NULL OR e.DepartamentoId = @DepartamentoId)
    GROUP  BY e.EmpleadoId, e.Codigo, e.Nombre, d.Nombre
    ORDER  BY TotalPrestamos DESC, e.Nombre;
END
GO

CREATE OR ALTER PROCEDURE sp_Reporte_RankingHerramientas
    @FechaDesde  DATE = NULL,
    @FechaHasta  DATE = NULL,
    @CategoriaId INT  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre  AS Herramienta,
           c.Nombre  AS Categoria,
           ISNULL(s.StockTotal, 0) AS TotalUnidades,
           COUNT(DISTINCT p.PrestamoId) AS TotalPrestamos,
           -- días por unidad prestada (hasta su devolución, o hasta hoy si sigue prestada)
           CAST(AVG(CAST(DATEDIFF(DAY, p.FechaPrestamo, ISNULL(pd.FechaDevuelta, GETDATE())) AS FLOAT))
                AS DECIMAL(8,1)) AS DiasPromedio
    FROM   PrestamoDetalle pd
    INNER  JOIN Prestamo             p ON p.PrestamoId    = pd.PrestamoId
    INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON c.CategoriaId   = h.CategoriaId
    LEFT   JOIN vw_HerramientaStock  s ON s.HerramientaId = h.HerramientaId
    WHERE  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR p.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@CategoriaId IS NULL OR h.CategoriaId = @CategoriaId)
    GROUP  BY h.HerramientaId, h.Codigo, h.Nombre, c.Nombre, s.StockTotal
    ORDER  BY TotalPrestamos DESC, DiasPromedio DESC, h.Nombre;
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
           m.RealizadoPor,
           m.FechaInicio,
           m.FechaFin,
           m.Costo,
           m.Descripcion,
           DATEDIFF(DAY, m.FechaInicio, ISNULL(m.FechaFin, GETDATE())) AS Duracion,
           CASE WHEN m.FechaFin IS NULL THEN 'Activo' ELSE 'Cerrado' END AS Estado
    FROM   Mantenimiento m
    INNER  JOIN Herramienta          h ON h.HerramientaId = m.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = m.UnidadId
    WHERE  (@FechaDesde IS NULL OR m.FechaInicio >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR m.FechaInicio <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@Tipo IS NULL OR m.TipoMantenimiento = @Tipo)
      AND  (@Estado IS NULL
            OR (@Estado = 'Activo'  AND m.FechaFin IS NULL)
            OR (@Estado = 'Cerrado' AND m.FechaFin IS NOT NULL))
    ORDER  BY m.FechaInicio DESC;
END
GO

-- @Condicion: 'Dañada' | 'Perdida' | NULL
CREATE OR ALTER PROCEDURE sp_Reporte_DanadasPerdidas
    @FechaDesde DATE        = NULL,
    @FechaHasta DATE        = NULL,
    @Condicion  VARCHAR(20) = NULL,
    @EmpleadoId INT         = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT h.Nombre        AS Herramienta,
           u.CodigoUnidad,
           CASE pd.EstadoDevolucion WHEN 'Dañado' THEN 'Dañada' ELSE 'Perdida' END AS Condicion,
           pd.ObservacionDevolucion AS Nota,
           p.PrestamoId,
           e.Codigo        AS CodigoEmpleado,
           e.Nombre        AS Empleado,
           d.Nombre        AS Departamento,
           pd.FechaDevuelta AS FechaDevolucion
    FROM   PrestamoDetalle pd
    INNER  JOIN Prestamo             p ON p.PrestamoId    = pd.PrestamoId
    INNER  JOIN Empleado             e ON e.EmpleadoId    = p.EmpleadoId
    LEFT   JOIN Departamento         d ON d.DepartamentoId = e.DepartamentoId
    INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = pd.UnidadId
    WHERE  pd.EstadoDevolucion IN ('Dañado', 'Perdido')
      AND  (@FechaDesde IS NULL OR pd.FechaDevuelta >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR pd.FechaDevuelta <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@Condicion IS NULL
            OR (@Condicion = 'Dañada'  AND pd.EstadoDevolucion = 'Dañado')
            OR (@Condicion = 'Perdida' AND pd.EstadoDevolucion = 'Perdido'))
      AND  (@EmpleadoId IS NULL OR p.EmpleadoId = @EmpleadoId)
    ORDER  BY pd.FechaDevuelta DESC;
END
GO

-- @Mostrar: 'Todas' | 'StockBajo' (< 30 % disponible) | 'SinDisponibles'
CREATE OR ALTER PROCEDURE sp_Reporte_InventarioCategoria
    @CategoriaId INT         = NULL,
    @Mostrar     VARCHAR(20) = 'Todas'
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Inv AS (
        SELECT h.HerramientaId,
               h.Codigo,
               h.Nombre  AS Herramienta,
               c.Nombre  AS Categoria,
               s.StockTotal         AS Total,
               s.StockDisponible    AS Disponibles,
               s.StockPrestado      AS Prestadas,
               s.StockMantenimiento AS EnMantenimiento,
               s.StockDañado        AS Danadas,
               CAST(CASE WHEN s.StockTotal > 0 THEN 100.0 * s.StockDisponible / s.StockTotal ELSE 0 END
                    AS DECIMAL(5,1)) AS PctDisponible
        FROM   Herramienta h
        INNER  JOIN vw_HerramientaStock  s ON s.HerramientaId = h.HerramientaId
        LEFT   JOIN CategoriaHerramienta c ON c.CategoriaId   = h.CategoriaId
        WHERE  h.Activa = 1
          AND  (@CategoriaId IS NULL OR h.CategoriaId = @CategoriaId)
    )
    SELECT * FROM Inv
    WHERE  ISNULL(@Mostrar, 'Todas') = 'Todas'
       OR  (@Mostrar = 'StockBajo'      AND PctDisponible < 30)
       OR  (@Mostrar = 'SinDisponibles' AND Disponibles = 0)
    ORDER  BY PctDisponible ASC, Herramienta;
END
GO
