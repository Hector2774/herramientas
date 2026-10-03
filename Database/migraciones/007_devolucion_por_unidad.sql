-- ============================================================
-- 007 · Devolución de varias unidades con condición y nota por unidad
--
-- sp_Devolucion_RegistrarVarias recibe en un solo XML las unidades a
-- devolver de UN préstamo, cada una con su condición y su nota:
--   <items>
--     <item id="PrestamoDetalleId" estado="Bueno|Dañado|Perdido" nota="..."/>
--   </items>
-- Todo o nada: si alguna unidad ya fue devuelta o no pertenece al
-- préstamo, no se registra ninguna. Al final cierra el préstamo si ya
-- no quedan pendientes (sp_Prestamo_CerrarSiCompleto).
--   Bueno → Disponible · Dañado → Dañada · Perdido → Perdida
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 007_devolucion_por_unidad.sql
-- ============================================================
SET XACT_ABORT ON;
GO

CREATE OR ALTER PROCEDURE sp_Devolucion_RegistrarVarias
    @PrestamoId INT,
    @Items      XML
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Devolver TABLE (
        PrestamoDetalleId INT PRIMARY KEY,
        Estado            VARCHAR(20),
        Nota              NVARCHAR(250),
        UnidadId          INT NULL
    );

    INSERT INTO @Devolver (PrestamoDetalleId, Estado, Nota)
    SELECT x.item.value('@id', 'INT'),
           x.item.value('@estado', 'VARCHAR(20)'),
           NULLIF(LTRIM(RTRIM(x.item.value('@nota', 'NVARCHAR(250)'))), N'')
    FROM   @Items.nodes('/items/item') AS x(item);

    IF NOT EXISTS (SELECT 1 FROM @Devolver)
        THROW 50000, 'Seleccione al menos una herramienta a devolver.', 1;

    IF EXISTS (SELECT 1 FROM @Devolver WHERE Estado IS NULL OR Estado NOT IN ('Bueno', 'Dañado', 'Perdido'))
        THROW 50000, 'Estado de devolución no válido. Use: Bueno, Dañado o Perdido.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Prestamo WITH (UPDLOCK)
                       WHERE PrestamoId = @PrestamoId AND Estado <> 'Cerrado')
            THROW 50000, 'El préstamo no existe o ya está cerrado.', 1;

        UPDATE d
        SET    d.UnidadId = pd.UnidadId
        FROM   @Devolver d
        INNER  JOIN PrestamoDetalle pd WITH (UPDLOCK)
                ON pd.PrestamoDetalleId = d.PrestamoDetalleId
               AND pd.PrestamoId        = @PrestamoId
               AND pd.FechaDevuelta     IS NULL;

        IF EXISTS (SELECT 1 FROM @Devolver WHERE UnidadId IS NULL)
            THROW 50000, 'Una o más herramientas ya fueron devueltas o no pertenecen a este préstamo.', 1;

        UPDATE pd
        SET    pd.FechaDevuelta         = GETDATE(),
               pd.EstadoDevolucion      = d.Estado,
               pd.ObservacionDevolucion = d.Nota
        FROM   PrestamoDetalle pd
        INNER  JOIN @Devolver  d ON d.PrestamoDetalleId = pd.PrestamoDetalleId;

        UPDATE u
        SET    u.Estado = CASE d.Estado
                              WHEN 'Bueno'  THEN 'Disponible'
                              WHEN 'Dañado' THEN 'Dañada'
                              ELSE               'Perdida'
                          END
        FROM   HerramientaUnidad u
        INNER  JOIN @Devolver   d ON d.UnidadId = u.UnidadId;

        EXEC sp_Prestamo_CerrarSiCompleto @PrestamoId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    -- Resumen para el mensaje de confirmación
    SELECT SUM(CASE WHEN Estado = 'Bueno'  THEN 1 ELSE 0 END) AS Buenas,
           SUM(CASE WHEN Estado = 'Dañado' THEN 1 ELSE 0 END) AS Dañadas,
           SUM(CASE WHEN Estado = 'Perdido' THEN 1 ELSE 0 END) AS Perdidas,
           (SELECT COUNT(*) FROM PrestamoDetalle
            WHERE  PrestamoId = @PrestamoId AND FechaDevuelta IS NULL) AS Pendientes,
           CAST(CASE WHEN EXISTS (SELECT 1 FROM Prestamo
                                  WHERE PrestamoId = @PrestamoId AND Estado = 'Cerrado')
                     THEN 1 ELSE 0 END AS BIT) AS PrestamoCerrado
    FROM   @Devolver;
END
GO
