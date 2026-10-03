-- ============================================================
-- 011 · Dashboard (FrmDashboard)
--
-- Los KPIs salen de sp_Reporte_KPIs (010), igual que en Reportes.
-- Criterio de "vencido": préstamo abierto con fecha límite anterior a HOY.
--
--   sp_Dashboard_DistribucionUnidades   dona: estado de todas las unidades
--   sp_Dashboard_ActividadMensual       líneas: préstamos y devoluciones por día (30 días)
--   sp_Dashboard_PrestamosPorDepartamento  barras: préstamos abiertos por depto (top 6)
--   sp_Dashboard_ProximasDevoluciones   vencidas + las que vencen en los próximos 7 días
--   sp_Dashboard_HerramientasSinStock   herramientas sin unidades disponibles
--   sp_Dashboard_ActividadReciente      préstamos y devoluciones de hoy
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 011_dashboard.sql
-- ============================================================
SET XACT_ABORT ON;
GO

-- Unidades de herramientas activas (sin las dadas de baja ni perdidas).
-- Las prestadas se separan en "al día" y "vencidas" según su préstamo.
CREATE OR ALTER PROCEDURE sp_Dashboard_DistribucionUnidades
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH U AS (
        SELECT u.UnidadId, u.Estado,
               CASE WHEN u.Estado = 'Prestada' AND EXISTS (
                        SELECT 1 FROM PrestamoDetalle pd
                        INNER  JOIN Prestamo p ON p.PrestamoId = pd.PrestamoId
                        WHERE  pd.UnidadId = u.UnidadId AND pd.FechaDevuelta IS NULL
                          AND  CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE))
                    THEN 1 ELSE 0 END AS Vencida
        FROM   HerramientaUnidad u
        INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
        WHERE  h.Activa = 1
          AND  u.Estado NOT IN ('Baja', 'Perdida')
    )
    SELECT SUM(CASE WHEN Estado = 'Disponible' THEN 1 ELSE 0 END)                AS Disponibles,
           SUM(CASE WHEN Estado = 'Prestada' AND Vencida = 0 THEN 1 ELSE 0 END)  AS Prestadas,
           SUM(CASE WHEN Estado = 'Prestada' AND Vencida = 1 THEN 1 ELSE 0 END)  AS Vencidas,
           SUM(CASE WHEN Estado = 'En Mantenimiento' THEN 1 ELSE 0 END)          AS EnMantenimiento,
           SUM(CASE WHEN Estado = 'Dañada' THEN 1 ELSE 0 END)                    AS Danadas
    FROM   U;
END
GO

-- Préstamos registrados y devoluciones por día. Una "devolución" es un préstamo con
-- unidades devueltas ese día (una devolución parcial cuenta una vez por día).
CREATE OR ALTER PROCEDURE sp_Dashboard_ActividadMensual
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    ;WITH Dias AS (
        SELECT DATEADD(DAY, -n, @Hoy) AS Fecha
        FROM  (SELECT TOP 30 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS n FROM sys.all_objects) t
    ),
    Pres AS (
        SELECT CAST(FechaPrestamo AS DATE) AS Fecha, COUNT(*) AS Prestamos
        FROM   Prestamo
        WHERE  FechaPrestamo >= DATEADD(DAY, -29, @Hoy)
        GROUP  BY CAST(FechaPrestamo AS DATE)
    ),
    Dev AS (
        SELECT Fecha, COUNT(*) AS Devoluciones
        FROM  (SELECT DISTINCT PrestamoId, CAST(FechaDevuelta AS DATE) AS Fecha
               FROM   PrestamoDetalle
               WHERE  FechaDevuelta >= DATEADD(DAY, -29, @Hoy)) x
        GROUP  BY Fecha
    )
    SELECT d.Fecha,
           ISNULL(Pres.Prestamos, 0)   AS Prestamos,
           ISNULL(Dev.Devoluciones, 0) AS Devoluciones
    FROM   Dias d
    LEFT   JOIN Pres ON Pres.Fecha = d.Fecha
    LEFT   JOIN Dev  ON Dev.Fecha  = d.Fecha
    ORDER  BY d.Fecha;
END
GO

CREATE OR ALTER PROCEDURE sp_Dashboard_PrestamosPorDepartamento
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 6
           ISNULL(d.Nombre, 'Sin departamento') AS Departamento,
           COUNT(*) AS Total
    FROM   Prestamo p
    INNER  JOIN Empleado     e ON e.EmpleadoId     = p.EmpleadoId
    LEFT   JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    WHERE  p.Estado <> 'Cerrado'
    GROUP  BY d.Nombre
    ORDER  BY Total DESC, Departamento;
END
GO

-- Vencidas y las que vencen en los próximos 7 días (DiasRestantes < 0 = vencido)
CREATE OR ALTER PROCEDURE sp_Dashboard_ProximasDevoluciones
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    SELECT TOP 8
           p.PrestamoId,
           e.Codigo AS CodigoEmpleado,
           e.Nombre AS Empleado,
           (SELECT STRING_AGG(h.Nombre, N', ') WITHIN GROUP (ORDER BY h.Nombre)
            FROM   PrestamoDetalle pd
            INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
            WHERE  pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL) AS Herramientas,
           p.FechaDevolucionEsperada,
           DATEDIFF(DAY, @Hoy, CAST(p.FechaDevolucionEsperada AS DATE)) AS DiasRestantes
    FROM   Prestamo p
    INNER  JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
    WHERE  p.Estado <> 'Cerrado'
      AND  CAST(p.FechaDevolucionEsperada AS DATE) <= DATEADD(DAY, 7, @Hoy)
      AND  EXISTS (SELECT 1 FROM PrestamoDetalle pd
                   WHERE pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL)
    ORDER  BY p.FechaDevolucionEsperada, p.PrestamoId;
END
GO

-- Herramientas activas que tienen unidades pero ninguna disponible
CREATE OR ALTER PROCEDURE sp_Dashboard_HerramientasSinStock
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
           h.HerramientaId,
           h.Codigo,
           h.Nombre,
           s.StockTotal      AS Total,
           s.StockDisponible AS Disponibles
    FROM   Herramienta h
    INNER  JOIN vw_HerramientaStock s ON s.HerramientaId = h.HerramientaId
    WHERE  h.Activa = 1
      AND  s.StockTotal > 0
      AND  s.StockDisponible = 0
    ORDER  BY s.StockTotal DESC, h.Nombre;
END
GO

-- Préstamos registrados hoy y devoluciones de hoy (una por préstamo)
CREATE OR ALTER PROCEDURE sp_Dashboard_ActividadReciente
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    SELECT TOP 8 Tipo, PrestamoId, CodigoEmpleado, Empleado, Herramientas, Fecha
    FROM (
        SELECT 'Préstamo' AS Tipo,
               p.PrestamoId,
               e.Codigo AS CodigoEmpleado,
               e.Nombre AS Empleado,
               (SELECT STRING_AGG(h.Nombre, N', ') WITHIN GROUP (ORDER BY h.Nombre)
                FROM   PrestamoDetalle pd
                INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
                WHERE  pd.PrestamoId = p.PrestamoId) AS Herramientas,
               p.FechaPrestamo AS Fecha
        FROM   Prestamo p
        INNER  JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        WHERE  CAST(p.FechaPrestamo AS DATE) = @Hoy

        UNION ALL

        SELECT 'Devolución',
               p.PrestamoId,
               e.Codigo,
               e.Nombre,
               STRING_AGG(h.Nombre, N', ') WITHIN GROUP (ORDER BY h.Nombre),
               MAX(pd.FechaDevuelta)
        FROM   PrestamoDetalle pd
        INNER  JOIN Prestamo    p ON p.PrestamoId    = pd.PrestamoId
        INNER  JOIN Empleado    e ON e.EmpleadoId    = p.EmpleadoId
        INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
        WHERE  CAST(pd.FechaDevuelta AS DATE) = @Hoy
        GROUP  BY p.PrestamoId, e.Codigo, e.Nombre
    ) t
    ORDER  BY Fecha DESC;
END
GO
