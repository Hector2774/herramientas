-- ============================================================
-- 016 · Roles en tabla propia + edición de usuarios
--   · Rol pasa de texto en Usuario (VARCHAR + CHECK) a la tabla Rol, referenciada por Usuario.RolId.
--   · El permiso de administrar usuarios es un dato del rol (Rol.AdministraUsuarios), no su nombre:
--     renombrar un rol no cambia lo que puede hacer.
--   · sp_Usuario_Actualizar edita usuario, empleado, rol y estado (la contraseña no).
--     El empleado solo se puede cambiar si el usuario aún no registró mantenimientos: esos
--     registros apuntan al usuario y pasarían a mostrarse a nombre de otra persona.
--
-- Ejecutar con:  sqlcmd -S localhost -E -C -I -f 65001 -b -i 016_roles_normalizados.sql
-- ============================================================
USE PROMACO_Herramientas;
GO

-- ── Tabla Rol ────────────────────────────────────────────────
IF OBJECT_ID('dbo.Rol') IS NULL
BEGIN
    CREATE TABLE Rol (
        RolId              INT IDENTITY(1,1) CONSTRAINT PK_Rol PRIMARY KEY,
        Nombre             VARCHAR(30)  NOT NULL CONSTRAINT UQ_Rol_Nombre UNIQUE,
        Descripcion        VARCHAR(200) NULL,
        AdministraUsuarios BIT          NOT NULL CONSTRAINT DF_Rol_AdministraUsuarios DEFAULT 0
    );

    INSERT INTO Rol (Nombre, Descripcion, AdministraUsuarios) VALUES
        ('Administrador', 'Préstamos, devoluciones e inventario; además administra los usuarios.', 1),
        ('Operador',      'Préstamos, devoluciones e inventario.', 0);
END
GO

-- ── Usuario.Rol (texto) → Usuario.RolId (FK) ─────────────────
IF COL_LENGTH('dbo.Usuario', 'RolId') IS NULL
    ALTER TABLE Usuario ADD RolId INT NULL CONSTRAINT FK_Usuario_Rol REFERENCES Rol(RolId);
GO

IF COL_LENGTH('dbo.Usuario', 'Rol') IS NOT NULL
BEGIN
    EXEC ('UPDATE u SET RolId = r.RolId FROM Usuario u JOIN Rol r ON r.Nombre = u.Rol WHERE u.RolId IS NULL;');

    IF EXISTS (SELECT 1 FROM Usuario WHERE RolId IS NULL)
        THROW 50000, 'Hay usuarios con un rol que no existe en la tabla Rol.', 1;

    ALTER TABLE Usuario DROP CONSTRAINT CK_Usuario_Rol;
    ALTER TABLE Usuario DROP CONSTRAINT DF_Usuario_Rol;
    ALTER TABLE Usuario DROP COLUMN Rol;
END
GO

ALTER TABLE Usuario ALTER COLUMN RolId INT NOT NULL;
GO

-- ── Roles ────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Rol_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT RolId, Nombre, Descripcion, AdministraUsuarios
    FROM   Rol
    ORDER  BY AdministraUsuarios, Nombre;
END
GO

-- ── Login ────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Usuario_ObtenerParaLogin
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.UsuarioId,
           u.Username,
           u.PasswordHash,
           CASE WHEN u.PasswordHash IS NULL THEN u.Password END AS PasswordLegacy,
           u.RolId,
           r.Nombre AS Rol,
           r.AdministraUsuarios,
           u.DebeCambiarPassword,
           e.EmpleadoId,
           e.Nombre AS NombreEmpleado
    FROM   Usuario  u
    INNER  JOIN Rol      r ON r.RolId      = u.RolId
    INNER  JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
    WHERE  u.Username = @Username
      AND  u.Activo   = 1
      AND  e.Activo   = 1;
END
GO

-- ── Administración de usuarios ───────────────────────────────
CREATE OR ALTER PROCEDURE sp_Usuario_ValidarAdministrador
    @AdminId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1
                   FROM   Usuario u
                   INNER  JOIN Rol r ON r.RolId = u.RolId
                   WHERE  u.UsuarioId = @AdminId AND u.Activo = 1 AND r.AdministraUsuarios = 1)
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
           u.RolId,
           r.Nombre  AS Rol,
           r.AdministraUsuarios,
           CAST(ISNULL(u.Activo, 0) AS BIT)                                AS Activo,
           u.DebeCambiarPassword,
           CAST(CASE WHEN u.PasswordHash IS NULL THEN 0 ELSE 1 END AS BIT) AS PasswordMigrada,
           e.EmpleadoId,
           e.Codigo  AS CodigoEmpleado,
           e.Nombre  AS Empleado,
           e.Activo  AS EmpleadoActivo,
           CAST(CASE WHEN EXISTS (SELECT 1 FROM Mantenimiento m
                                  WHERE  m.RegistradoPorId = u.UsuarioId OR m.CerradoPorId = u.UsuarioId)
                     THEN 1 ELSE 0 END AS BIT)                             AS TieneMovimientos
    FROM   Usuario  u
    INNER  JOIN Rol      r ON r.RolId      = u.RolId
    INNER  JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
    ORDER  BY ISNULL(u.Activo, 0) DESC, u.Username;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuario_Insertar
    @AdminId      INT,
    @Username     NVARCHAR(50),
    @PasswordHash VARCHAR(256),
    @EmpleadoId   INT,
    @RolId        INT
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
    IF NOT EXISTS (SELECT 1 FROM Rol WHERE RolId = @RolId)
        THROW 50000, 'El rol especificado no existe.', 1;

    INSERT INTO Usuario (Username, Password, PasswordHash, Activo, EmpleadoId, RolId, DebeCambiarPassword)
    VALUES (@Username, NULL, @PasswordHash, 1, @EmpleadoId, @RolId, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS UsuarioId;
END
GO

-- Edita todo menos la contraseña. Devuelve la fila actualizada (la app refresca la sesión
-- si el administrador se editó a sí mismo).
DROP PROCEDURE IF EXISTS sp_Usuario_Actualizar;
GO
CREATE PROCEDURE sp_Usuario_Actualizar
    @AdminId    INT,
    @UsuarioId  INT,
    @Username   NVARCHAR(50),
    @EmpleadoId INT,
    @RolId      INT,
    @Activo     BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    EXEC sp_Usuario_ValidarAdministrador @AdminId;

    DECLARE @EmpleadoActual INT = (SELECT EmpleadoId FROM Usuario WHERE UsuarioId = @UsuarioId);
    IF @EmpleadoActual IS NULL
        THROW 50000, 'El usuario especificado no existe.', 1;

    IF EXISTS (SELECT 1 FROM Usuario WHERE Username = @Username AND UsuarioId <> @UsuarioId)
        THROW 50000, 'Ya existe otro usuario con ese nombre.', 1;
    IF NOT EXISTS (SELECT 1 FROM Rol WHERE RolId = @RolId)
        THROW 50000, 'El rol especificado no existe.', 1;

    IF @UsuarioId = @AdminId
       AND (@Activo = 0 OR NOT EXISTS (SELECT 1 FROM Rol WHERE RolId = @RolId AND AdministraUsuarios = 1))
        THROW 50000, 'No puede quitarse su propio permiso de administrar usuarios ni desactivar su propio usuario.', 1;

    IF @EmpleadoId <> @EmpleadoActual
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Empleado WHERE EmpleadoId = @EmpleadoId AND Activo = 1)
            THROW 50000, 'El empleado no existe o está inactivo.', 1;
        IF EXISTS (SELECT 1 FROM Usuario WHERE EmpleadoId = @EmpleadoId)
            THROW 50000, 'Ese empleado ya tiene su propio usuario.', 1;
        IF EXISTS (SELECT 1 FROM Mantenimiento WHERE RegistradoPorId = @UsuarioId OR CerradoPorId = @UsuarioId)
            THROW 50000, 'No se puede cambiar el empleado: este usuario ya registró mantenimientos y quedarían a nombre de otra persona. Cree un usuario nuevo para el otro empleado.', 1;
    END

    BEGIN TRANSACTION;

    UPDATE Usuario
    SET    Username   = @Username,
           EmpleadoId = @EmpleadoId,
           RolId      = @RolId,
           Activo     = @Activo
    WHERE  UsuarioId  = @UsuarioId;

    IF NOT EXISTS (SELECT 1 FROM Usuario u INNER JOIN Rol r ON r.RolId = u.RolId
                   WHERE u.Activo = 1 AND r.AdministraUsuarios = 1)
        THROW 50000, 'Debe quedar al menos un usuario activo que pueda administrar usuarios.', 1;

    COMMIT TRANSACTION;

    SELECT u.UsuarioId, u.Username, u.RolId, r.Nombre AS Rol, r.AdministraUsuarios,
           e.EmpleadoId, e.Nombre AS NombreEmpleado
    FROM   Usuario  u
    INNER  JOIN Rol      r ON r.RolId      = u.RolId
    INNER  JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
    WHERE  u.UsuarioId = @UsuarioId;
END
GO
