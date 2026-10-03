-- ============================================================
-- 005 · Usuario ligado a Empleado / "Aprobado por" automático
--
-- Antes: al registrar un préstamo se elegía a mano quién lo aprobaba.
-- Ahora: cada Usuario del sistema está ligado a un Empleado
-- (Usuario.EmpleadoId) y el préstamo lo aprueba el empleado del
-- usuario que inició sesión.
--   sp_Usuario_Login       valida credenciales y devuelve el empleado
--   sp_Prestamo_Registrar  recibe @UsuarioId en lugar de @AprobadoPorId
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 005_usuario_empleado.sql
-- ============================================================
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
GO

-- ── Empleado que administra el sistema ───────────────────────
IF NOT EXISTS (SELECT 1 FROM Empleado WHERE Codigo = N'ADMIN')
    INSERT INTO Empleado (Codigo, Nombre, Activo, DepartamentoId, Cargo)
    VALUES (N'ADMIN', N'ADMINISTRADOR DEL SISTEMA', 1,
            (SELECT TOP 1 DepartamentoId FROM Departamento WHERE Nombre LIKE 'ADMINISTRACI%' ORDER BY DepartamentoId),
            'Administrador del sistema');
GO

-- ── Usuario.EmpleadoId ───────────────────────────────────────
ALTER TABLE Usuario ADD EmpleadoId INT NULL
    CONSTRAINT FK_Usuario_Empleado REFERENCES Empleado(EmpleadoId);
GO

UPDATE Usuario
SET    EmpleadoId = (SELECT EmpleadoId FROM Empleado WHERE Codigo = N'ADMIN')
WHERE  Username = N'admin';

IF EXISTS (SELECT 1 FROM Usuario WHERE EmpleadoId IS NULL)
    THROW 50000, 'Hay usuarios sin empleado asignado; ligarlos antes de continuar.', 1;

ALTER TABLE Usuario ALTER COLUMN EmpleadoId INT NOT NULL;

-- Un empleado solo puede tener un usuario
ALTER TABLE Usuario ADD CONSTRAINT UQ_Usuario_Empleado UNIQUE (EmpleadoId);
GO

-- ── Login ────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE sp_Usuario_Login
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.UsuarioId,
           u.Username,
           e.EmpleadoId,
           e.Nombre AS NombreEmpleado
    FROM   Usuario  u
    INNER  JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
    WHERE  u.Username = @Username
      AND  u.Password = @Password
      AND  u.Activo   = 1
      AND  e.Activo   = 1;
END
GO

-- ── Préstamos: el aprobador sale del usuario en sesión ───────
--   @Herramientas: '<herramientas><item id="UnidadId"/>...</herramientas>'
CREATE OR ALTER PROCEDURE sp_Prestamo_Registrar
    @EmpleadoId              INT,
    @UsuarioId               INT,
    @FechaDevolucionEsperada DATETIME,
    @Observaciones           VARCHAR(400) = NULL,
    @Herramientas            XML
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM Empleado WHERE EmpleadoId = @EmpleadoId AND Activo = 1)
        THROW 50000, 'El empleado no existe o no está activo.', 1;

    DECLARE @AprobadoPorId INT = (
        SELECT e.EmpleadoId
        FROM   Usuario  u
        INNER  JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
        WHERE  u.UsuarioId = @UsuarioId AND u.Activo = 1 AND e.Activo = 1);

    IF @AprobadoPorId IS NULL
        THROW 50000, 'El usuario en sesión no existe, no está activo o su empleado no está activo.', 1;

    IF @AprobadoPorId = @EmpleadoId
        THROW 50000, 'El empleado no puede aprobar su propio préstamo.', 1;

    IF @FechaDevolucionEsperada <= GETDATE()
        THROW 50000, 'La fecha de devolución esperada debe ser posterior a hoy.', 1;

    DECLARE @Items TABLE (UnidadId INT PRIMARY KEY);

    INSERT INTO @Items (UnidadId)
    SELECT DISTINCT x.item.value('@id', 'INT')
    FROM   @Herramientas.nodes('/herramientas/item') AS x(item);

    IF NOT EXISTS (SELECT 1 FROM @Items)
        THROW 50000, 'Debe seleccionar al menos una herramienta.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Todas las unidades deben existir, estar Disponibles y su grupo habilitado para préstamo
        IF (SELECT COUNT(*)
            FROM   @Items i
            INNER  JOIN HerramientaUnidad u WITH (UPDLOCK, HOLDLOCK) ON u.UnidadId = i.UnidadId
            INNER  JOIN Herramienta       h ON h.HerramientaId = u.HerramientaId
            WHERE  u.Estado             = 'Disponible'
              AND  h.Activa             = 1
              AND  h.PrestamoHabilitado = 1) <> (SELECT COUNT(*) FROM @Items)
            THROW 50000, 'Una o más herramientas ya no están disponibles para préstamo.', 1;

        INSERT INTO Prestamo (
            EmpleadoId, AprobadoPorId,
            FechaPrestamo, FechaDevolucionEsperada,
            Estado, Observaciones
        )
        VALUES (
            @EmpleadoId, @AprobadoPorId,
            GETDATE(), @FechaDevolucionEsperada,
            'Activo', @Observaciones
        );

        DECLARE @PrestamoId INT = SCOPE_IDENTITY();

        INSERT INTO PrestamoDetalle (PrestamoId, HerramientaId, UnidadId, EstadoDevolucion)
        SELECT @PrestamoId, u.HerramientaId, u.UnidadId, 'Pendiente'
        FROM   @Items i
        INNER  JOIN HerramientaUnidad u ON u.UnidadId = i.UnidadId;

        UPDATE HerramientaUnidad
        SET    Estado = 'Prestada'
        WHERE  UnidadId IN (SELECT UnidadId FROM @Items);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT @PrestamoId AS PrestamoId;
END
GO

COMMIT TRANSACTION;
GO
