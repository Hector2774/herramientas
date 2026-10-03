-- ============================================================
-- 004 · Datos de prueba: categorías, marcas, ubicaciones y
--       20 herramientas (con sus unidades físicas).
--
-- Requiere 003 (código automático). Solo inserta lo que no exista.
--
-- Ejecutar con:  sqlcmd -S localhost -E -d PROMACO_Herramientas -b -I -f 65001 -i 004_seed_herramientas.sql
-- ============================================================
SET XACT_ABORT ON;
SET NOCOUNT ON;
GO
BEGIN TRANSACTION;
GO

-- ── Categorías ───────────────────────────────────────────────
INSERT INTO CategoriaHerramienta (Nombre, Descripcion)
SELECT v.Nombre, v.Descripcion
FROM (VALUES
    ('Eléctricas',      'Herramientas eléctricas de cable'),
    ('Inalámbricas',    'Herramientas a batería'),
    ('Manuales',        'Herramientas de mano'),
    ('Medición',        'Instrumentos de medición y nivelación'),
    ('Neumáticas',      'Herramientas de aire comprimido'),
    ('Jardinería',      'Herramientas para exteriores y jardín'),
    ('Seguridad',       'Equipo de protección y seguridad')
) v (Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM CategoriaHerramienta c WHERE c.Nombre = v.Nombre);

-- ── Marcas ───────────────────────────────────────────────────
INSERT INTO Marca (NombreMarca, Descripcion)
SELECT v.Nombre, v.Descripcion
FROM (VALUES
    ('DeWalt',   'Herramientas eléctricas profesionales'),
    ('Makita',   'Herramientas eléctricas e inalámbricas'),
    ('Bosch',    'Herramientas eléctricas y de medición'),
    ('Truper',   'Herramientas manuales y de jardín'),
    ('Stanley',  'Herramientas manuales y de medición'),
    ('Milwaukee','Herramientas profesionales a batería'),
    ('Pretul',   'Línea económica de herramientas'),
    ('3M',       'Equipo de seguridad')
) v (Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM Marca m WHERE m.NombreMarca = v.Nombre);

-- ── Ubicaciones ──────────────────────────────────────────────
INSERT INTO Ubicacion (Nombre, Descripcion)
SELECT v.Nombre, v.Descripcion
FROM (VALUES
    ('Almacén A - Estante 1', 'Herramientas eléctricas'),
    ('Almacén A - Estante 2', 'Herramientas manuales'),
    ('Almacén B - Gabinete',  'Instrumentos de medición'),
    ('Bodega exterior',       'Jardinería y neumáticas'),
    ('Caseta de seguridad',   'Equipo de protección')
) v (Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM Ubicacion u WHERE u.Nombre = v.Nombre);
GO

-- ── Herramientas ─────────────────────────────────────────────
DECLARE @h TABLE (
    Orden INT IDENTITY, Nombre VARCHAR(120), Caracteristicas VARCHAR(400),
    Categoria VARCHAR(120), Marca VARCHAR(100), Ubicacion VARCHAR(120), Stock INT);

INSERT INTO @h (Nombre, Caracteristicas, Categoria, Marca, Ubicacion, Stock) VALUES
('Taladro percutor 1/2"',          '750 W, velocidad variable, reversible',        'Eléctricas',   'DeWalt',    'Almacén A - Estante 1', 4),
('Esmeriladora angular 4 1/2"',    '850 W, 11,000 RPM',                            'Eléctricas',   'Makita',    'Almacén A - Estante 1', 3),
('Sierra circular 7 1/4"',         '1,800 W, corte a 45°',                         'Eléctricas',   'Bosch',     'Almacén A - Estante 1', 2),
('Rotomartillo SDS Plus',          '800 W, 3 modos de trabajo',                    'Eléctricas',   'Bosch',     'Almacén A - Estante 1', 2),
('Lijadora orbital',               '300 W, base de 1/4 de hoja',                   'Eléctricas',   'DeWalt',    'Almacén A - Estante 1', 3),
('Atornillador de impacto 20V',    'Incluye 2 baterías y cargador',                'Inalámbricas', 'Milwaukee', 'Almacén A - Estante 1', 3),
('Taladro inalámbrico 18V',        'Mandril 1/2", 2 velocidades',                  'Inalámbricas', 'Makita',    'Almacén A - Estante 1', 4),
('Sierra caladora inalámbrica',    '20V, carrera de 1"',                           'Inalámbricas', 'DeWalt',    'Almacén A - Estante 1', 2),
('Juego de desarmadores 10 pzas',  'Punta plana y Phillips, mango ergonómico',     'Manuales',     'Truper',    'Almacén A - Estante 2', 6),
('Martillo de uña 16 oz',          'Mango de fibra de vidrio',                     'Manuales',     'Stanley',   'Almacén A - Estante 2', 8),
('Llave stillson 14"',             'Acero forjado',                                'Manuales',     'Truper',    'Almacén A - Estante 2', 4),
('Juego de llaves combinadas',     '12 piezas, milimétricas',                      'Manuales',     'Pretul',    'Almacén A - Estante 2', 5),
('Pinza de presión 10"',           'Mordaza curva',                                'Manuales',     'Stanley',   'Almacén A - Estante 2', 6),
('Flexómetro 8 m',                 'Cinta de 1", con freno',                       'Medición',     'Stanley',   'Almacén B - Gabinete',  10),
('Nivel láser de líneas',          'Autonivelante, alcance 15 m',                  'Medición',     'Bosch',     'Almacén B - Gabinete',  2),
('Nivel de aluminio 24"',          '3 burbujas',                                   'Medición',     'Truper',    'Almacén B - Gabinete',  5),
('Pistola de impacto neumática',   'Cuadro de 1/2", 90 PSI',                       'Neumáticas',   'Truper',    'Bodega exterior',       2),
('Podadora de césped',             'Motor a gasolina 4 tiempos, 20"',              'Jardinería',   'Truper',    'Bodega exterior',       1),
('Tijera para podar',              'Hoja de acero al carbono 8"',                  'Jardinería',   'Pretul',    'Bodega exterior',       4),
('Careta para soldar',             'Oscurecimiento automático',                    'Seguridad',    '3M',        'Caseta de seguridad',   3);

DECLARE @i INT = 1, @n INT = (SELECT COUNT(*) FROM @h);
DECLARE @Nombre VARCHAR(120), @Car VARCHAR(400), @CatId INT, @MarId INT, @UbiId INT, @Stock INT;

WHILE @i <= @n
BEGIN
    SELECT @Nombre = h.Nombre, @Car = h.Caracteristicas, @Stock = h.Stock,
           @CatId  = (SELECT CategoriaId FROM CategoriaHerramienta WHERE Nombre = h.Categoria),
           @MarId  = (SELECT MarcaId     FROM Marca                WHERE NombreMarca = h.Marca),
           @UbiId  = (SELECT UbicacionId FROM Ubicacion            WHERE Nombre = h.Ubicacion)
    FROM @h h WHERE h.Orden = @i;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE Nombre = @Nombre)
        EXEC sp_Herramienta_Insertar @Nombre, @Car, @CatId, @MarId, @UbiId, @Stock;

    SET @i += 1;
END
GO

COMMIT TRANSACTION;
GO
