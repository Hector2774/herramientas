-- ============================================================
-- 015 · Seguridad de usuarios
--   · Contraseñas con hash PBKDF2-SHA256 (lo calcula la app; SQL Server no sabe calcularlo).
--     La contraseña nunca se compara en SQL: el SP de login solo devuelve el hash guardado.
--   · Migración transparente: los usuarios con contraseña en texto plano (Password) se
--     migran solos en su siguiente inicio de sesión (la app guarda el hash y borra Password).
--   · Roles: Administrador (maneja usuarios) y Operador (préstamos, devoluciones, inventario).
--   · DebeCambiarPassword: contraseñas temporales (usuario nuevo o restablecido).
--
-- Ejecutar con:  sqlcmd -S localhost -E -C -I -f 65001 -b -i 015_usuarios_seguridad.sql
--
-- Más adelante, cuando todos hayan iniciado sesión al menos una vez
-- (SELECT COUNT(*) FROM Usuario WHERE PasswordHash IS NULL  →  0):
--     ALTER TABLE Usuario DROP COLUMN Password;
-- ============================================================
USE PROMACO_Herramientas;
GO

IF COL_LENGTH('dbo.Usuario', 'PasswordHash') IS NULL
    ALTER TABLE Usuario ADD PasswordHash VARCHAR(256) NULL;
GO

IF COL_LENGTH('dbo.Usuario', 'Rol') IS NULL
BEGIN
    ALTER TABLE Usuario ADD Rol VARCHAR(20) NOT NULL
        CONSTRAINT DF_Usuario_Rol DEFAULT 'Operador'
        CONSTRAINT CK_Usuario_Rol CHECK (Rol IN ('Administrador', 'Operador'));

    -- Los usuarios que ya existían tenían acceso total: lo conservan
    EXEC ('UPDATE Usuario SET Rol = ''Administrador'';');
END
GO

IF COL_LENGTH('dbo.Usuario', 'DebeCambiarPassword') IS NULL
    ALTER TABLE Usuario ADD DebeCambiarPassword BIT NOT NULL
        CONSTRAINT DF_Usuario_DebeCambiarPassword DEFAULT 0;
GO

-- El login anterior comparaba la contraseña en texto plano (y sin distinguir mayúsculas)
DROP PROCEDURE IF EXISTS sp_Usuario_Login;
GO

-- ── Login ────────────────────────────────────────────────────
-- Devuelve las credenciales guardadas; la app verifica la contraseña.
-- Password (texto plano) solo viene mientras el usuario no se haya migrado.
CREATE OR ALTER PROCEDURE sp_Usuario_ObtenerParaLogin
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.UsuarioId,
           u.Username,
           u.PasswordHash,
           CASE WHEN u.PasswordHash IS NULL THEN u.Password END AS PasswordLegacy,
           u.Rol,
           u.DebeCambiarPassword,
           e.EmpleadoId,
           e.Nombre AS NombreEmpleado
    FROM   Usuario  u
    INNER  JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
    WHERE  u.Username = @Username
      AND  u.Activo   = 1
      AND  e.Activo   = 1;
END
GO

-- Guarda un hash nuevo y elimina la contraseña en texto plano.
-- Lo usan: la migración al iniciar sesión, "Cambiar mi contraseña" y el restablecimiento.
CREATE OR ALTER PROCEDURE sp_Usuario_GuardarPassword
    @UsuarioId           INT,
    @PasswordHash        VARCHAR(256),
    @DebeCambiarPassword BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuario
    SET    PasswordHash        = @PasswordHash,
           Password            = NULL,
           DebeCambiarPassword = @DebeCambiarPassword
    WHERE  UsuarioId = @UsuarioId;

    IF @@ROWCOUNT = 0
        THROW 50000, 'El usuario especificado no existe.', 1;
END
GO

-- ── Administración de usuarios ───────────────────────────────
-- @AdminId: usuario en sesión; debe ser Administrador activo.

CREATE OR ALTER PROCEDURE sp_Usuario_ValidarAdministrador
    @AdminId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE UsuarioId = @AdminId AND Rol = 'Administrador' AND Activo = 1)
        THROW 50000, 'Solo un administrador puede administrar usuarios.', 1;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuario_ObtenerTodos
    @AdminId INT
AS
BEGIN
    SET NOCOUNT ON;
    EXEC sp_Usuario_ValidarAdministrador @AdminId;

    SELECT u.UsuarioId,
           u.Username,
           u.Rol,
           CAST(ISNULL(u.Activo, 0) AS BIT)                         AS Activo,
           u.DebeCambiarPassword,
           CAST(CASE WHEN u.PasswordHash IS NULL THEN 0 ELSE 1 END AS BIT) AS PasswordMigrada,
           e.EmpleadoId,
           e.Codigo  AS CodigoEmpleado,
           e.Nombre  AS Empleado,
           e.Activo  AS EmpleadoActivo
    FROM   Usuario  u
    INNER  JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
    ORDER  BY ISNULL(u.Activo, 0) DESC, u.Username;
END
GO

-- Usuario nuevo con contraseña temporal: deberá cambiarla en su primer inicio de sesión
CREATE OR ALTER PROCEDURE sp_Usuario_Insertar
    @AdminId      INT,
    @Username     NVARCHAR(50),
    @PasswordHash VARCHAR(256),
    @EmpleadoId   INT,
    @Rol          VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    EXEC sp_Usuario_ValidarAdministrador @AdminId;

    IF EXISTS (SELECT 1 FROM Usuario WHERE Username = @Username)
        THROW 50000, 'Ya existe un usuario con ese nombre.', 1;
    IF NOT EXISTS (SELECT 1 FROM Empleado WHERE EmpleadoId = @EmpleadoId AND Activo = 1)
        THROW 50000, 'El empleado no existe o está inactivo.', 1;
    IF EXISTS (SELECT 1 FROM Usuario WHERE EmpleadoId = @EmpleadoId)
        THROW 50000, 'Ese empleado ya tiene un usuario. Si está desactivado, actívelo en lugar de crear otro.', 1;

    INSERT INTO Usuario (Username, Password, PasswordHash, Activo, EmpleadoId, Rol, DebeCambiarPassword)
    VALUES (@Username, NULL, @PasswordHash, 1, @EmpleadoId, @Rol, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS UsuarioId;
END
GO

-- Cambia rol y/o estado. Nunca deja el sistema sin un administrador activo.
-- No se puede borrar un usuario: préstamos y mantenimientos guardan quién los registró.
CREATE OR ALTER PROCEDURE sp_Usuario_Actualizar
    @AdminId   INT,
    @UsuarioId INT,
    @Rol       VARCHAR(20),
    @Activo    BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    EXEC sp_Usuario_ValidarAdministrador @AdminId;

    IF @UsuarioId = @AdminId
       AND EXISTS (SELECT 1 FROM Usuario WHERE UsuarioId = @UsuarioId AND (Rol <> @Rol OR Activo <> @Activo))
        THROW 50000, 'No puede quitarse su propio rol de administrador ni desactivar su propio usuario.', 1;

    BEGIN TRANSACTION;

    UPDATE Usuario SET Rol = @Rol, Activo = @Activo WHERE UsuarioId = @UsuarioId;

    IF @@ROWCOUNT = 0
        THROW 50000, 'El usuario especificado no existe.', 1;

    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Rol = 'Administrador' AND Activo = 1)
        THROW 50000, 'Debe quedar al menos un administrador activo.', 1;

    COMMIT TRANSACTION;
END
GO

-- Restablece con una contraseña temporal (olvido de contraseña)
CREATE OR ALTER PROCEDURE sp_Usuario_RestablecerPassword
    @AdminId      INT,
    @UsuarioId    INT,
    @PasswordHash VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    EXEC sp_Usuario_ValidarAdministrador @AdminId;
    EXEC sp_Usuario_GuardarPassword @UsuarioId, @PasswordHash, 1;
END
GO
