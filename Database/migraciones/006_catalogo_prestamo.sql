-- ============================================================
-- 006 · Catálogo para la nueva pantalla de préstamos
--
-- Una fila por herramienta prestable (activa y con préstamo habilitado),
-- incluidas las que hoy no tienen unidades disponibles, para que el
-- catálogo pueda mostrarlas como "Sin disponibles".
-- Las unidades concretas siguen saliendo de sp_Herramienta_ObtenerDisponibles.
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 006_catalogo_prestamo.sql
-- ============================================================
SET XACT_ABORT ON;
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
           ISNULL(s.StockTotal, 0)      AS StockTotal
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
