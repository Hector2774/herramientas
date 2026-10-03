-- ============================================================
-- PROMACO · Sistema de Control de Herramientas
-- 001 · Datos iniciales (ejecutar después de 000_esquema.sql)
--
-- Lo mínimo para que el sistema arranque:
--   · Roles (Administrador / Operador)
--   · Catálogos de herramientas: categorías, marcas y ubicaciones
--   · Departamento, empleado y usuario administrador
--
-- Usuario inicial:   admin
-- Contraseña inicial: Promaco2026   (temporal: el sistema obliga a cambiarla
--                                    en el primer inicio de sesión)
--
-- Los empleados reales se cargan desde RRHH con "Sincronizar empleados"
-- en la pantalla Empleados. Se puede ejecutar más de una vez: solo inserta
-- lo que falta.
--
--   sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i 001_datos_iniciales.sql
-- ============================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

IF DB_NAME() IN (N'master', N'model', N'msdb', N'tempdb')
BEGIN
    RAISERROR(N'Seleccione la base PROMACO_Herramientas antes de ejecutar este script.', 16, 1);
    SET NOEXEC ON;
END
GO

BEGIN TRANSACTION;

-- ── Roles ────────────────────────────────────────────────────
-- El permiso está en AdministraUsuarios, no en el nombre del rol
IF NOT EXISTS (SELECT 1 FROM Rol)
BEGIN
    SET IDENTITY_INSERT Rol ON;
    INSERT INTO Rol (RolId, Nombre, Descripcion, AdministraUsuarios) VALUES
        (1, 'Administrador', 'Préstamos, devoluciones e inventario; además administra los usuarios.', 1),
        (2, 'Operador',      'Préstamos, devoluciones e inventario.', 0);
    SET IDENTITY_INSERT Rol OFF;
END

-- ── Catálogos de herramientas ────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM CategoriaHerramienta)
BEGIN
    SET IDENTITY_INSERT CategoriaHerramienta ON;
    INSERT INTO CategoriaHerramienta (CategoriaId, Nombre, Descripcion) VALUES
        (1, 'Eléctricas',   'Herramientas eléctricas de cable'),
        (2, 'Inalámbricas', 'Herramientas a batería'),
        (3, 'Manuales',     'Herramientas de mano'),
        (4, 'Medición',     'Instrumentos de medición y nivelación'),
        (5, 'Neumáticas',   'Herramientas de aire comprimido'),
        (6, 'Jardinería',   'Herramientas para exteriores y jardín'),
        (7, 'Seguridad',    'Equipo de protección y seguridad');
    SET IDENTITY_INSERT CategoriaHerramienta OFF;
END

IF NOT EXISTS (SELECT 1 FROM Marca)
BEGIN
    SET IDENTITY_INSERT Marca ON;
    INSERT INTO Marca (MarcaId, NombreMarca, Descripcion) VALUES
        (0, 'DeWalt',    'Herramientas eléctricas profesionales'),
        (1, 'Makita',    'Herramientas eléctricas e inalámbricas'),
        (2, 'Bosch',     'Herramientas eléctricas y de medición'),
        (3, 'Truper',    'Herramientas manuales y de jardín'),
        (4, 'Stanley',   'Herramientas manuales y de medición'),
        (5, 'Milwaukee', 'Herramientas profesionales a batería'),
        (6, 'Pretul',    'Línea económica de herramientas'),
        (7, '3M',        'Equipo de seguridad');
    SET IDENTITY_INSERT Marca OFF;
END

IF NOT EXISTS (SELECT 1 FROM Ubicacion)
BEGIN
    SET IDENTITY_INSERT Ubicacion ON;
    INSERT INTO Ubicacion (UbicacionId, Nombre, Descripcion) VALUES
        (1, 'Almacén A - Estante 1', 'Herramientas eléctricas'),
        (2, 'Almacén A - Estante 2', 'Herramientas manuales'),
        (3, 'Almacén B - Gabinete',  'Instrumentos de medición'),
        (4, 'Bodega exterior',       'Jardinería y neumáticas'),
        (5, 'Caseta de seguridad',   'Equipo de protección');
    SET IDENTITY_INSERT Ubicacion OFF;
END

-- ── Administrador del sistema ────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Departamento WHERE Nombre = N'ADMINISTRACIÓN')
    INSERT INTO Departamento (Nombre, Activo) VALUES (N'ADMINISTRACIÓN', 1);

IF NOT EXISTS (SELECT 1 FROM Empleado WHERE Codigo = N'ADMIN')
    INSERT INTO Empleado (Codigo, Nombre, Activo, DepartamentoId)
    VALUES (N'ADMIN', N'ADMINISTRADOR DEL SISTEMA', 1,
            (SELECT DepartamentoId FROM Departamento WHERE Nombre = N'ADMINISTRACIÓN'));

-- Hash PBKDF2-SHA256 de la contraseña temporal "Promaco2026" (generado con PasswordHasher de la app)
IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Username = N'admin')
    INSERT INTO Usuario (Username, Password, PasswordHash, Activo, EmpleadoId, RolId, DebeCambiarPassword)
    VALUES (N'admin', NULL,
            'pbkdf2-sha256$600000$lcvnlBKBfx/0CB/asZdWTA==$gdsO54Gq+UmXUIgjDzvcIz2cXglFEXoQTGmhRF8xlLU=',
            1,
            (SELECT EmpleadoId FROM Empleado WHERE Codigo = N'ADMIN'),
            (SELECT RolId FROM Rol WHERE Nombre = 'Administrador'),
            1);

COMMIT TRANSACTION;
GO

SET NOEXEC OFF;
GO
