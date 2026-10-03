-- ============================================================
-- 009 · Historial de sincronizaciones de empleados (API de RRHH)
--
-- FrmEmpleados muestra "Última sincronización: DD/MM/YYYY HH:MM".
-- Antes la fecha solo vivía en memoria y se perdía al cerrar.
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 009_sincronizacion_empleados.sql
-- ============================================================
SET XACT_ABORT ON;
GO

IF OBJECT_ID('SincronizacionEmpleados') IS NULL
    CREATE TABLE SincronizacionEmpleados (
        SincronizacionId INT IDENTITY(1,1) CONSTRAINT PK_SincronizacionEmpleados PRIMARY KEY,
        Fecha            DATETIME2 NOT NULL CONSTRAINT DF_SincronizacionEmpleados_Fecha DEFAULT SYSDATETIME(),
        Nuevos           INT       NOT NULL,
        Actualizados     INT       NOT NULL,
        TotalActivos     INT       NOT NULL
    );
GO
