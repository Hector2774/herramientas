-- ============================================================
-- PROMACO · Sistema de Control de Herramientas
-- Datos de demostración: seis meses de operación de la bodega
--
-- Simula día por día (desde el primer día de hace seis meses hasta hoy) los préstamos,
-- devoluciones y mantenimientos de las herramientas, con las mismas reglas que la app:
--   · una unidad nunca está en dos préstamos o mantenimientos a la vez
--   · la mayoría de devoluciones llega a tiempo; algunas tarde; pocas dañadas o perdidas
--   · una devolución dañada genera un mantenimiento correctivo ligado a ella
--     (interno con técnico de la empresa, o externo con proveedor y factura)
--   · mantenimiento preventivo mensual y calibración trimestral del nivel láser
--   · lo que "todavía no se devuelve" queda como préstamo activo o vencido, y el estado
--     final de cada unidad coincide con su último movimiento
--
-- Es reproducible: usa una secuencia pseudoaleatoria fija (mismo resultado cada vez).
-- No toca préstamos ni mantenimientos ya existentes: las unidades que tienen movimientos
-- reales quedan fuera de la simulación.
--
-- En una instalación nueva (después de 000 y 001) crea también las 20 herramientas y
-- empleados de ejemplo, porque la sincronización con RRHH solo funciona en la red de la empresa.
--
-- Solo se puede cargar una vez por base (queda marcada con la propiedad PROMACO_DatosDemo).
--
--   sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i datos_demo.sql
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

IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE class = 0 AND name = N'PROMACO_DatosDemo')
BEGIN
    RAISERROR(N'Los datos de demostración ya se cargaron en esta base.', 16, 1);
    SET NOEXEC ON;
END
GO

IF NOT EXISTS (SELECT 1 FROM Usuario u INNER JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId WHERE u.Activo = 1 AND e.Activo = 1)
BEGIN
    RAISERROR(N'No hay usuarios activos: ejecute primero Database/instalacion/001_datos_iniciales.sql.', 16, 1);
    SET NOEXEC ON;
END
GO

BEGIN TRANSACTION;

DECLARE @Ahora  DATETIME2(0) = SYSDATETIME();
DECLARE @Hoy    DATE         = CAST(@Ahora AS DATE);
DECLARE @Inicio DATE         = DATEFROMPARTS(YEAR(DATEADD(MONTH, -6, @Hoy)), MONTH(DATEADD(MONTH, -6, @Hoy)), 1);
DECLARE @Nunca  DATETIME2(0) = '9999-12-31';

-- ════════════════════════════════════════════════════════════
-- 1. Catálogos que la demo necesita
-- ════════════════════════════════════════════════════════════

-- Proveedores: uno para reparaciones y otro para calibraciones
IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = 'Electromecánica Del Valle')
    INSERT INTO Proveedor (Nombre, Telefono, Descripcion, Activo)
    VALUES ('Electromecánica Del Valle', '2440-1188', 'Reparación de herramientas eléctricas e inalámbricas', 1);
IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = 'Metrología Industrial, S.A.')
    INSERT INTO Proveedor (Nombre, Telefono, Descripcion, Activo)
    VALUES ('Metrología Industrial, S.A.', '2361-7720', 'Calibración de instrumentos de medición', 1);

-- Herramientas (solo en una base sin herramientas: instalación nueva)
IF NOT EXISTS (SELECT 1 FROM Herramienta)
BEGIN
    DECLARE @Herr TABLE (Orden INT PRIMARY KEY, Nombre NVARCHAR(120), Car NVARCHAR(400), C INT, M INT, U INT, N INT, F NVARCHAR(200));
    INSERT INTO @Herr VALUES
        ( 1, N'Taladro percutor 1/2"',          N'750 W, velocidad variable, reversible',      1, 0, 1,  4, N'taladro_percutor.jpg'),
        ( 2, N'Esmeriladora angular 4 1/2"',    N'850 W, 11,000 RPM',                           1, 1, 1,  3, N'esmeriladora.png'),
        ( 3, N'Sierra circular 7 1/4"',         N'1,800 W, corte a 45°',                        1, 2, 1,  2, N'sierra_circular.jpg'),
        ( 4, N'Rotomartillo SDS Plus',          N'800 W, 3 modos de trabajo',                   1, 2, 1,  2, N'rotomartillo.jpg'),
        ( 5, N'Lijadora orbital',               N'300 W, base de 1/4 de hoja',                  1, 0, 1,  3, N'lijadora_orbital.jpeg'),
        ( 6, N'Atornillador de impacto 20V',    N'Incluye 2 baterías y cargador',               2, 5, 1,  3, N'atornillador_impacto.jpg'),
        ( 7, N'Taladro inalámbrico 18V',        N'Mandril 1/2", 2 velocidades',                 2, 1, 1,  4, N'taladro_inalambrico.jpeg'),
        ( 8, N'Sierra caladora inalámbrica',    N'20V, carrera de 1"',                          2, 0, 1,  2, N'sierra_caladora.jpg'),
        ( 9, N'Juego de desarmadores 10 pzas',  N'Punta plana y Phillips, mango ergonómico',    3, 3, 2,  6, N'desarmadores_truper.jpg'),
        (10, N'Martillo de uña 16 oz',          N'Mango de fibra de vidrio',                    3, 4, 2,  8, N'martillo_unia.jpeg'),
        (11, N'Llave stillson 14"',             N'Acero forjado',                               3, 3, 2,  4, N'llave_stilson.jpg'),
        (12, N'Juego de llaves combinadas',     N'12 piezas, milimétricas',                     3, 6, 2,  5, N'llaves_combinadas.jpg'),
        (13, N'Pinza de presión 10"',           N'Mordaza curva',                               3, 4, 2,  6, N'pinza_presion.jpg'),
        (14, N'Flexómetro 8 m',                 N'Cinta de 1", con freno',                      4, 4, 3, 10, N'flexometro.jpg'),
        (15, N'Nivel láser de líneas',          N'Autonivelante, alcance 15 m',                 4, 2, 3,  2, N'nivel_laser.jpeg'),
        (16, N'Nivel de aluminio 24"',          N'3 burbujas',                                  4, 3, 3,  5, N'nivel_aluminio.jpeg'),
        (17, N'Pistola de impacto neumática',   N'Cuadro de 1/2", 90 PSI',                      5, 3, 4,  2, N'pistola_neumatica.jpg'),
        (18, N'Podadora de césped',             N'Motor a gasolina 4 tiempos, 20"',             6, 3, 4,  1, N'podadora.jpg'),
        (19, N'Tijera para podar',              N'Hoja de acero al carbono 8"',                 6, 6, 4,  4, N'tijera_podar.jpg'),
        (20, N'Careta para soldar',             N'Oscurecimiento automático',                   7, 7, 5,  3, N'careta_soldar.jpg');

    DECLARE @Creada TABLE (HerramientaId INT, Codigo VARCHAR(20));
    DECLARE @o INT = 1, @hNom NVARCHAR(120), @hCar NVARCHAR(400), @hC INT, @hM INT, @hU INT, @hN INT, @hF NVARCHAR(200);
    WHILE @o <= 20
    BEGIN
        SELECT @hNom = Nombre, @hCar = Car, @hC = C, @hM = M, @hU = U, @hN = N, @hF = F FROM @Herr WHERE Orden = @o;
        INSERT INTO @Creada EXEC sp_Herramienta_Insertar @Nombre = @hNom, @Caracteristicas = @hCar, @CategoriaId = @hC,
             @MarcaId = @hM, @UbicacionId = @hU, @StockTotal = @hN, @FotoNombre = @hF;
        SET @o += 1;
    END
END

-- Departamentos que usan herramientas y su peso en la demanda
DECLARE @Depto TABLE (Nombre NVARCHAR(120) PRIMARY KEY, Peso FLOAT);
INSERT INTO @Depto VALUES
    (N'BODEGA CEDIS', 10), (N'BODEGA TIENDA', 6), (N'BODEGA HIERRO', 5), (N'BODEGA MADERA', 4),
    (N'TALLER DE MANTENIMIENTO', 6), (N'INVENTARIO', 3), (N'OPERACIONES', 3), (N'TRANSPORTE', 3),
    (N'SEGURIDAD', 2), (N'LIMPIEZA Y ASEO', 2);

-- Empleados que aprueban (los de usuarios activos) no pueden pedir prestado a sí mismos
DECLARE @Aprob TABLE (Id INT IDENTITY PRIMARY KEY, EmpleadoId INT, UsuarioId INT);
INSERT INTO @Aprob (EmpleadoId, UsuarioId)
SELECT e.EmpleadoId, u.UsuarioId
FROM   Usuario u INNER JOIN Empleado e ON e.EmpleadoId = u.EmpleadoId
WHERE  u.Activo = 1 AND e.Activo = 1
ORDER  BY u.UsuarioId;

-- Empleados de ejemplo (solo si la base no tiene personal en esos departamentos)
IF (SELECT COUNT(*) FROM Empleado e INNER JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    INNER JOIN @Depto x ON x.Nombre = d.Nombre
    WHERE e.Activo = 1 AND e.EmpleadoId NOT IN (SELECT EmpleadoId FROM @Aprob)) < 10
BEGIN
    INSERT INTO Departamento (Nombre, Activo)
    SELECT x.Nombre, 1 FROM @Depto x WHERE NOT EXISTS (SELECT 1 FROM Departamento d WHERE d.Nombre = x.Nombre);

    DECLARE @Nombres TABLE (i INT IDENTITY PRIMARY KEY, Nombre NVARCHAR(120));
    INSERT INTO @Nombres (Nombre) VALUES
        (N'JOSÉ LUIS PÉREZ ORELLANA'), (N'MARVIN ESTUARDO LÓPEZ CHÁVEZ'), (N'CARLOS HUMBERTO RAMÍREZ SOTO'),
        (N'BYRON ALEXANDER GARCÍA MÉNDEZ'), (N'EDGAR RENÉ MORALES CASTILLO'), (N'JUAN PABLO HERNÁNDEZ AJÚ'),
        (N'KEVIN ESTUARDO GÓMEZ PAZ'), (N'LUIS FERNANDO DE LEÓN ROJAS'), (N'MARIO ROBERTO CIFUENTES LIMA'),
        (N'OSCAR DANIEL REYES BARRIOS'), (N'WALTER ALFONSO CHÁVEZ TUY'), (N'ERICK JOSUÉ ARGUETA MÉRIDA'),
        (N'HUGO LEONEL SANTIZO VÁSQUEZ'), (N'DIEGO ANDRÉS SOLÓRZANO PINEDA'), (N'RONY GEOVANI XOCOP CHAJ'),
        (N'ANA LUCÍA MONTERROSO FUENTES'), (N'JORGE MARIO QUIÑÓNEZ ESCOBAR'), (N'SELVIN OTONIEL TZUL COJ'),
        (N'FRANCISCO JAVIER ALVARADO RUANO'), (N'ALEX ROLANDO MAZARIEGOS POP'), (N'CRISTIAN MANUEL OVALLE SICAJÁ'),
        (N'HÉCTOR GIOVANNI JUÁREZ PIRIR'), (N'VÍCTOR MANUEL CANO ESTRADA'), (N'DENNIS OMAR BATRES LEMUS'),
        (N'RAFAEL ANTONIO ICAL CAAL'), (N'ELMER ADOLFO VELÁSQUEZ GODOY'), (N'GERSON DAVID MONZÓN AGUILAR'),
        (N'ARIEL ENRIQUE PORTILLO HUINAC'), (N'SERGIO IVÁN FIGUEROA CORADO'), (N'MIGUEL ÁNGEL TECÚN SAQUIC');

    -- Tres por departamento, en el orden de @Depto
    INSERT INTO Empleado (Codigo, Nombre, Activo, DepartamentoId)
    SELECT N'EMP-' + CAST(1000 + n.i AS NVARCHAR(10)), n.Nombre, 1, d.DepartamentoId
    FROM   @Nombres n
    INNER  JOIN (SELECT Nombre, ROW_NUMBER() OVER (ORDER BY Peso DESC, Nombre) AS Pos FROM @Depto) x ON x.Pos = (n.i - 1) / 3 + 1
    INNER  JOIN Departamento d ON d.Nombre = x.Nombre
    WHERE  NOT EXISTS (SELECT 1 FROM Empleado e WHERE e.Codigo = N'EMP-' + CAST(1000 + n.i AS NVARCHAR(10)));
END

-- ════════════════════════════════════════════════════════════
-- 2. Participantes, herramientas y unidades de la simulación
-- ════════════════════════════════════════════════════════════

-- Quién pide prestado: hasta 8 personas por departamento (un grupo que se repite, como en la realidad).
-- Cada departamento conserva su peso total, repartido entre sus empleados.
CREATE TABLE #Prestatario (EmpleadoId INT PRIMARY KEY, Desde FLOAT, Hasta FLOAT);
WITH Candidatos AS (
    SELECT e.EmpleadoId, x.Peso,
           ROW_NUMBER() OVER (PARTITION BY d.DepartamentoId ORDER BY e.EmpleadoId) AS Pos,
           COUNT(*)     OVER (PARTITION BY d.DepartamentoId)                       AS EnDepto
    FROM   Empleado e
    INNER  JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    INNER  JOIN @Depto       x ON x.Nombre = d.Nombre
    WHERE  e.Activo = 1 AND e.EmpleadoId NOT IN (SELECT EmpleadoId FROM @Aprob)
), Elegidos AS (
    SELECT EmpleadoId, Peso / CASE WHEN EnDepto > 8 THEN 8 ELSE EnDepto END AS W
    FROM   Candidatos WHERE Pos <= 8
), Acumulado AS (
    SELECT EmpleadoId, SUM(W) OVER (ORDER BY EmpleadoId ROWS UNBOUNDED PRECEDING) AS Hasta, W, SUM(W) OVER () AS Total
    FROM   Elegidos
)
INSERT INTO #Prestatario (EmpleadoId, Desde, Hasta)
SELECT EmpleadoId, (Hasta - W) / Total, Hasta / Total FROM Acumulado;

IF NOT EXISTS (SELECT 1 FROM #Prestatario)
    THROW 50000, 'No hay empleados en los departamentos de bodega/operaciones para simular préstamos.', 1;

-- Técnicos internos: el taller de mantenimiento (o, si no hay, personal de bodega)
DECLARE @Tecnico TABLE (Id INT IDENTITY PRIMARY KEY, EmpleadoId INT);
INSERT INTO @Tecnico (EmpleadoId)
SELECT e.EmpleadoId FROM Empleado e INNER JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
WHERE  e.Activo = 1 AND d.Nombre = N'TALLER DE MANTENIMIENTO' ORDER BY e.EmpleadoId;
IF NOT EXISTS (SELECT 1 FROM @Tecnico)
    INSERT INTO @Tecnico (EmpleadoId) SELECT TOP 2 EmpleadoId FROM #Prestatario ORDER BY EmpleadoId;

DECLARE @ProvRep INT = (SELECT ProveedorId FROM Proveedor WHERE Nombre = 'Electromecánica Del Valle');
DECLARE @ProvCal INT = (SELECT ProveedorId FROM Proveedor WHERE Nombre = 'Metrología Industrial, S.A.');
DECLARE @ProvOtro INT = (SELECT TOP 1 ProveedorId FROM Proveedor WHERE Activo = 1 AND ProveedorId NOT IN (@ProvRep, @ProvCal) ORDER BY ProveedorId);
DECLARE @nAprob INT = (SELECT COUNT(*) FROM @Aprob), @nTec INT = (SELECT COUNT(*) FROM @Tecnico);

-- Demanda relativa de cada herramienta (por código)
DECLARE @Demanda TABLE (Codigo VARCHAR(20) PRIMARY KEY, Peso FLOAT);
INSERT INTO @Demanda VALUES
    ('HER-0001', 10), ('HER-0002', 7), ('HER-0003', 5), ('HER-0004', 5), ('HER-0005', 3),
    ('HER-0006', 8),  ('HER-0007', 10), ('HER-0008', 4), ('HER-0009', 9), ('HER-0010', 9),
    ('HER-0011', 4),  ('HER-0012', 6), ('HER-0013', 5), ('HER-0014', 10), ('HER-0015', 3),
    ('HER-0016', 4),  ('HER-0017', 3), ('HER-0018', 2), ('HER-0019', 3), ('HER-0020', 4);

-- Unidades que participan: disponibles hoy y sin ningún préstamo ni mantenimiento registrado
CREATE TABLE #Unidad (
    UnidadId      INT PRIMARY KEY,
    HerramientaId INT,
    Motor         BIT,               -- eléctrica, inalámbrica, neumática o a gasolina
    PierdeFacil   BIT,               -- herramienta de mano o de medición (pequeña)
    LibreDesde    DATETIME2(0),
    Retirada      BIT DEFAULT 0,     -- perdida o dada de baja
    EstadoFinal   VARCHAR(20) DEFAULT 'Disponible',
    FechaBaja     DATETIME2(0) NULL
);
INSERT INTO #Unidad (UnidadId, HerramientaId, Motor, PierdeFacil, LibreDesde)
SELECT u.UnidadId, u.HerramientaId,
       CASE WHEN h.CategoriaId IN (1, 2, 5) OR h.Codigo = 'HER-0018' THEN 1 ELSE 0 END,
       CASE WHEN h.CategoriaId IN (3, 4) THEN 1 ELSE 0 END,
       @Inicio
FROM   HerramientaUnidad u
INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
INNER  JOIN @Demanda    w ON w.Codigo = h.Codigo
WHERE  h.Activa = 1 AND h.PrestamoHabilitado = 1 AND u.Estado = 'Disponible'
  AND  NOT EXISTS (SELECT 1 FROM PrestamoDetalle pd WHERE pd.UnidadId = u.UnidadId)
  AND  NOT EXISTS (SELECT 1 FROM Mantenimiento   m  WHERE m.UnidadId  = u.UnidadId);

-- Herramientas con unidades en la simulación, con su rango de probabilidad
CREATE TABLE #Herramienta (HerramientaId INT PRIMARY KEY, Desde FLOAT, Hasta FLOAT);
WITH Pesos AS (
    SELECT h.HerramientaId, w.Peso,
           SUM(w.Peso) OVER (ORDER BY h.Codigo ROWS UNBOUNDED PRECEDING) AS Acum, SUM(w.Peso) OVER () AS Total
    FROM   Herramienta h INNER JOIN @Demanda w ON w.Codigo = h.Codigo
    WHERE  h.HerramientaId IN (SELECT HerramientaId FROM #Unidad)
)
INSERT INTO #Herramienta SELECT HerramientaId, (Acum - Peso) / Total, Acum / Total FROM Pesos;

-- El inventario físico ya existía antes del historial: alta de las unidades antes del inicio
UPDATE HerramientaUnidad SET FechaAlta = DATEADD(DAY, -45, @Inicio)
WHERE  FechaAlta > DATEADD(DAY, -45, @Inicio) AND Estado <> 'Baja';

-- ════════════════════════════════════════════════════════════
-- 3. Secuencia pseudoaleatoria fija y textos
-- ════════════════════════════════════════════════════════════
CREATE TABLE #Azar (i INT PRIMARY KEY, r FLOAT);
WITH D AS (SELECT v FROM (VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) AS t(v))
INSERT INTO #Azar (i, r)
SELECT n, CAST(CAST(CAST(HASHBYTES('SHA2_256', CONCAT('promaco-demo-', n)) AS BINARY(4)) AS BIGINT) AS FLOAT) / 4294967296.0
FROM  (SELECT a.v * 10000 + b.v * 1000 + c.v * 100 + d.v * 10 + e.v + 1 AS n
       FROM D a CROSS JOIN D b CROSS JOIN D c CROSS JOIN D d CROSS JOIN D e) x;

DECLARE @NotaDanio TABLE (Motor BIT, i INT, Texto NVARCHAR(250), Arreglo VARCHAR(400), PRIMARY KEY (Motor, i));
INSERT INTO @NotaDanio VALUES
    (1, 0, N'No enciende',                          'Se reemplazó el interruptor y se probó en vacío'),
    (1, 1, N'Carbones gastados, chispea al usarla', 'Se cambiaron carbones y se limpió el colector'),
    (1, 2, N'Cable de alimentación pelado',         'Se reemplazó el cable de alimentación'),
    (1, 3, N'Batería no retiene la carga',          'Se reemplazó la batería'),
    (1, 4, N'Mandril flojo',                        'Se ajustó y lubricó el mandril'),
    (1, 5, N'Ruido anormal en el motor',            'Se cambiaron rodamientos del motor'),
    (0, 0, N'Mango quebrado',                       'Se reemplazó el mango'),
    (0, 1, N'Mordaza desalineada',                  'Se ajustó la mordaza'),
    (0, 2, N'Cinta doblada, no regresa',            'Se cambió la cinta'),
    (0, 3, N'Golpe fuerte, pieza deformada',        'Se reemplazó la pieza dañada'),
    (0, 4, N'Visor rayado',                         'Se cambió el visor');

DECLARE @Uso TABLE (i INT PRIMARY KEY, Texto NVARCHAR(400));
INSERT INTO @Uso VALUES
    (0, N'Inventario físico en bodega'), (1, N'Armado de estanterías'), (2, N'Reparación de tarimas'),
    (3, N'Instalación de rótulos en tienda'), (4, N'Corte de varilla para pedido de cliente'),
    (5, N'Ordenamiento de bodega de madera'), (6, N'Reparaciones en área de carga y descarga'),
    (7, N'Mantenimiento de puertas y portones');

-- ════════════════════════════════════════════════════════════
-- 4. Simulación día por día
-- ════════════════════════════════════════════════════════════
DECLARE @k INT = 1, @r FLOAT;                       -- @k: posición en la secuencia; @r: número en [0,1)
DECLARE @dia DATE = @Inicio, @dow INT, @mes INT = 0, @nPrest INT, @p INT, @nItems INT, @it INT;
DECLARE @emp INT, @aprobEmp INT, @usr INT, @fPrest DATETIME2(0), @dur INT, @fEsp DATETIME2(0), @fBase DATETIME2(0), @fDev DATETIME2(0);
DECLARE @h INT, @u INT, @pid INT, @pdid INT, @patron INT, @cond VARCHAR(20), @nota NVARCHAR(250), @arreglo VARCHAR(400);
DECLARE @motor BIT, @pierde BIT, @m0 DATETIME2(0), @m1 DATETIME2(0), @externo BIT, @garantia BIT;
DECLARE @mat DECIMAL(10,2), @mano DECIMAL(10,2), @folio INT = 10230, @baja BIT, @n INT;
DECLARE @Items TABLE (Orden INT IDENTITY PRIMARY KEY, UnidadId INT);

WHILE @dia <= @Hoy
BEGIN
    SET @dow = DATEDIFF(DAY, '19000107', @dia) % 7;      -- 0 = domingo, 6 = sábado (sin depender de DATEFIRST)

    IF @dow <> 0
    BEGIN
        -- ── Mantenimiento programado: primer día hábil de cada mes ──
        IF MONTH(@dia) <> @mes
        BEGIN
            SET @mes = MONTH(@dia);

            -- Preventivo interno de 3 herramientas con motor
            SET @n = 0;
            WHILE @n < 3
            BEGIN
                SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                SET @u = NULL;
                SELECT TOP 1 @u = UnidadId, @h = HerramientaId FROM #Unidad
                WHERE  Motor = 1 AND Retirada = 0 AND LibreDesde <= DATEADD(MINUTE, 450, CAST(@dia AS DATETIME2(0)))
                ORDER  BY CHECKSUM(UnidadId * 7919 + @k);
                IF @u IS NOT NULL
                BEGIN
                    SET @m0 = DATEADD(MINUTE, 450, CAST(@dia AS DATETIME2(0)));
                    SET @m1 = DATEADD(MINUTE, 600 + FLOOR(@r * 420), @m0);
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                    SET @mat = ROUND((35 + @r * 85) / 5, 0) * 5;
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                    SELECT @usr = UsuarioId FROM @Aprob WHERE Id = 1 + FLOOR(@r * @nAprob);
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;

                    IF @m0 < @Ahora
                    BEGIN
                        INSERT INTO Mantenimiento (HerramientaId, UnidadId, FechaInicio, TipoMantenimiento, Descripcion, TipoServicio,
                               EmpleadoId, RegistradoPorId, FechaFin, NotasCierre, CostoMateriales, Costo, EnGarantia, Resultado, CerradoPorId)
                        SELECT @h, @u, @m0, 'Preventivo', 'Mantenimiento preventivo mensual: limpieza, lubricación y revisión general',
                               'Interno', (SELECT EmpleadoId FROM @Tecnico WHERE Id = 1 + FLOOR(@r * @nTec)), @usr,
                               CASE WHEN @m1 < @Ahora THEN @m1 END,
                               CASE WHEN @m1 < @Ahora THEN 'Sin novedades; lista para préstamo' END,
                               CASE WHEN @m1 < @Ahora THEN @mat END,
                               CASE WHEN @m1 < @Ahora THEN @mat END,
                               0,
                               CASE WHEN @m1 < @Ahora THEN 'Reparada' END,
                               CASE WHEN @m1 < @Ahora THEN @usr END;

                        UPDATE #Unidad
                        SET    LibreDesde  = CASE WHEN @m1 < @Ahora THEN @m1 ELSE @Nunca END,
                               EstadoFinal = CASE WHEN @m1 < @Ahora THEN 'Disponible' ELSE 'En Mantenimiento' END
                        WHERE  UnidadId = @u;
                    END
                END
                SET @n += 1;
            END

            -- Calibración externa trimestral del nivel láser (enero, abril, julio, octubre)
            IF MONTH(@dia) IN (1, 4, 7, 10) AND @ProvCal IS NOT NULL
            BEGIN
                DECLARE cal CURSOR LOCAL FAST_FORWARD FOR
                    SELECT x.UnidadId, x.HerramientaId FROM #Unidad x INNER JOIN Herramienta hh ON hh.HerramientaId = x.HerramientaId
                    WHERE  hh.Codigo = 'HER-0015' AND x.Retirada = 0 AND x.LibreDesde <= DATEADD(MINUTE, 480, CAST(@dia AS DATETIME2(0)));
                OPEN cal;
                FETCH NEXT FROM cal INTO @u, @h;
                WHILE @@FETCH_STATUS = 0
                BEGIN
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                    SET @m0 = DATEADD(MINUTE, 480, CAST(@dia AS DATETIME2(0)));
                    SET @m1 = DATEADD(MINUTE, 300 + FLOOR(@r * 120), DATEADD(DAY, 3 + FLOOR(@r * 3), @m0));
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                    SET @mano = ROUND((300 + @r * 150) / 5, 0) * 5;
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                    SELECT @usr = UsuarioId FROM @Aprob WHERE Id = 1 + FLOOR(@r * @nAprob);

                    IF @m0 < @Ahora
                    BEGIN
                        SET @folio += 1 + FLOOR(@r * 40);
                        INSERT INTO Mantenimiento (HerramientaId, UnidadId, FechaInicio, TipoMantenimiento, Descripcion, TipoServicio,
                               ProveedorId, RegistradoPorId, FechaFin, NotasCierre, CostoMateriales, CostoManoObra, Costo,
                               EnGarantia, FolioFactura, Resultado, CerradoPorId)
                        SELECT @h, @u, @m0, 'Calibración', 'Calibración trimestral del nivel láser', 'Externo', @ProvCal, @usr,
                               CASE WHEN @m1 < @Ahora THEN @m1 END,
                               CASE WHEN @m1 < @Ahora THEN 'Calibrado y certificado dentro de tolerancia' END,
                               CASE WHEN @m1 < @Ahora THEN 0 END,
                               CASE WHEN @m1 < @Ahora THEN @mano END,
                               CASE WHEN @m1 < @Ahora THEN @mano END,
                               0,
                               CASE WHEN @m1 < @Ahora THEN 'FAC-' + CAST(@folio AS VARCHAR(10)) END,
                               CASE WHEN @m1 < @Ahora THEN 'Reparada' END,
                               CASE WHEN @m1 < @Ahora THEN @usr END;

                        UPDATE #Unidad
                        SET    LibreDesde  = CASE WHEN @m1 < @Ahora THEN @m1 ELSE @Nunca END,
                               EstadoFinal = CASE WHEN @m1 < @Ahora THEN 'Disponible' ELSE 'En Mantenimiento' END
                        WHERE  UnidadId = @u;
                    END
                    FETCH NEXT FROM cal INTO @u, @h;
                END
                CLOSE cal; DEALLOCATE cal;
            END
        END

        -- ── Préstamos del día: más en julio (inventario de medio año) y septiembre, menos en abril ──
        SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
        SET @nPrest = CASE WHEN @dow = 6 THEN CASE WHEN @r < 0.6 THEN 0 ELSE 1 END
                           WHEN @r < 0.12 THEN 0 WHEN @r < 0.42 THEN 1 WHEN @r < 0.78 THEN 2 WHEN @r < 0.95 THEN 3 ELSE 4 END;
        SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
        IF MONTH(@dia) IN (7, 9) AND @r < 0.35 SET @nPrest += 1;
        IF MONTH(@dia) = 4 AND @r < 0.3 AND @nPrest > 0 SET @nPrest -= 1;

        SET @p = 0;
        WHILE @p < @nPrest
        BEGIN
            SET @p += 1;

            -- Quién pide, quién aprueba y cuándo
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SELECT @emp = EmpleadoId FROM #Prestatario WHERE @r >= Desde AND @r < Hasta;
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SELECT @aprobEmp = EmpleadoId, @usr = UsuarioId FROM @Aprob WHERE Id = 1 + FLOOR(@r * @nAprob);
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SET @fPrest = DATEADD(MINUTE, 420 + FLOOR(@r * 540), CAST(@dia AS DATETIME2(0)));      -- 07:00 a 16:00
            IF @fPrest >= @Ahora CONTINUE;

            -- Plazo pactado (a las 17:00, nunca en domingo)
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SET @dur = CASE WHEN @r < 0.25 THEN 1 WHEN @r < 0.50 THEN 2 WHEN @r < 0.70 THEN 3
                            WHEN @r < 0.85 THEN 5 WHEN @r < 0.95 THEN 7 ELSE 14 END;
            SET @fEsp = DATEADD(HOUR, 17, CAST(DATEADD(DAY, @dur, @dia) AS DATETIME2(0)));
            IF DATEDIFF(DAY, '19000107', @fEsp) % 7 = 0 SET @fEsp = DATEADD(DAY, 1, @fEsp);

            -- Cuándo se devuelve: 78 % a tiempo, 18 % con atraso corto, 4 % con atraso largo
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SET @patron = CASE WHEN @r < 0.78 THEN 0 WHEN @r < 0.96 THEN 1 ELSE 2 END;
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SET @fBase = CAST(DATEADD(DAY, CASE @patron WHEN 0 THEN FLOOR(@r * (DATEDIFF(DAY, @dia, CAST(@fEsp AS DATE)) + 1))
                                                        WHEN 1 THEN DATEDIFF(DAY, @dia, CAST(@fEsp AS DATE)) + 1 + FLOOR(@r * 5)
                                                        ELSE        DATEDIFF(DAY, @dia, CAST(@fEsp AS DATE)) + 7 + FLOOR(@r * 9) END,
                                      @dia) AS DATETIME2(0));
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SET @fBase = DATEADD(MINUTE, 480 + FLOOR(@r * 540), @fBase);                          -- 08:00 a 17:00
            IF DATEDIFF(DAY, '19000107', @fBase) % 7 = 0 SET @fBase = DATEADD(DAY, 1, @fBase);
            IF @patron = 0 AND @fBase > @fEsp SET @fBase = DATEADD(MINUTE, -30, @fEsp);
            IF @fBase <= @fPrest SET @fBase = DATEADD(HOUR, 2, @fPrest);

            -- Qué se lleva: 1 a 3 unidades libres
            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            SET @nItems = CASE WHEN @r < 0.6 THEN 1 WHEN @r < 0.9 THEN 2 ELSE 3 END;
            DELETE FROM @Items;
            SET @it = 0;
            WHILE @it < @nItems
            BEGIN
                SET @it += 1;
                SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                SELECT @h = HerramientaId FROM #Herramienta WHERE @r >= Desde AND @r < Hasta;
                SET @u = NULL;
                SELECT TOP 1 @u = UnidadId FROM #Unidad
                WHERE  HerramientaId = @h AND Retirada = 0 AND LibreDesde <= @fPrest
                  AND  UnidadId NOT IN (SELECT UnidadId FROM @Items)
                ORDER  BY CHECKSUM(UnidadId * 7919 + @k);
                IF @u IS NOT NULL INSERT INTO @Items (UnidadId) VALUES (@u);
            END
            IF NOT EXISTS (SELECT 1 FROM @Items) CONTINUE;

            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
            INSERT INTO Prestamo (EmpleadoId, AprobadoPorId, FechaPrestamo, FechaDevolucionEsperada, Estado, Observaciones)
            VALUES (@emp, @aprobEmp, @fPrest, @fEsp, 'Activo',
                    CASE WHEN @r < 0.3 THEN (SELECT Texto FROM @Uso WHERE i = FLOOR(@r / 0.3 * 8)) END);
            SET @pid = SCOPE_IDENTITY();

            -- Cada unidad: devolución, condición y lo que le pasa después
            DECLARE item CURSOR LOCAL FAST_FORWARD FOR SELECT UnidadId FROM @Items ORDER BY Orden;
            OPEN item;
            FETCH NEXT FROM item INTO @u;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                SELECT @h = HerramientaId, @motor = Motor, @pierde = PierdeFacil FROM #Unidad WHERE UnidadId = @u;

                -- 15 % se devuelve por separado (devolución parcial), unos días después
                SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                SET @fDev = CASE WHEN @r < 0.15 THEN DATEADD(DAY, 1 + FLOOR(@r / 0.15 * 3), @fBase) ELSE @fBase END;
                IF DATEDIFF(DAY, '19000107', @fDev) % 7 = 0 SET @fDev = DATEADD(DAY, 1, @fDev);
                -- Si el préstamo se devolvía a tiempo, la devolución parcial tampoco pasa del plazo
                IF @patron = 0 AND @fDev > @fEsp SET @fDev = CASE WHEN @fBase < DATEADD(MINUTE, -30, @fEsp) THEN DATEADD(MINUTE, -30, @fEsp) ELSE @fBase END;

                IF @fDev >= @Ahora
                BEGIN
                    -- Todavía no regresa: sigue prestada
                    INSERT INTO PrestamoDetalle (PrestamoId, HerramientaId, UnidadId, EstadoDevolucion)
                    VALUES (@pid, @h, @u, 'Pendiente');
                    UPDATE #Unidad SET LibreDesde = @Nunca, EstadoFinal = 'Prestada' WHERE UnidadId = @u;
                END
                ELSE
                BEGIN
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                    SET @cond = CASE WHEN @r < CASE WHEN @pierde = 1 THEN 0.02 ELSE 0.004 END THEN 'Perdido'
                                     WHEN @r < CASE WHEN @motor = 1 THEN 0.075 ELSE 0.045 END THEN 'Dañado'
                                     ELSE 'Bueno' END;
                    SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                    SET @nota = NULL; SET @arreglo = NULL;
                    SELECT @nota = Texto, @arreglo = Arreglo FROM @NotaDanio
                    WHERE  Motor = @motor AND i = FLOOR(@r * (SELECT COUNT(*) FROM @NotaDanio WHERE Motor = @motor));
                    IF @cond = 'Perdido' SET @nota = N'El empleado reporta que se extravió en el área de trabajo';
                    IF @cond = 'Bueno' SET @nota = NULL;

                    INSERT INTO PrestamoDetalle (PrestamoId, HerramientaId, UnidadId, FechaDevuelta, EstadoDevolucion, ObservacionDevolucion)
                    VALUES (@pid, @h, @u, @fDev, @cond, @nota);
                    SET @pdid = SCOPE_IDENTITY();

                    IF @cond = 'Bueno'
                        UPDATE #Unidad SET LibreDesde = @fDev, EstadoFinal = 'Disponible' WHERE UnidadId = @u;
                    ELSE IF @cond = 'Perdido'
                        UPDATE #Unidad SET LibreDesde = @Nunca, Retirada = 1, EstadoFinal = 'Perdida' WHERE UnidadId = @u;
                    ELSE
                    BEGIN
                        -- Dañada: entra a mantenimiento correctivo 0 a 3 días después.
                        -- Si se dañó en el último mes, más de la mitad sigue en la bandeja de pendientes.
                        SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                        SET @m0 = DATEADD(MINUTE, 60 + FLOOR(@r * 240), CAST(DATEADD(DAY, FLOOR(@r * 4), CAST(@fDev AS DATE)) AS DATETIME2(0)));
                        SET @m0 = DATEADD(HOUR, 8, @m0);
                        IF @m0 <= @fDev SET @m0 = DATEADD(HOUR, 1, @fDev);

                        IF @m0 >= @Ahora OR (@fDev >= DATEADD(DAY, -30, @Ahora) AND @r < 0.6)
                            UPDATE #Unidad SET LibreDesde = @Nunca, EstadoFinal = 'Dañada' WHERE UnidadId = @u;
                        ELSE
                        BEGIN
                            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                            SET @externo = CASE WHEN @motor = 1 AND @r < 0.55 THEN 1 ELSE 0 END;
                            SET @m1 = DATEADD(MINUTE, FLOOR(@r * 300), DATEADD(DAY, 2 + FLOOR(@r * 11), @m0));
                            IF DATEDIFF(DAY, '19000107', @m1) % 7 = 0 SET @m1 = DATEADD(DAY, 1, @m1);
                            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                            SET @garantia = CASE WHEN @externo = 1 AND @r < 0.15 THEN 1 ELSE 0 END;
                            SET @baja = CASE WHEN @r > 0.9 THEN 1 ELSE 0 END;
                            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                            SET @mat = CASE WHEN @garantia = 1 OR @baja = 1 THEN 0
                                            WHEN @externo = 1 THEN ROUND((80 + @r * 570) / 5, 0) * 5
                                            ELSE ROUND((45 + @r * 405) / 5, 0) * 5 END;
                            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                            SET @mano = CASE WHEN @externo = 0 THEN NULL
                                             WHEN @garantia = 1 THEN 0
                                             WHEN @baja = 1 THEN ROUND((100 + @r * 100) / 5, 0) * 5     -- solo el diagnóstico
                                             ELSE ROUND((150 + @r * 450) / 5, 0) * 5 END;
                            SELECT @r = r FROM #Azar WHERE i = @k; SET @k += 1;
                            SELECT @usr = UsuarioId FROM @Aprob WHERE Id = 1 + FLOOR(@r * @nAprob);
                            IF @externo = 1 AND @garantia = 0 SET @folio += 1 + FLOOR(@r * 40);

                            INSERT INTO Mantenimiento (HerramientaId, UnidadId, FechaInicio, TipoMantenimiento, Descripcion, TipoServicio,
                                   EmpleadoId, ProveedorId, RegistradoPorId, PrestamoDetalleId,
                                   FechaFin, NotasCierre, CostoMateriales, CostoManoObra, Costo, EnGarantia, FolioFactura, Resultado, CerradoPorId)
                            SELECT @h, @u, @m0, 'Correctivo', 'Reportada dañada en devolución: ' + CAST(@nota AS VARCHAR(250)),
                                   CASE WHEN @externo = 1 THEN 'Externo' ELSE 'Interno' END,
                                   CASE WHEN @externo = 0 THEN (SELECT EmpleadoId FROM @Tecnico WHERE Id = 1 + FLOOR(@r * @nTec)) END,
                                   CASE WHEN @externo = 1 THEN CASE WHEN @r < 0.7 OR @ProvOtro IS NULL THEN @ProvRep ELSE @ProvOtro END END,
                                   @usr, @pdid,
                                   CASE WHEN @m1 < @Ahora THEN @m1 END,
                                   CASE WHEN @m1 < @Ahora THEN CASE WHEN @baja = 1 THEN 'La reparación cuesta más que una unidad nueva; se da de baja'
                                                                    WHEN @garantia = 1 THEN @arreglo + ' (cubierto por garantía)'
                                                                    ELSE @arreglo END END,
                                   CASE WHEN @m1 < @Ahora THEN @mat END,
                                   CASE WHEN @m1 < @Ahora THEN @mano END,
                                   CASE WHEN @m1 < @Ahora THEN @mat + ISNULL(@mano, 0) END,
                                   CASE WHEN @m1 < @Ahora THEN @garantia ELSE 0 END,
                                   CASE WHEN @m1 < @Ahora AND @externo = 1 AND @garantia = 0 THEN 'FAC-' + CAST(@folio AS VARCHAR(10)) END,
                                   CASE WHEN @m1 < @Ahora THEN CASE WHEN @baja = 1 THEN 'Baja' ELSE 'Reparada' END END,
                                   CASE WHEN @m1 < @Ahora THEN @usr END;

                            UPDATE #Unidad
                            SET    LibreDesde  = CASE WHEN @m1 >= @Ahora OR @baja = 1 THEN @Nunca ELSE @m1 END,
                                   Retirada    = CASE WHEN @m1 < @Ahora AND @baja = 1 THEN 1 ELSE 0 END,
                                   EstadoFinal = CASE WHEN @m1 >= @Ahora THEN 'En Mantenimiento'
                                                      WHEN @baja = 1     THEN 'Baja'
                                                      ELSE                    'Disponible' END,
                                   FechaBaja   = CASE WHEN @m1 < @Ahora AND @baja = 1 THEN @m1 END
                            WHERE  UnidadId = @u;
                        END
                    END
                END
                FETCH NEXT FROM item INTO @u;
            END
            CLOSE item; DEALLOCATE item;

            -- Estado del préstamo, igual que sp_Prestamo_CerrarSiCompleto y sp_Prestamo_MarcarVencidos
            UPDATE p
            SET    Estado      = CASE WHEN x.Pendientes = 0 THEN 'Cerrado'
                                      WHEN p.FechaDevolucionEsperada < @Ahora THEN 'Vencido'
                                      ELSE 'Activo' END,
                   FechaCierre = CASE WHEN x.Pendientes = 0 THEN x.UltimaDevolucion END
            FROM   Prestamo p
            CROSS  APPLY (SELECT SUM(CASE WHEN FechaDevuelta IS NULL THEN 1 ELSE 0 END) AS Pendientes,
                                 MAX(FechaDevuelta) AS UltimaDevolucion
                          FROM PrestamoDetalle WHERE PrestamoId = p.PrestamoId) x
            WHERE  p.PrestamoId = @pid;
        END
    END

    SET @dia = DATEADD(DAY, 1, @dia);
END

-- ════════════════════════════════════════════════════════════
-- 5. Estado final de cada unidad = su último movimiento
-- ════════════════════════════════════════════════════════════
UPDATE hu
SET    Estado    = u.EstadoFinal,
       FechaBaja = CASE WHEN u.EstadoFinal = 'Baja' THEN u.FechaBaja ELSE hu.FechaBaja END
FROM   HerramientaUnidad hu
INNER  JOIN #Unidad u ON u.UnidadId = hu.UnidadId;

-- ════════════════════════════════════════════════════════════
-- 6. Para la demostración: al menos 2 unidades dañadas pendientes de reparar
--    (la bandeja "Pendientes" de Mantenimiento y el indicador "Dañadas" del dashboard).
--    Se toman las devoluciones en buen estado más recientes de unidades simuladas
--    que no tuvieron movimientos después, y se registran como dañadas.
-- ════════════════════════════════════════════════════════════
DECLARE @Faltan INT = 2 - (SELECT COUNT(*) FROM HerramientaUnidad WHERE Estado = 'Dañada');
IF @Faltan > 0
BEGIN
    DECLARE @Pendientes TABLE (PrestamoDetalleId INT, UnidadId INT, Motor BIT);
    INSERT INTO @Pendientes
    SELECT TOP (@Faltan) d.PrestamoDetalleId, d.UnidadId, x.Motor
    FROM   PrestamoDetalle d
    INNER  JOIN #Unidad x           ON x.UnidadId = d.UnidadId
    INNER  JOIN HerramientaUnidad u ON u.UnidadId = d.UnidadId AND u.Estado = 'Disponible'
    WHERE  d.EstadoDevolucion = 'Bueno'
      AND  d.FechaDevuelta >= DATEADD(DAY, -20, @Ahora)
      AND  NOT EXISTS (SELECT 1 FROM PrestamoDetalle d2 INNER JOIN Prestamo p2 ON p2.PrestamoId = d2.PrestamoId
                       WHERE d2.UnidadId = d.UnidadId AND p2.FechaPrestamo > d.FechaDevuelta)
      AND  NOT EXISTS (SELECT 1 FROM Mantenimiento m WHERE m.UnidadId = d.UnidadId AND m.FechaInicio > d.FechaDevuelta)
    ORDER  BY d.FechaDevuelta DESC;

    UPDATE d
    SET    EstadoDevolucion      = 'Dañado',
           ObservacionDevolucion = CASE WHEN p.Motor = 1 THEN N'Cable de alimentación pelado' ELSE N'Mango quebrado' END
    FROM   PrestamoDetalle d INNER JOIN @Pendientes p ON p.PrestamoDetalleId = d.PrestamoDetalleId;

    UPDATE u SET Estado = 'Dañada'
    FROM   HerramientaUnidad u INNER JOIN @Pendientes p ON p.UnidadId = u.UnidadId;
END

EXEC sys.sp_addextendedproperty @name = N'PROMACO_DatosDemo', @value = N'Datos de demostración cargados con Database/demo/datos_demo.sql';

COMMIT TRANSACTION;

-- Resumen
SELECT (SELECT COUNT(*) FROM Prestamo)                                           AS Prestamos,
       (SELECT COUNT(*) FROM Prestamo WHERE Estado = 'Activo')                   AS Activos,
       (SELECT COUNT(*) FROM Prestamo WHERE Estado = 'Vencido')                  AS Vencidos,
       (SELECT COUNT(*) FROM PrestamoDetalle WHERE EstadoDevolucion = 'Dañado')  AS DevueltasDañadas,
       (SELECT COUNT(*) FROM PrestamoDetalle WHERE EstadoDevolucion = 'Perdido') AS Perdidas,
       (SELECT COUNT(*) FROM Mantenimiento)                                      AS Mantenimientos,
       (SELECT SUM(Costo) FROM Mantenimiento)                                    AS CostoTotalQ;
GO

DROP TABLE IF EXISTS #Azar, #Unidad, #Herramienta, #Prestatario;
GO
SET NOEXEC OFF;
GO
