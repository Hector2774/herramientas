-- ============================================================
-- 018 · Mantenimiento.Costo con centavos
--   La columna era DECIMAL(18,0): el costo total (materiales + mano de obra, ambas
--   DECIMAL(10,2)) se redondeaba al quetzal. Q 125.50 se guardaba como Q 126.
--   Pasa a DECIMAL(12,2): admite la suma de los dos componentes sin redondear.
--   Los registros existentes no cambian (ya estaban redondeados).
--
-- Ejecutar con:  sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i 018_costo_con_centavos.sql
-- ============================================================
SET XACT_ABORT ON;
GO

IF EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.types t ON t.user_type_id = c.user_type_id
           WHERE c.object_id = OBJECT_ID('dbo.Mantenimiento') AND c.name = 'Costo' AND c.scale = 0)
BEGIN
    BEGIN TRANSACTION;

    -- La restricción CHECK sobre la columna impide cambiar su tipo: se quita y se vuelve a crear
    ALTER TABLE Mantenimiento DROP CONSTRAINT CK_Mantenimiento_Costo;
    ALTER TABLE Mantenimiento ALTER COLUMN Costo DECIMAL(12,2) NULL;
    ALTER TABLE Mantenimiento WITH CHECK ADD CONSTRAINT CK_Mantenimiento_Costo CHECK (Costo IS NULL OR Costo >= 0);

    COMMIT TRANSACTION;
END
GO
