-- ============================================================
-- 012 · Ubicaciones administrables desde Catálogos
--
--   sp_Ubicacion_ObtenerTodas
--       + columna Herramientas (cuántas herramientas activas la usan),
--       igual que categorías y marcas
--   sp_Ubicacion_Eliminar (nuevo)
--       desliga las herramientas y borra el registro en una transacción
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 012_catalogo_ubicaciones.sql
-- ============================================================
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
GO

CREATE OR ALTER PROCEDURE sp_Ubicacion_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UbicacionId,
           u.Nombre,
           u.Descripcion,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.UbicacionId = u.UbicacionId AND h.Activa = 1) AS Herramientas
    FROM   Ubicacion u
    ORDER  BY u.Nombre ASC;
END
GO

CREATE OR ALTER PROCEDURE sp_Ubicacion_Eliminar
    @UbicacionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Herramienta SET UbicacionId = NULL WHERE UbicacionId = @UbicacionId;
        DELETE FROM Ubicacion WHERE UbicacionId = @UbicacionId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

COMMIT TRANSACTION;
GO
