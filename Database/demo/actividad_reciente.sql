-- ============================================================
-- PROMACO · Sistema de Control de Herramientas
-- Datos de demostración: poner al día la operación
--
-- datos_demo.sql simula la operación hasta el momento en que se carga; con los días la base
-- se queda "congelada" (sin préstamos recientes, mantenimientos abiertos desde hace semanas).
-- Este script continúa la operación desde el último movimiento registrado (como máximo
-- 30 días atrás) hasta este momento, con las mismas reglas que la aplicación:
--   · préstamos en días hábiles (pocos los sábados, ninguno el domingo); la mayoría se
--     devuelve a tiempo, algunos con atraso y pocos dañados; lo que todavía no regresa
--     queda activo o vencido
--   · los mantenimientos abiertos hace más de una semana se cierran con su costo en lempiras
--   · hoy: préstamos nuevos, al menos una devolución y un mantenimiento preventivo, para que
--     los indicadores del dashboard y de reportes muestren movimiento del día
--   · al menos 2 préstamos vencidos, para mostrar el reporte de vencidos y "Notificar"
--
-- Se ejecuta la mañana de una presentación (de preferencia después de las 7:00).
-- Si ya se ejecutó hoy, no hace nada. Solo funciona en una base con los datos de
-- demostración cargados, así que nunca altera una base de producción.
--
--   sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i actividad_reciente.sql
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

IF NOT EXISTS (SELECT 1 FROM sys.extended_properties WHERE class = 0 AND name = N'PROMACO_DatosDemo')
BEGIN
    RAISERROR(N'Esta base no tiene los datos de demostración: ejecute primero Database/demo/datos_demo.sql.', 16, 1);
    SET NOEXEC ON;
END
GO

IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE class = 0 AND name = N'PROMACO_DemoAlDia'
           AND CAST(value AS NVARCHAR(10)) = CONVERT(NCHAR(10), CAST(SYSDATETIME() AS DATE), 23))
BEGIN
    PRINT N'La actividad de demostración ya se puso al día hoy; no se agregó nada.';
    SET NOEXEC ON;
END
GO

BEGIN TRANSACTION;

DECLARE @Ahora DATETIME2(0) = SYSDATETIME();
DECLARE @Hoy   DATE         = CAST(@Ahora AS DATE);
DECLARE @Nunca DATETIME2(0) = '9999-12-31';

-- Desde el día siguiente al último movimiento (préstamo, devolución o mantenimiento),
-- sin ir más de 30 días atrás. Hoy siempre se simula.
DECLARE @Ultimo DATE = (SELECT CAST(MAX(v) AS DATE) FROM (
    SELECT MAX(FechaPrestamo) FROM Prestamo
    UNION ALL SELECT MAX(FechaDevuelta) FROM PrestamoDetalle
    UNION ALL SELECT MAX(CAST(FechaInicio AS DATETIME2(7))) FROM Mantenimiento
    UNION ALL SELECT MAX(CAST(FechaFin AS DATETIME2(7))) FROM Mantenimiento) x(v));
DECLARE @Desde DATE = DATEADD(DAY, 1, ISNULL(@Ultimo, @Hoy));
IF @Desde < DATEADD(DAY, -30, @Hoy) SET @Desde = DATEADD(DAY, -30, @Hoy);
IF @Desde > @Hoy SET @Desde = @Hoy;

-- Ventana de trabajo de hoy: de 07:00 hasta hace 10 minutos (si es muy temprano, la última hora)
DECLARE @FinHoy DATETIME2(0) = DATEADD(MINUTE, -10, @Ahora);
IF @FinHoy > DATEADD(HOUR, 16, CAST(@Hoy AS DATETIME2(0))) SET @FinHoy = DATEADD(HOUR, 16, CAST(@Hoy AS DATETIME2(0)));
DECLARE @IniHoy DATETIME2(0) = DATEADD(HOUR, 7, CAST(@Hoy AS DATETIME2(0)));
IF @FinHoy <= DATEADD(MINUTE, 30, @IniHoy) SET @IniHoy = DATEADD(MINUTE, -60, @FinHoy);

-- ════════════════════════════════════════════════════════════
-- 1. Participantes
-- ════════════════════════════════════════════════════════════

-- Quién aprueba: los empleados de los usuarios activos
DECLARE @Aprob TABLE (Id INT IDENTITY PRIMARY KEY, EmpleadoId INT, UsuarioId INT);
INSERT INTO @Aprob (EmpleadoId, UsuarioId)
SELECT e.EmpleadoId, u.UsuarioId
FROM   Usuario u INNER JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
WHERE  u.Activo = 1 AND e.Activo = 1
ORDER  BY u.UsuarioId;
DECLARE @nAprob INT = (SELECT COUNT(*) FROM @Aprob);
IF @nAprob = 0 THROW 50000, 'No hay usuarios activos para aprobar préstamos.', 1;

-- Quién pide prestado: los mismos que ya pedían, con más peso los que más piden
CREATE TABLE #Prestatario (EmpleadoId INT PRIMARY KEY, Desde FLOAT, Hasta FLOAT);
WITH Frecuencia AS (
    SELECT p.EmpleadoId, CAST(COUNT(*) AS FLOAT) AS W
    FROM   Prestamo p INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
    WHERE  e.Activo = 1 AND p.EmpleadoId NOT IN (SELECT EmpleadoId FROM @Aprob)
    GROUP  BY p.EmpleadoId
), Acumulado AS (
    SELECT EmpleadoId, W, SUM(W) OVER (ORDER BY EmpleadoId ROWS UNBOUNDED PRECEDING) AS Hasta, SUM(W) OVER () AS Total
    FROM   Frecuencia
)
INSERT INTO #Prestatario SELECT EmpleadoId, (Hasta - W) / Total, Hasta / Total FROM Acumulado;
IF NOT EXISTS (SELECT 1 FROM #Prestatario)
    THROW 50000, 'No hay empleados con préstamos anteriores para continuar la simulación.', 1;

-- Técnicos internos: el taller de mantenimiento (o, si no hay, quienes más piden prestado)
DECLARE @Tecnico TABLE (Id INT IDENTITY PRIMARY KEY, EmpleadoId INT);
INSERT INTO @Tecnico (EmpleadoId)
SELECT e.EmpleadoId FROM Empleado e INNER JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
WHERE  e.Activo = 1 AND d.Nombre = N'TALLER DE MANTENIMIENTO' ORDER BY e.EmpleadoId;
IF NOT EXISTS (SELECT 1 FROM @Tecnico)
    INSERT INTO @Tecnico (EmpleadoId) SELECT TOP 2 EmpleadoId FROM #Prestatario ORDER BY Hasta - Desde DESC;
DECLARE @nTec INT = (SELECT COUNT(*) FROM @Tecnico);

-- Folio de la última factura de proveedor, para seguir la numeración
DECLARE @folio INT = ISNULL((SELECT MAX(TRY_CAST(SUBSTRING(FolioFactura, 5, 20) AS INT)) FROM Mantenimiento
                             WHERE FolioFactura LIKE 'FAC-%'), 10230);

-- ════════════════════════════════════════════════════════════
-- 2. Unidades que pueden prestarse y desde cuándo están libres
-- ════════════════════════════════════════════════════════════
CREATE TABLE #Unidad (
    UnidadId      INT PRIMARY KEY,
    HerramientaId INT,
    Motor         BIT,                 -- eléctrica, inalámbrica, neumática o a gasolina
    PierdeFacil   BIT,                 -- herramienta de mano o de medición (pequeña)
    LibreDesde    DATETIME2(0),
    EstadoFinal   VARCHAR(20),
    Cambio        BIT DEFAULT 0
);
INSERT INTO #Unidad (UnidadId, HerramientaId, Motor, PierdeFacil, LibreDesde, EstadoFinal)
SELECT u.UnidadId, u.HerramientaId,
       CASE WHEN h.CategoriaId IN (1, 2, 5) OR h.Nombre LIKE N'Podadora%' THEN 1 ELSE 0 END,
       CASE WHEN h.CategoriaId IN (3, 4) THEN 1 ELSE 0 END,
       CASE WHEN u.Estado = 'Disponible' THEN @Ahora ELSE @Nunca END,
       u.Estado
FROM   HerramientaUnidad u
INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
WHERE  h.Activa = 1 AND h.PrestamoHabilitado = 1 AND u.Estado IN ('Disponible', 'En Mantenimiento');

-- Una unidad disponible quedó libre con su última devolución o su último mantenimiento
UPDATE x
SET    LibreDesde = ISNULL((SELECT MAX(v) FROM (
                        SELECT MAX(d.FechaDevuelta) FROM PrestamoDetalle d WHERE d.UnidadId = x.UnidadId
                        UNION ALL
                        SELECT MAX(CAST(m.FechaFin AS DATETIME2(7))) FROM Mantenimiento m WHERE m.UnidadId = x.UnidadId) t(v)),
                        CAST(@Desde AS DATETIME2(0)))
FROM   #Unidad x
WHERE  x.EstadoFinal = 'Disponible';
UPDATE #Unidad SET LibreDesde = CAST(@Desde AS DATETIME2(0)) WHERE EstadoFinal = 'Disponible' AND LibreDesde < @Desde;

-- Demanda: las herramientas que más se han prestado se siguen prestando más
CREATE TABLE #Peso (HerramientaId INT PRIMARY KEY, Peso FLOAT);
INSERT INTO #Peso
SELECT h.HerramientaId, 1 + (SELECT COUNT(*) FROM PrestamoDetalle d WHERE d.HerramientaId = h.HerramientaId)
FROM   Herramienta h WHERE h.HerramientaId IN (SELECT HerramientaId FROM #Unidad);

-- ════════════════════════════════════════════════════════════
-- 3. Textos
-- ════════════════════════════════════════════════════════════
DECLARE @NotaDanio TABLE (Motor BIT, i INT, Texto NVARCHAR(250), PRIMARY KEY (Motor, i));
INSERT INTO @NotaDanio VALUES
    (1, 0, N'No enciende'), (1, 1, N'Carbones gastados, chispea al usarla'), (1, 2, N'Cable de alimentación pelado'),
    (1, 3, N'Batería no retiene la carga'), (1, 4, N'Mandril flojo'),
    (0, 0, N'Mango quebrado'), (0, 1, N'Mordaza desalineada'), (0, 2, N'Cinta doblada, no regresa'), (0, 3, N'Golpe fuerte, pieza deformada');

DECLARE @Uso TABLE (i INT PRIMARY KEY, Texto NVARCHAR(400));
INSERT INTO @Uso VALUES
    (0, N'Inventario físico en bodega'), (1, N'Armado de estanterías'), (2, N'Reparación de tarimas'),
    (3, N'Instalación de rótulos en tienda'), (4, N'Corte de varilla para pedido de cliente'),
    (5, N'Ordenamiento de bodega de madera'), (6, N'Reparaciones en área de carga y descarga'),
    (7, N'Mantenimiento de puertas y portones');

DECLARE @r FLOAT, @r2 FLOAT, @usr INT, @emp INT, @aprobEmp INT, @u INT, @h INT, @mid INT;
DECLARE @m0 DATETIME2(0), @m1 DATETIME2(0), @tipo VARCHAR(50), @servicio VARCHAR(10), @mat DECIMAL(10,2), @mano DECIMAL(10,2);

-- ════════════════════════════════════════════════════════════
-- 4. Cerrar los mantenimientos que llevan más de una semana abiertos
-- ════════════════════════════════════════════════════════════
DECLARE abiertos CURSOR LOCAL FAST_FORWARD FOR
    SELECT m.MantenimientoId, m.UnidadId, m.FechaInicio, m.TipoMantenimiento, m.TipoServicio
    FROM   Mantenimiento m
    WHERE  m.FechaFin IS NULL AND m.FechaInicio < DATEADD(DAY, -7, @Ahora);
OPEN abiertos;
FETCH NEXT FROM abiertos INTO @mid, @u, @m0, @tipo, @servicio;
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Se cierra entre 3 días después de abrirse (o el primer día simulado) y ayer, de 09:00 a 16:00
    SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
    DECLARE @lo DATE = CAST(DATEADD(DAY, 3, @m0) AS DATE);
    IF @lo < @Desde SET @lo = @Desde;
    IF @lo >= @Hoy SET @lo = DATEADD(DAY, -1, @Hoy);
    SET @m1 = DATEADD(MINUTE, 540 + FLOOR(@r * 420),
                      CAST(DATEADD(DAY, FLOOR(@r * (DATEDIFF(DAY, @lo, @Hoy))), @lo) AS DATETIME2(0)));
    IF DATEDIFF(DAY, '19000107', @m1) % 7 = 0 SET @m1 = DATEADD(DAY, -1, @m1);      -- domingo → sábado
    IF @m1 >= @Ahora SET @m1 = DATEADD(MINUTE, -30, @IniHoy);
    IF @m1 <= @m0 SET @m1 = DATEADD(HOUR, 4, @m0);

    SET @r2 = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
    SET @mat  = CASE WHEN @tipo = 'Preventivo' THEN ROUND((35 + @r2 * 85) / 5, 0) * 15
                     WHEN @servicio = 'Externo' THEN ROUND((80 + @r2 * 570) / 5, 0) * 15
                     ELSE ROUND((45 + @r2 * 405) / 5, 0) * 15 END;
    SET @mano = CASE WHEN @servicio = 'Externo' THEN ROUND((150 + @r * 450) / 5, 0) * 15 END;
    IF @servicio = 'Externo' SET @folio += 1 + FLOOR(@r2 * 40);
    SELECT @usr = UsuarioId FROM @Aprob WHERE Id = 1 + FLOOR(@r * @nAprob);

    UPDATE Mantenimiento
    SET    FechaFin        = @m1,
           NotasCierre     = CASE @tipo WHEN 'Preventivo'  THEN 'Sin novedades; lista para préstamo'
                                        WHEN 'Calibración' THEN 'Calibrado y certificado dentro de tolerancia'
                                        ELSE 'Se reparó y se probó en vacío; lista para préstamo' END,
           CostoMateriales = @mat,
           CostoManoObra   = @mano,
           Costo           = @mat + ISNULL(@mano, 0),
           FolioFactura    = CASE WHEN @servicio = 'Externo' THEN 'FAC-' + CAST(@folio AS VARCHAR(10)) END,
           Resultado       = 'Reparada',
           CerradoPorId    = @usr
    WHERE  MantenimientoId = @mid;

    UPDATE #Unidad SET LibreDesde = @m1, EstadoFinal = 'Disponible', Cambio = 1 WHERE UnidadId = @u;
    UPDATE HerramientaUnidad SET Estado = 'Disponible' WHERE UnidadId = @u AND Estado = 'En Mantenimiento';

    FETCH NEXT FROM abiertos INTO @mid, @u, @m0, @tipo, @servicio;
END
CLOSE abiertos; DEALLOCATE abiertos;

-- ════════════════════════════════════════════════════════════
-- 5. Préstamos día por día
-- ════════════════════════════════════════════════════════════
DECLARE @dia DATE = @Desde, @dow INT, @ini DATETIME2(0), @fin DATETIME2(0), @nPrest INT, @p INT, @nItems INT, @it INT;
DECLARE @fPrest DATETIME2(0), @dur INT, @fEsp DATETIME2(0), @patron INT, @fBase DATETIME2(0), @fDev DATETIME2(0);
DECLARE @pid INT, @cond VARCHAR(20), @nota NVARCHAR(250), @motor BIT, @pierde BIT;
DECLARE @Items TABLE (Orden INT IDENTITY PRIMARY KEY, UnidadId INT, HerramientaId INT);
DECLARE @Nuevos TABLE (PrestamoId INT PRIMARY KEY);

WHILE @dia <= @Hoy
BEGIN
    SET @dow = DATEDIFF(DAY, '19000107', @dia) % 7;      -- 0 = domingo, 6 = sábado

    IF @dow <> 0
    BEGIN
        SET @ini = DATEADD(HOUR, 7, CAST(@dia AS DATETIME2(0)));
        SET @fin = DATEADD(HOUR, 16, CAST(@dia AS DATETIME2(0)));
        IF @dia = @Hoy BEGIN SET @ini = @IniHoy; SET @fin = @FinHoy; END

        SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
        SET @nPrest = CASE WHEN @dow = 6 THEN CASE WHEN @r < 0.6 THEN 0 ELSE 1 END
                           WHEN @r < 0.10 THEN 0 WHEN @r < 0.40 THEN 1 WHEN @r < 0.75 THEN 2 WHEN @r < 0.93 THEN 3 ELSE 4 END;
        IF @dia = @Hoy AND @nPrest < 3 SET @nPrest = 3;  -- hoy siempre hay movimiento

        SET @p = 0;
        WHILE @p < @nPrest
        BEGIN
            SET @p += 1;

            -- Quién pide, quién aprueba y cuándo
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SELECT @emp = EmpleadoId FROM #Prestatario WHERE @r >= Desde AND @r < Hasta;
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SELECT @aprobEmp = EmpleadoId FROM @Aprob WHERE Id = 1 + FLOOR(@r * @nAprob);
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SET @fPrest = DATEADD(MINUTE, FLOOR(@r * DATEDIFF(MINUTE, @ini, @fin)), @ini);

            -- Plazo pactado (a las 17:00, nunca en domingo)
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SET @dur = CASE WHEN @r < 0.25 THEN 1 WHEN @r < 0.50 THEN 2 WHEN @r < 0.70 THEN 3
                            WHEN @r < 0.85 THEN 5 WHEN @r < 0.95 THEN 7 ELSE 14 END;
            SET @fEsp = DATEADD(HOUR, 17, CAST(DATEADD(DAY, @dur, @dia) AS DATETIME2(0)));
            IF DATEDIFF(DAY, '19000107', @fEsp) % 7 = 0 SET @fEsp = DATEADD(DAY, 1, @fEsp);

            -- Cuándo se devuelve: 78 % a tiempo, 18 % con atraso corto, 4 % con atraso largo
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SET @patron = CASE WHEN @r < 0.78 THEN 0 WHEN @r < 0.96 THEN 1 ELSE 2 END;
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SET @fBase = CAST(DATEADD(DAY, CASE @patron WHEN 0 THEN FLOOR(@r * (DATEDIFF(DAY, @dia, CAST(@fEsp AS DATE)) + 1))
                                                        WHEN 1 THEN DATEDIFF(DAY, @dia, CAST(@fEsp AS DATE)) + 1 + FLOOR(@r * 5)
                                                        ELSE        DATEDIFF(DAY, @dia, CAST(@fEsp AS DATE)) + 7 + FLOOR(@r * 9) END,
                                      @dia) AS DATETIME2(0));
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SET @fBase = DATEADD(MINUTE, 480 + FLOOR(@r * 540), @fBase);                          -- 08:00 a 17:00
            IF DATEDIFF(DAY, '19000107', @fBase) % 7 = 0 SET @fBase = DATEADD(DAY, 1, @fBase);
            IF @patron = 0 AND @fBase > @fEsp SET @fBase = DATEADD(MINUTE, -30, @fEsp);
            IF @fBase <= @fPrest SET @fBase = DATEADD(HOUR, 2, @fPrest);
            IF @dia = @Hoy SET @fBase = @Nunca;          -- lo prestado hoy sigue afuera

            -- Qué se lleva: 1 a 3 unidades libres en ese momento
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SET @nItems = CASE WHEN @r < 0.6 THEN 1 WHEN @r < 0.9 THEN 2 ELSE 3 END;
            DELETE FROM @Items;
            SET @it = 0;
            WHILE @it < @nItems
            BEGIN
                SET @it += 1;
                SET @u = NULL;
                SELECT TOP 1 @u = x.UnidadId, @h = x.HerramientaId
                FROM   #Unidad x INNER JOIN #Peso w ON w.HerramientaId = x.HerramientaId
                WHERE  x.LibreDesde <= @fPrest
                  AND  x.HerramientaId NOT IN (SELECT HerramientaId FROM @Items)
                ORDER  BY -LOG((ABS(CHECKSUM(NEWID()) % 1000000) + 1) / 1000001.0) / w.Peso;
                IF @u IS NOT NULL INSERT INTO @Items (UnidadId, HerramientaId) VALUES (@u, @h);
            END
            IF NOT EXISTS (SELECT 1 FROM @Items) CONTINUE;

            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            INSERT INTO Prestamo (EmpleadoId, AprobadoPorId, FechaPrestamo, FechaDevolucionEsperada, Estado, Observaciones)
            VALUES (@emp, @aprobEmp, @fPrest, @fEsp, 'Activo',
                    CASE WHEN @r < 0.3 THEN (SELECT Texto FROM @Uso WHERE i = FLOOR(@r / 0.3 * 8)) END);
            SET @pid = SCOPE_IDENTITY();
            INSERT INTO @Nuevos VALUES (@pid);

            -- Cada unidad: se devuelve (o no todavía) y en qué condición
            DECLARE item CURSOR LOCAL FAST_FORWARD FOR SELECT UnidadId, HerramientaId FROM @Items ORDER BY Orden;
            OPEN item;
            FETCH NEXT FROM item INTO @u, @h;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                SELECT @motor = Motor, @pierde = PierdeFacil FROM #Unidad WHERE UnidadId = @u;

                -- 15 % se devuelve por separado, unos días después (sin pasarse del plazo si iba a tiempo)
                SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
                SET @fDev = CASE WHEN @fBase < @Nunca AND @r < 0.15 THEN DATEADD(DAY, 1 + FLOOR(@r / 0.15 * 3), @fBase) ELSE @fBase END;
                IF @fDev < @Nunca AND DATEDIFF(DAY, '19000107', @fDev) % 7 = 0 SET @fDev = DATEADD(DAY, 1, @fDev);
                IF @patron = 0 AND @fDev < @Nunca AND @fDev > @fEsp SET @fDev = @fBase;

                IF @fDev >= @Ahora
                BEGIN
                    INSERT INTO PrestamoDetalle (PrestamoId, HerramientaId, UnidadId, EstadoDevolucion)
                    VALUES (@pid, @h, @u, 'Pendiente');
                    UPDATE #Unidad SET LibreDesde = @Nunca, EstadoFinal = 'Prestada', Cambio = 1 WHERE UnidadId = @u;
                END
                ELSE
                BEGIN
                    SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
                    SET @cond = CASE WHEN @r < CASE WHEN @pierde = 1 THEN 0.015 ELSE 0.003 END THEN 'Perdido'
                                     WHEN @r < CASE WHEN @motor = 1 THEN 0.06 ELSE 0.035 END THEN 'Dañado'
                                     ELSE 'Bueno' END;
                    SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
                    SET @nota = CASE @cond
                                    WHEN 'Perdido' THEN N'El empleado reporta que se extravió en el área de trabajo'
                                    WHEN 'Dañado'  THEN (SELECT Texto FROM @NotaDanio WHERE Motor = @motor
                                                         AND i = FLOOR(@r * (SELECT COUNT(*) FROM @NotaDanio WHERE Motor = @motor))) END;

                    INSERT INTO PrestamoDetalle (PrestamoId, HerramientaId, UnidadId, FechaDevuelta, EstadoDevolucion, ObservacionDevolucion)
                    VALUES (@pid, @h, @u, @fDev, @cond, @nota);

                    -- Dañada: no vuelve al stock, queda en la bandeja de pendientes de mantenimiento
                    UPDATE #Unidad
                    SET    LibreDesde  = CASE @cond WHEN 'Bueno' THEN @fDev ELSE @Nunca END,
                           EstadoFinal = CASE @cond WHEN 'Bueno' THEN 'Disponible' WHEN 'Dañado' THEN 'Dañada' ELSE 'Perdida' END,
                           Cambio      = 1
                    WHERE  UnidadId = @u;
                END
                FETCH NEXT FROM item INTO @u, @h;
            END
            CLOSE item; DEALLOCATE item;
        END
    END

    SET @dia = DATEADD(DAY, 1, @dia);
END

-- Estado final de las unidades que se movieron
UPDATE hu SET Estado = x.EstadoFinal
FROM   HerramientaUnidad hu INNER JOIN #Unidad x ON x.UnidadId = hu.UnidadId
WHERE  x.Cambio = 1;

-- ════════════════════════════════════════════════════════════
-- 6. Movimiento de hoy (si no es domingo)
-- ════════════════════════════════════════════════════════════
IF DATEDIFF(DAY, '19000107', @Hoy) % 7 <> 0
BEGIN
    -- Al menos una devolución en buen estado: el préstamo pendiente de antes de hoy
    -- con el plazo más próximo que todavía no vence
    IF NOT EXISTS (SELECT 1 FROM PrestamoDetalle WHERE CAST(FechaDevuelta AS DATE) = @Hoy AND EstadoDevolucion = 'Bueno')
    BEGIN
        DECLARE @did INT = NULL;
        SELECT TOP 1 @did = d.PrestamoDetalleId, @u = d.UnidadId, @fPrest = p.FechaPrestamo
        FROM   PrestamoDetalle d INNER JOIN Prestamo p ON p.PrestamoId = d.PrestamoId
        WHERE  d.FechaDevuelta IS NULL AND p.FechaPrestamo < @Hoy AND p.FechaDevolucionEsperada >= @Ahora
        ORDER  BY p.FechaDevolucionEsperada, p.FechaPrestamo;

        IF @did IS NOT NULL
        BEGIN
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            UPDATE PrestamoDetalle
            SET    FechaDevuelta = DATEADD(MINUTE, FLOOR(@r * DATEDIFF(MINUTE, @IniHoy, @FinHoy)), @IniHoy),
                   EstadoDevolucion = 'Bueno'
            WHERE  PrestamoDetalleId = @did;
            UPDATE HerramientaUnidad SET Estado = 'Disponible' WHERE UnidadId = @u;
        END
    END

    -- Un mantenimiento preventivo que entra hoy al taller (interno, queda abierto)
    IF NOT EXISTS (SELECT 1 FROM Mantenimiento WHERE CAST(FechaInicio AS DATE) = @Hoy)
    BEGIN
        SET @u = NULL;
        SELECT TOP 1 @u = hu.UnidadId, @h = hu.HerramientaId
        FROM   HerramientaUnidad hu INNER JOIN #Unidad x ON x.UnidadId = hu.UnidadId
        WHERE  hu.Estado = 'Disponible' AND x.Motor = 1 AND x.LibreDesde <= @IniHoy
          AND  NOT EXISTS (SELECT 1 FROM Mantenimiento m WHERE m.UnidadId = hu.UnidadId AND m.FechaInicio >= DATEADD(DAY, -30, @Hoy))
        ORDER  BY NEWID();

        IF @u IS NOT NULL
        BEGIN
            SET @r = ABS(CHECKSUM(NEWID()) % 1000000) / 1000000.0;
            SELECT @usr = UsuarioId FROM @Aprob WHERE Id = 1 + FLOOR(@r * @nAprob);
            INSERT INTO Mantenimiento (HerramientaId, UnidadId, FechaInicio, TipoMantenimiento, Descripcion, TipoServicio,
                                       EmpleadoId, RegistradoPorId, EnGarantia)
            VALUES (@h, @u, DATEADD(MINUTE, FLOOR(@r * DATEDIFF(MINUTE, @IniHoy, @FinHoy)), @IniHoy), 'Preventivo',
                    'Mantenimiento preventivo: limpieza, lubricación y revisión general', 'Interno',
                    (SELECT EmpleadoId FROM @Tecnico WHERE Id = 1 + FLOOR(@r * @nTec)), @usr, 0);
            UPDATE HerramientaUnidad SET Estado = 'En Mantenimiento' WHERE UnidadId = @u;
        END
    END
END

-- ════════════════════════════════════════════════════════════
-- 7. Al menos 2 préstamos vencidos (indicador rojo, reporte de vencidos y botón "Notificar").
--    Se toman préstamos recientes devueltos en buen estado cuyo plazo venció hace 1 a 12 días
--    y cuyas unidades no tuvieron movimientos después: quedan como "todavía no regresan".
-- ════════════════════════════════════════════════════════════
DECLARE @Faltan INT = 2 - (SELECT COUNT(*) FROM Prestamo WHERE Estado = 'Vencido');
IF @Faltan > 0
BEGIN
    DECLARE @Atrasar TABLE (PrestamoId INT PRIMARY KEY);
    INSERT INTO @Atrasar
    SELECT TOP (@Faltan) p.PrestamoId
    FROM   Prestamo p
    WHERE  p.Estado = 'Cerrado'
      AND  p.FechaDevolucionEsperada >= DATEADD(DAY, -12, @Ahora)
      AND  p.FechaDevolucionEsperada <  DATEADD(DAY, -1, @Ahora)
      AND  NOT EXISTS (SELECT 1 FROM PrestamoDetalle d
                       INNER JOIN HerramientaUnidad u ON u.UnidadId = d.UnidadId
                       WHERE d.PrestamoId = p.PrestamoId
                         AND (d.EstadoDevolucion <> 'Bueno' OR u.Estado <> 'Disponible'
                              OR EXISTS (SELECT 1 FROM PrestamoDetalle d2 INNER JOIN Prestamo p2 ON p2.PrestamoId = d2.PrestamoId
                                         WHERE d2.UnidadId = d.UnidadId AND p2.FechaPrestamo > p.FechaPrestamo)
                              OR EXISTS (SELECT 1 FROM Mantenimiento m WHERE m.UnidadId = d.UnidadId AND m.FechaInicio > p.FechaPrestamo)))
    ORDER  BY p.FechaDevolucionEsperada DESC;

    UPDATE d SET FechaDevuelta = NULL, EstadoDevolucion = 'Pendiente', ObservacionDevolucion = NULL
    FROM   PrestamoDetalle d INNER JOIN @Atrasar a ON a.PrestamoId = d.PrestamoId;

    UPDATE u SET Estado = 'Prestada'
    FROM   HerramientaUnidad u INNER JOIN PrestamoDetalle d ON d.UnidadId = u.UnidadId
    INNER  JOIN @Atrasar a ON a.PrestamoId = d.PrestamoId;

    UPDATE p SET Estado = 'Vencido', FechaCierre = NULL
    FROM   Prestamo p INNER JOIN @Atrasar a ON a.PrestamoId = p.PrestamoId;
END

-- ════════════════════════════════════════════════════════════
-- 8. Estado de los préstamos, igual que sp_Prestamo_CerrarSiCompleto y sp_Prestamo_MarcarVencidos
-- ════════════════════════════════════════════════════════════
UPDATE p
SET    Estado      = CASE WHEN x.Pendientes = 0 THEN 'Cerrado'
                          WHEN p.FechaDevolucionEsperada < @Ahora THEN 'Vencido'
                          ELSE 'Activo' END,
       FechaCierre = CASE WHEN x.Pendientes = 0 THEN x.UltimaDevolucion END
FROM   Prestamo p
CROSS  APPLY (SELECT SUM(CASE WHEN FechaDevuelta IS NULL THEN 1 ELSE 0 END) AS Pendientes,
                     MAX(FechaDevuelta) AS UltimaDevolucion
              FROM PrestamoDetalle WHERE PrestamoId = p.PrestamoId) x
WHERE  p.Estado <> 'Cerrado' OR p.PrestamoId IN (SELECT PrestamoId FROM @Nuevos);

-- Marca de la última puesta al día
DECLARE @Marca NVARCHAR(10) = CONVERT(NCHAR(10), @Hoy, 23);
IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE class = 0 AND name = N'PROMACO_DemoAlDia')
    EXEC sys.sp_updateextendedproperty @name = N'PROMACO_DemoAlDia', @value = @Marca;
ELSE
    EXEC sys.sp_addextendedproperty @name = N'PROMACO_DemoAlDia', @value = @Marca;

COMMIT TRANSACTION;

-- Resumen
SELECT @Desde                                                                     AS SimuladoDesde,
       (SELECT COUNT(*) FROM @Nuevos)                                             AS PrestamosNuevos,
       (SELECT COUNT(*) FROM Prestamo WHERE Estado = 'Activo')                    AS Activos,
       (SELECT COUNT(*) FROM Prestamo WHERE Estado = 'Vencido')                   AS Vencidos,
       (SELECT COUNT(*) FROM Prestamo WHERE CAST(FechaPrestamo AS DATE) = @Hoy)   AS PrestamosHoy,
       (SELECT COUNT(*) FROM PrestamoDetalle WHERE CAST(FechaDevuelta AS DATE) = @Hoy) AS DevolucionesHoy,
       (SELECT COUNT(*) FROM Mantenimiento WHERE FechaFin IS NULL)                AS MantenimientosAbiertos;
GO

DROP TABLE IF EXISTS #Prestatario, #Unidad, #Peso;
GO
SET NOEXEC OFF;
GO
