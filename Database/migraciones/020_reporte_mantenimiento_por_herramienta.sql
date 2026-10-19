-- ============================================================
-- 020 · Reporte de historial de mantenimiento filtrable por herramienta
--   sp_Reporte_HistorialMantenimiento recibe @HerramientaId (opcional) para ver
--   todo lo que se le ha hecho a una herramienta: costo acumulado, frecuencia
--   de reparaciones y técnicos que la atendieron. NULL = todas.
--
-- Ejecutar con:  sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i 020_reporte_mantenimiento_por_herramienta.sql
-- ============================================================

-- @Estado: 'Activo' (sin cerrar) | 'Cerrado' | NULL
CREATE OR ALTER PROCEDURE sp_Reporte_HistorialMantenimiento
    @HerramientaId INT      = NULL,
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
    WHERE  (@HerramientaId IS NULL OR m.HerramientaId = @HerramientaId)
      AND  (@FechaDesde IS NULL OR m.FechaInicio >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR m.FechaInicio <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@Tipo IS NULL OR m.TipoMantenimiento = @Tipo)
      AND  (@Estado IS NULL
            OR (@Estado = 'Activo'  AND m.FechaFin IS NULL)
            OR (@Estado = 'Cerrado' AND m.FechaFin IS NOT NULL))
    ORDER  BY m.FechaInicio DESC;
END
GO
