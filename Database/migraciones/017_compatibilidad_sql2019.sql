-- ============================================================
-- 017 · Compatibilidad con SQL Server 2019
--   sp_Herramienta_AgregarUnidades usaba GENERATE_SERIES, que solo existe desde
--   SQL Server 2022. Se reemplaza por una tabla de números 1–1000 armada con VALUES,
--   suficiente para el límite de 999 unidades que ya valida el procedimiento.
--
-- Ejecutar con:  sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i 017_compatibilidad_sql2019.sql
-- ============================================================
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

        -- Números 1..1000: centenas x decenas x unidades
        WITH Digitos AS (SELECT d FROM (VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) AS t(d)),
             Numeros AS (SELECT c.d * 100 + de.d * 10 + u.d + 1 AS n
                         FROM Digitos c CROSS JOIN Digitos de CROSS JOIN Digitos u)
        INSERT INTO HerramientaUnidad (HerramientaId, Numero)
        SELECT @HerramientaId, @Ultimo + n
        FROM   Numeros
        WHERE  n <= @Cantidad;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
