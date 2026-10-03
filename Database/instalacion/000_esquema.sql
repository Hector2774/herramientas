-- ============================================================
-- PROMACO · Sistema de Control de Herramientas
-- 000 · Esquema completo de la base de datos
--
-- Crea todas las tablas, restricciones, índices, vistas, funciones y
-- procedimientos almacenados en su estado actual. Para una instalación
-- nueva se ejecuta este script y luego 001_datos_iniciales.sql.
-- (Database/migraciones/ guarda la historia de cambios 002–018; una
--  instalación nueva NO necesita ejecutarlos: ya están incluidos aquí.)
--
-- Pasos:
--   1. Crear la base vacía:
--        CREATE DATABASE PROMACO_Herramientas COLLATE Modern_Spanish_CI_AS;
--   2. Ejecutar este script SOBRE esa base:
--        sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i 000_esquema.sql
--      (en SSMS: seleccionar la base PROMACO_Herramientas y ejecutar)
--   3. Ejecutar 001_datos_iniciales.sql de la misma forma.
--
-- Generado desde la base de desarrollo con SMO (mismo motor que
-- "Generar scripts" de SSMS), en orden de dependencias.
-- ============================================================

-- Protección: no crear el esquema en una base del sistema por error
IF DB_NAME() IN (N'master', N'model', N'msdb', N'tempdb')
BEGIN
    RAISERROR(N'Seleccione la base PROMACO_Herramientas antes de ejecutar este script (ver pasos arriba).', 16, 1);
    SET NOEXEC ON;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriaHerramienta](
	[CategoriaId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](120) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Descripcion] [varchar](400) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK_CategoriaHerramienta] PRIMARY KEY CLUSTERED 
(
	[CategoriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Categoria_Nombre] ON [dbo].[CategoriaHerramienta]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departamento](
	[DepartamentoId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](120) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Activo] [tinyint] NOT NULL,
 CONSTRAINT [PK_Departamento] PRIMARY KEY CLUSTERED 
(
	[DepartamentoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Departamento] ADD  CONSTRAINT [DF_Departamento_Activo]  DEFAULT ((1)) FOR [Activo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[EmpleadoId] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Nombre] [nvarchar](120) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Activo] [bit] NOT NULL,
	[DepartamentoId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmpleadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Empleado_DepartamentoId] ON [dbo].[Empleado]
(
	[DepartamentoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Empleado] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK_Empleado_Departamento] FOREIGN KEY([DepartamentoId])
REFERENCES [dbo].[Departamento] ([DepartamentoId])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Departamento]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marca](
	[MarcaId] [int] IDENTITY(1,1) NOT NULL,
	[NombreMarca] [varchar](100) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Descripcion] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[MarcaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Marca_NombreMarca] ON [dbo].[Marca]
(
	[NombreMarca] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ubicacion](
	[UbicacionId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](120) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Descripcion] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK_Ubicacion] PRIMARY KEY CLUSTERED 
(
	[UbicacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Ubicacion_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Herramienta](
	[HerramientaId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](120) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Caracteristicas] [nvarchar](400) COLLATE Modern_Spanish_CI_AS NULL,
	[Activa] [bit] NOT NULL,
	[CategoriaId] [int] NULL,
	[UbicacionId] [int] NULL,
	[MarcaId] [int] NULL,
	[PrestamoHabilitado] [bit] NOT NULL,
	[Codigo]  AS (CONVERT([varchar](20),'HER-'+right('0000'+CONVERT([varchar](10),[HerramientaId]),case when [HerramientaId]>(9999) then len(CONVERT([varchar](10),[HerramientaId])) else (4) end))) PERSISTED NOT NULL,
	[FotoNombre] [nvarchar](200) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[HerramientaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Herramienta_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Herramienta_CategoriaId] ON [dbo].[Herramienta]
(
	[CategoriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Herramienta_MarcaId] ON [dbo].[Herramienta]
(
	[MarcaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Herramienta_UbicacionId] ON [dbo].[Herramienta]
(
	[UbicacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Herramienta] ADD  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[Herramienta] ADD  CONSTRAINT [DF_Herramienta_PrestamoHabilitado]  DEFAULT ((1)) FOR [PrestamoHabilitado]
GO
ALTER TABLE [dbo].[Herramienta]  WITH CHECK ADD  CONSTRAINT [FK_Herramienta_Categoria] FOREIGN KEY([CategoriaId])
REFERENCES [dbo].[CategoriaHerramienta] ([CategoriaId])
GO
ALTER TABLE [dbo].[Herramienta] CHECK CONSTRAINT [FK_Herramienta_Categoria]
GO
ALTER TABLE [dbo].[Herramienta]  WITH CHECK ADD  CONSTRAINT [FK_Herramienta_Marca] FOREIGN KEY([MarcaId])
REFERENCES [dbo].[Marca] ([MarcaId])
GO
ALTER TABLE [dbo].[Herramienta] CHECK CONSTRAINT [FK_Herramienta_Marca]
GO
ALTER TABLE [dbo].[Herramienta]  WITH CHECK ADD  CONSTRAINT [FK_Herramienta_Ubicacion] FOREIGN KEY([UbicacionId])
REFERENCES [dbo].[Ubicacion] ([UbicacionId])
GO
ALTER TABLE [dbo].[Herramienta] CHECK CONSTRAINT [FK_Herramienta_Ubicacion]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HerramientaUnidad](
	[UnidadId] [int] IDENTITY(1,1) NOT NULL,
	[HerramientaId] [int] NOT NULL,
	[Numero] [int] NOT NULL,
	[Estado] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[FechaAlta] [datetime2](7) NOT NULL,
	[FechaBaja] [datetime2](7) NULL,
	[Observaciones] [nvarchar](250) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK_HerramientaUnidad] PRIMARY KEY CLUSTERED 
(
	[UnidadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Unidad_Herramienta] UNIQUE NONCLUSTERED 
(
	[UnidadId] ASC,
	[HerramientaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Unidad_Numero] UNIQUE NONCLUSTERED 
(
	[HerramientaId] ASC,
	[Numero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_Unidad_Estado] ON [dbo].[HerramientaUnidad]
(
	[Estado] ASC
)
INCLUDE([HerramientaId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_Unidad_Herramienta_Estado] ON [dbo].[HerramientaUnidad]
(
	[HerramientaId] ASC,
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HerramientaUnidad] ADD  CONSTRAINT [DF_Unidad_Estado]  DEFAULT ('Disponible') FOR [Estado]
GO
ALTER TABLE [dbo].[HerramientaUnidad] ADD  CONSTRAINT [DF_Unidad_FechaAlta]  DEFAULT (sysdatetime()) FOR [FechaAlta]
GO
ALTER TABLE [dbo].[HerramientaUnidad]  WITH CHECK ADD  CONSTRAINT [FK_Unidad_Herramienta] FOREIGN KEY([HerramientaId])
REFERENCES [dbo].[Herramienta] ([HerramientaId])
GO
ALTER TABLE [dbo].[HerramientaUnidad] CHECK CONSTRAINT [FK_Unidad_Herramienta]
GO
ALTER TABLE [dbo].[HerramientaUnidad]  WITH CHECK ADD  CONSTRAINT [CK_Unidad_Estado] CHECK  (([Estado]='Baja' OR [Estado]='Perdida' OR [Estado]='Dañada' OR [Estado]='En Mantenimiento' OR [Estado]='Prestada' OR [Estado]='Disponible'))
GO
ALTER TABLE [dbo].[HerramientaUnidad] CHECK CONSTRAINT [CK_Unidad_Estado]
GO
ALTER TABLE [dbo].[HerramientaUnidad]  WITH CHECK ADD  CONSTRAINT [CK_Unidad_FechaBaja] CHECK  (([Estado]='Baja' AND [FechaBaja] IS NOT NULL OR [Estado]<>'Baja' AND [FechaBaja] IS NULL))
GO
ALTER TABLE [dbo].[HerramientaUnidad] CHECK CONSTRAINT [CK_Unidad_FechaBaja]
GO
ALTER TABLE [dbo].[HerramientaUnidad]  WITH CHECK ADD  CONSTRAINT [CK_Unidad_Numero] CHECK  (([Numero]>(0)))
GO
ALTER TABLE [dbo].[HerramientaUnidad] CHECK CONSTRAINT [CK_Unidad_Numero]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prestamo](
	[PrestamoId] [int] IDENTITY(1,1) NOT NULL,
	[EmpleadoId] [int] NOT NULL,
	[FechaPrestamo] [datetime2](7) NOT NULL,
	[FechaCierre] [datetime2](7) NULL,
	[Observaciones] [nvarchar](400) COLLATE Modern_Spanish_CI_AS NULL,
	[AprobadoPorId] [int] NULL,
	[FechaDevolucionEsperada] [datetime] NULL,
	[Estado] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrestamoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Prestamo_AprobadoPorId] ON [dbo].[Prestamo]
(
	[AprobadoPorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Prestamo_EmpleadoId] ON [dbo].[Prestamo]
(
	[EmpleadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_Prestamo_Estado] ON [dbo].[Prestamo]
(
	[Estado] ASC
)
INCLUDE([FechaDevolucionEsperada],[EmpleadoId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Prestamo] ADD  DEFAULT (sysdatetime()) FOR [FechaPrestamo]
GO
ALTER TABLE [dbo].[Prestamo] ADD  CONSTRAINT [DF_Prestamo_Estado]  DEFAULT ('Activo') FOR [Estado]
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD  CONSTRAINT [FK_Prestamo_Aprobador] FOREIGN KEY([AprobadoPorId])
REFERENCES [dbo].[Empleado] ([EmpleadoId])
GO
ALTER TABLE [dbo].[Prestamo] CHECK CONSTRAINT [FK_Prestamo_Aprobador]
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD  CONSTRAINT [FK_Prestamo_Empleado] FOREIGN KEY([EmpleadoId])
REFERENCES [dbo].[Empleado] ([EmpleadoId])
GO
ALTER TABLE [dbo].[Prestamo] CHECK CONSTRAINT [FK_Prestamo_Empleado]
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD  CONSTRAINT [CK_Prestamo_Estado] CHECK  (([Estado]='Cerrado' OR [Estado]='Vencido' OR [Estado]='Activo'))
GO
ALTER TABLE [dbo].[Prestamo] CHECK CONSTRAINT [CK_Prestamo_Estado]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrestamoDetalle](
	[PrestamoDetalleId] [int] IDENTITY(1,1) NOT NULL,
	[PrestamoId] [int] NOT NULL,
	[HerramientaId] [int] NOT NULL,
	[FechaDevuelta] [datetime2](7) NULL,
	[ObservacionDevolucion] [nvarchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[EstadoDevolucion] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[UnidadId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrestamoDetalleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Detalle_Prestamo_Unidad] UNIQUE NONCLUSTERED 
(
	[PrestamoId] ASC,
	[UnidadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Detalle_Herramienta] ON [dbo].[PrestamoDetalle]
(
	[HerramientaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Detalle_Unidad] ON [dbo].[PrestamoDetalle]
(
	[UnidadId] ASC
)
INCLUDE([FechaDevuelta]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrestamoDetalle] ADD  CONSTRAINT [DF_PrestamoDetalle_Estado]  DEFAULT ('Pendiente') FOR [EstadoDevolucion]
GO
ALTER TABLE [dbo].[PrestamoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Herramienta] FOREIGN KEY([HerramientaId])
REFERENCES [dbo].[Herramienta] ([HerramientaId])
GO
ALTER TABLE [dbo].[PrestamoDetalle] CHECK CONSTRAINT [FK_Detalle_Herramienta]
GO
ALTER TABLE [dbo].[PrestamoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Prestamo] FOREIGN KEY([PrestamoId])
REFERENCES [dbo].[Prestamo] ([PrestamoId])
GO
ALTER TABLE [dbo].[PrestamoDetalle] CHECK CONSTRAINT [FK_Detalle_Prestamo]
GO
ALTER TABLE [dbo].[PrestamoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Unidad] FOREIGN KEY([UnidadId], [HerramientaId])
REFERENCES [dbo].[HerramientaUnidad] ([UnidadId], [HerramientaId])
GO
ALTER TABLE [dbo].[PrestamoDetalle] CHECK CONSTRAINT [FK_Detalle_Unidad]
GO
ALTER TABLE [dbo].[PrestamoDetalle]  WITH CHECK ADD  CONSTRAINT [CK_PrestamoDetalle_EstadoDevolucion] CHECK  (([EstadoDevolucion]='Perdido' OR [EstadoDevolucion]='Dañado' OR [EstadoDevolucion]='Bueno' OR [EstadoDevolucion]='Pendiente'))
GO
ALTER TABLE [dbo].[PrestamoDetalle] CHECK CONSTRAINT [CK_PrestamoDetalle_EstadoDevolucion]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[RolId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](30) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Descripcion] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[AdministraUsuarios] [bit] NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[RolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Rol_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Rol] ADD  CONSTRAINT [DF_Rol_AdministraUsuarios]  DEFAULT ((0)) FOR [AdministraUsuarios]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[UsuarioId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[Password] [nvarchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[Activo] [bit] NULL,
	[EmpleadoId] [int] NOT NULL,
	[PasswordHash] [varchar](256) COLLATE Modern_Spanish_CI_AS NULL,
	[DebeCambiarPassword] [bit] NOT NULL,
	[RolId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Usuario_Empleado] UNIQUE NONCLUSTERED 
(
	[EmpleadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Usuario_Username] ON [dbo].[Usuario]
(
	[Username] ASC
)
WHERE ([Username] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_DebeCambiarPassword]  DEFAULT ((0)) FOR [DebeCambiarPassword]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Empleado] FOREIGN KEY([EmpleadoId])
REFERENCES [dbo].[Empleado] ([EmpleadoId])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Empleado]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([RolId])
REFERENCES [dbo].[Rol] ([RolId])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedor](
	[ProveedorId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](120) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Telefono] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[Descripcion] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Proveedor] PRIMARY KEY CLUSTERED 
(
	[ProveedorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Proveedor_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Proveedor] ADD  CONSTRAINT [DF_Proveedor_Activo]  DEFAULT ((1)) FOR [Activo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mantenimiento](
	[MantenimientoId] [int] IDENTITY(1,1) NOT NULL,
	[HerramientaId] [int] NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NULL,
	[TipoMantenimiento] [varchar](50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Descripcion] [varchar](400) COLLATE Modern_Spanish_CI_AS NULL,
	[RealizadoPor] [varchar](120) COLLATE Modern_Spanish_CI_AS NULL,
	[Costo] [decimal](12, 2) NULL,
	[UnidadId] [int] NOT NULL,
	[TipoServicio] [varchar](10) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[EmpleadoId] [int] NULL,
	[ProveedorId] [int] NULL,
	[RegistradoPorId] [int] NULL,
	[CerradoPorId] [int] NULL,
	[CostoMateriales] [decimal](10, 2) NULL,
	[CostoManoObra] [decimal](10, 2) NULL,
	[EnGarantia] [bit] NOT NULL,
	[FolioFactura] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[NotasCierre] [varchar](400) COLLATE Modern_Spanish_CI_AS NULL,
	[Resultado] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[PrestamoDetalleId] [int] NULL,
 CONSTRAINT [PK_Mantenimiento] PRIMARY KEY CLUSTERED 
(
	[MantenimientoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Mantenimiento_HerramientaId] ON [dbo].[Mantenimiento]
(
	[HerramientaId] ASC
)
INCLUDE([FechaFin]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Mantenimiento_UnidadAbierta] ON [dbo].[Mantenimiento]
(
	[UnidadId] ASC
)
WHERE ([FechaFin] IS NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Mantenimiento] ADD  CONSTRAINT [DF_Mantenimiento_TipoServicio]  DEFAULT ('Interno') FOR [TipoServicio]
GO
ALTER TABLE [dbo].[Mantenimiento] ADD  CONSTRAINT [DF_Mantenimiento_EnGarantia]  DEFAULT ((0)) FOR [EnGarantia]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [FK_Mantenimiento_CerradoPor] FOREIGN KEY([CerradoPorId])
REFERENCES [dbo].[Usuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [FK_Mantenimiento_CerradoPor]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [FK_Mantenimiento_Empleado] FOREIGN KEY([EmpleadoId])
REFERENCES [dbo].[Empleado] ([EmpleadoId])
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [FK_Mantenimiento_Empleado]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [FK_Mantenimiento_Herramienta] FOREIGN KEY([HerramientaId])
REFERENCES [dbo].[Herramienta] ([HerramientaId])
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [FK_Mantenimiento_Herramienta]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [FK_Mantenimiento_PrestamoDetalle] FOREIGN KEY([PrestamoDetalleId])
REFERENCES [dbo].[PrestamoDetalle] ([PrestamoDetalleId])
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [FK_Mantenimiento_PrestamoDetalle]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [FK_Mantenimiento_Proveedor] FOREIGN KEY([ProveedorId])
REFERENCES [dbo].[Proveedor] ([ProveedorId])
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [FK_Mantenimiento_Proveedor]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [FK_Mantenimiento_RegistradoPor] FOREIGN KEY([RegistradoPorId])
REFERENCES [dbo].[Usuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [FK_Mantenimiento_RegistradoPor]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [FK_Mantenimiento_Unidad] FOREIGN KEY([UnidadId], [HerramientaId])
REFERENCES [dbo].[HerramientaUnidad] ([UnidadId], [HerramientaId])
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [FK_Mantenimiento_Unidad]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_Costo] CHECK  (([Costo] IS NULL OR [Costo]>=(0)))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_Costo]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_CostoManoObra] CHECK  (([CostoManoObra] IS NULL OR [CostoManoObra]>=(0)))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_CostoManoObra]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_CostoMateriales] CHECK  (([CostoMateriales] IS NULL OR [CostoMateriales]>=(0)))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_CostoMateriales]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_Fechas] CHECK  (([FechaFin] IS NULL OR [FechaFin]>=[FechaInicio]))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_Fechas]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_Responsable] CHECK  (([TipoServicio]='Interno' AND [ProveedorId] IS NULL OR [TipoServicio]='Externo' AND [EmpleadoId] IS NULL))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_Responsable]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_Resultado] CHECK  (([Resultado] IS NULL OR ([Resultado]='Baja' OR [Resultado]='Reparada')))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_Resultado]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_Tipo] CHECK  (([TipoMantenimiento]='Calibración' OR [TipoMantenimiento]='Correctivo' OR [TipoMantenimiento]='Preventivo'))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_Tipo]
GO
ALTER TABLE [dbo].[Mantenimiento]  WITH CHECK ADD  CONSTRAINT [CK_Mantenimiento_TipoServicio] CHECK  (([TipoServicio]='Externo' OR [TipoServicio]='Interno'))
GO
ALTER TABLE [dbo].[Mantenimiento] CHECK CONSTRAINT [CK_Mantenimiento_TipoServicio]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SincronizacionEmpleados](
	[SincronizacionId] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Nuevos] [int] NOT NULL,
	[Actualizados] [int] NOT NULL,
	[TotalActivos] [int] NOT NULL,
 CONSTRAINT [PK_SincronizacionEmpleados] PRIMARY KEY CLUSTERED 
(
	[SincronizacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SincronizacionEmpleados] ADD  CONSTRAINT [DF_SincronizacionEmpleados_Fecha]  DEFAULT (sysdatetime()) FOR [Fecha]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- StockTotal = unidades que siguen siendo de la empresa (no perdidas ni dadas de baja)
CREATE   VIEW vw_HerramientaStock
AS
SELECT h.HerramientaId,
       SUM(CASE WHEN u.Estado NOT IN ('Baja', 'Perdida') THEN 1 ELSE 0 END) AS StockTotal,
       SUM(CASE WHEN u.Estado = 'Disponible' THEN 1 ELSE 0 END) AS StockDisponible,
       SUM(CASE WHEN u.Estado = 'Prestada' THEN 1 ELSE 0 END) AS StockPrestado,
       SUM(CASE WHEN u.Estado = 'En Mantenimiento' THEN 1 ELSE 0 END) AS StockMantenimiento,
       SUM(CASE WHEN u.Estado = 'Dañada' THEN 1 ELSE 0 END) AS StockDañado,
       SUM(CASE WHEN u.Estado = 'Perdida' THEN 1 ELSE 0 END) AS StockPerdido,
       SUM(CASE WHEN u.Estado = 'Baja' THEN 1 ELSE 0 END) AS StockBaja
FROM   Herramienta h
LEFT   JOIN HerramientaUnidad u ON u.HerramientaId = h.HerramientaId
GROUP  BY h.HerramientaId;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- VISTAS
-- ============================================================
CREATE   VIEW vw_HerramientaUnidad
AS
SELECT u.UnidadId,
       u.HerramientaId,
       u.Numero,
       h.Codigo + N'-' + CASE WHEN u.Numero < 100
                              THEN RIGHT('0' + CAST(u.Numero AS VARCHAR(10)), 2)
                              ELSE CAST(u.Numero AS VARCHAR(10)) END AS CodigoUnidad,
       u.Estado,
       u.FechaAlta,
       u.FechaBaja,
       u.Observaciones
FROM   HerramientaUnidad u
INNER  JOIN Herramienta  h ON h.HerramientaId = u.HerramientaId;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Último reporte de daño de la unidad que todavía no se atiende: la devolución "Dañado"
-- posterior al inicio de su último mantenimiento. Solo aplica a unidades en estado Dañada.
CREATE   FUNCTION fn_UnidadReporteDanio (@UnidadId INT)
RETURNS TABLE
AS
RETURN
    SELECT TOP (1)
           pd.PrestamoDetalleId,
           pd.FechaDevuelta         AS FechaReporte,
           e.Nombre                 AS ReportadoPor,
           pd.ObservacionDevolucion AS Nota
    FROM   PrestamoDetalle pd
    INNER  JOIN Prestamo p ON p.PrestamoId = pd.PrestamoId
    INNER  JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
    WHERE  pd.UnidadId         = @UnidadId
      AND  pd.EstadoDevolucion = 'Dañado'
      AND  pd.FechaDevuelta    > ISNULL((SELECT MAX(m.FechaInicio) FROM Mantenimiento m
                                         WHERE  m.UnidadId = @UnidadId), '19000101')
    ORDER  BY pd.FechaDevuelta DESC;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 6. Actualizar categoría
CREATE   PROCEDURE sp_Categoria_Actualizar
    @CategoriaId INT,
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE CategoriaId = @CategoriaId)
    BEGIN
        RAISERROR('La categoría especificada no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE Nombre = @Nombre AND CategoriaId <> @CategoriaId)
    BEGIN
        RAISERROR('Ya existe otra categoría con ese nombre.', 16, 1);
        RETURN;
    END

    UPDATE CategoriaHerramienta
    SET    Nombre      = @Nombre,
           Descripcion = @Descripcion
    WHERE  CategoriaId = @CategoriaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Categoria_Eliminar
    @CategoriaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Herramienta SET CategoriaId = NULL WHERE CategoriaId = @CategoriaId;
        DELETE FROM CategoriaHerramienta WHERE CategoriaId = @CategoriaId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 5. Insertar categoría
CREATE   PROCEDURE sp_Categoria_Insertar
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM CategoriaHerramienta WHERE Nombre = @Nombre)
    BEGIN
        RAISERROR('Ya existe una categoría con ese nombre.', 16, 1);
        RETURN;
    END

    INSERT INTO CategoriaHerramienta (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);

    SELECT SCOPE_IDENTITY() AS CategoriaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Categorías ───────────────────────────────────────────────
CREATE   PROCEDURE sp_Categoria_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CategoriaId,
           c.Nombre,
           c.Descripcion,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.CategoriaId = c.CategoriaId AND h.Activa = 1) AS Herramientas
    FROM   CategoriaHerramienta c
    ORDER  BY c.Nombre ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Préstamos registrados y devoluciones por día. Una "devolución" es un préstamo con
-- unidades devueltas ese día (una devolución parcial cuenta una vez por día).
CREATE   PROCEDURE sp_Dashboard_ActividadMensual
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    ;WITH Dias AS (
        SELECT DATEADD(DAY, -n, @Hoy) AS Fecha
        FROM  (SELECT TOP 30 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS n FROM sys.all_objects) t
    ),
    Pres AS (
        SELECT CAST(FechaPrestamo AS DATE) AS Fecha, COUNT(*) AS Prestamos
        FROM   Prestamo
        WHERE  FechaPrestamo >= DATEADD(DAY, -29, @Hoy)
        GROUP  BY CAST(FechaPrestamo AS DATE)
    ),
    Dev AS (
        SELECT Fecha, COUNT(*) AS Devoluciones
        FROM  (SELECT DISTINCT PrestamoId, CAST(FechaDevuelta AS DATE) AS Fecha
               FROM   PrestamoDetalle
               WHERE  FechaDevuelta >= DATEADD(DAY, -29, @Hoy)) x
        GROUP  BY Fecha
    )
    SELECT d.Fecha,
           ISNULL(Pres.Prestamos, 0)   AS Prestamos,
           ISNULL(Dev.Devoluciones, 0) AS Devoluciones
    FROM   Dias d
    LEFT   JOIN Pres ON Pres.Fecha = d.Fecha
    LEFT   JOIN Dev  ON Dev.Fecha  = d.Fecha
    ORDER  BY d.Fecha;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Préstamos registrados hoy y devoluciones de hoy (una por préstamo)
CREATE   PROCEDURE sp_Dashboard_ActividadReciente
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    SELECT TOP 8 Tipo, PrestamoId, CodigoEmpleado, Empleado, Herramientas, Fecha
    FROM (
        SELECT 'Préstamo' AS Tipo,
               p.PrestamoId,
               e.Codigo AS CodigoEmpleado,
               e.Nombre AS Empleado,
               (SELECT STRING_AGG(h.Nombre, N', ') WITHIN GROUP (ORDER BY h.Nombre)
                FROM   PrestamoDetalle pd
                INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
                WHERE  pd.PrestamoId = p.PrestamoId) AS Herramientas,
               p.FechaPrestamo AS Fecha
        FROM   Prestamo p
        INNER  JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        WHERE  CAST(p.FechaPrestamo AS DATE) = @Hoy

        UNION ALL

        SELECT 'Devolución',
               p.PrestamoId,
               e.Codigo,
               e.Nombre,
               STRING_AGG(h.Nombre, N', ') WITHIN GROUP (ORDER BY h.Nombre),
               MAX(pd.FechaDevuelta)
        FROM   PrestamoDetalle pd
        INNER  JOIN Prestamo    p ON p.PrestamoId    = pd.PrestamoId
        INNER  JOIN Empleado    e ON e.EmpleadoId    = p.EmpleadoId
        INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
        WHERE  CAST(pd.FechaDevuelta AS DATE) = @Hoy
        GROUP  BY p.PrestamoId, e.Codigo, e.Nombre
    ) t
    ORDER  BY Fecha DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Unidades de herramientas activas (sin las dadas de baja ni perdidas).
-- Las prestadas se separan en "al día" y "vencidas" según su préstamo.
CREATE   PROCEDURE sp_Dashboard_DistribucionUnidades
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH U AS (
        SELECT u.UnidadId, u.Estado,
               CASE WHEN u.Estado = 'Prestada' AND EXISTS (
                        SELECT 1 FROM PrestamoDetalle pd
                        INNER  JOIN Prestamo p ON p.PrestamoId = pd.PrestamoId
                        WHERE  pd.UnidadId = u.UnidadId AND pd.FechaDevuelta IS NULL
                          AND  CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE))
                    THEN 1 ELSE 0 END AS Vencida
        FROM   HerramientaUnidad u
        INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
        WHERE  h.Activa = 1
          AND  u.Estado NOT IN ('Baja', 'Perdida')
    )
    SELECT SUM(CASE WHEN Estado = 'Disponible' THEN 1 ELSE 0 END)                AS Disponibles,
           SUM(CASE WHEN Estado = 'Prestada' AND Vencida = 0 THEN 1 ELSE 0 END)  AS Prestadas,
           SUM(CASE WHEN Estado = 'Prestada' AND Vencida = 1 THEN 1 ELSE 0 END)  AS Vencidas,
           SUM(CASE WHEN Estado = 'En Mantenimiento' THEN 1 ELSE 0 END)          AS EnMantenimiento,
           SUM(CASE WHEN Estado = 'Dañada' THEN 1 ELSE 0 END)                    AS Danadas
    FROM   U;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Herramientas activas que tienen unidades pero ninguna disponible
CREATE   PROCEDURE sp_Dashboard_HerramientasSinStock
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
           h.HerramientaId,
           h.Codigo,
           h.Nombre,
           s.StockTotal      AS Total,
           s.StockDisponible AS Disponibles
    FROM   Herramienta h
    INNER  JOIN vw_HerramientaStock s ON s.HerramientaId = h.HerramientaId
    WHERE  h.Activa = 1
      AND  s.StockTotal > 0
      AND  s.StockDisponible = 0
    ORDER  BY s.StockTotal DESC, h.Nombre;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Dashboard_PrestamosPorDepartamento
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 6
           ISNULL(d.Nombre, 'Sin departamento') AS Departamento,
           COUNT(*) AS Total
    FROM   Prestamo p
    INNER  JOIN Empleado     e ON e.EmpleadoId     = p.EmpleadoId
    LEFT   JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    WHERE  p.Estado <> 'Cerrado'
    GROUP  BY d.Nombre
    ORDER  BY Total DESC, Departamento;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Vencidas y las que vencen en los próximos 7 días (DiasRestantes < 0 = vencido)
CREATE   PROCEDURE sp_Dashboard_ProximasDevoluciones
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    SELECT TOP 8
           p.PrestamoId,
           e.Codigo AS CodigoEmpleado,
           e.Nombre AS Empleado,
           (SELECT STRING_AGG(h.Nombre, N', ') WITHIN GROUP (ORDER BY h.Nombre)
            FROM   PrestamoDetalle pd
            INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
            WHERE  pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL) AS Herramientas,
           p.FechaDevolucionEsperada,
           DATEDIFF(DAY, @Hoy, CAST(p.FechaDevolucionEsperada AS DATE)) AS DiasRestantes
    FROM   Prestamo p
    INNER  JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
    WHERE  p.Estado <> 'Cerrado'
      AND  CAST(p.FechaDevolucionEsperada AS DATE) <= DATEADD(DAY, 7, @Hoy)
      AND  EXISTS (SELECT 1 FROM PrestamoDetalle pd
                   WHERE pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL)
    ORDER  BY p.FechaDevolucionEsperada, p.PrestamoId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Devolucion_ObtenerDetallePendiente
    @PrestamoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT pd.PrestamoDetalleId,
           pd.HerramientaId,
           pd.UnidadId,
           u.CodigoUnidad AS CodigoHerramienta,
           h.Nombre       AS Herramienta,
           m.NombreMarca  AS Marca,
           c.Nombre       AS Categoria,
           pd.EstadoDevolucion,
           h.FotoNombre
    FROM   PrestamoDetalle          pd
    INNER  JOIN Herramienta          h ON pd.HerramientaId = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON pd.UnidadId      = u.UnidadId
    LEFT   JOIN Marca                m ON h.MarcaId        = m.MarcaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId    = c.CategoriaId
    WHERE  pd.PrestamoId    = @PrestamoId
      AND  pd.FechaDevuelta IS NULL
    ORDER  BY h.Nombre ASC, u.Numero ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE sp_Prestamo_MarcarVencidos
AS
BEGIN
    SET NOCOUNT ON;  -- esto suprime el conteo de filas afectadas

    UPDATE Prestamo
    SET    Estado = 'Vencido'
    WHERE  Estado                = 'Activo'
      AND  FechaDevolucionEsperada < GETDATE();

    -- SE ELIMINA: SELECT @@ROWCOUNT AS PrestamosMarcados
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- 25. Obtener préstamos con herramientas pendientes de devolver
--     Para llenar la pantalla de Devoluciones al cargar
-- ============================================================
CREATE   PROCEDURE sp_Devolucion_ObtenerPrestamosActivos
AS
BEGIN
    SET NOCOUNT ON;

    -- Llamar primero para asegurarse de que los vencidos estén marcados
    EXEC sp_Prestamo_MarcarVencidos;

    SELECT p.PrestamoId,
           e.Nombre   AS Empleado,
           e.Codigo   AS CodigoEmpleado,
           d.Nombre   AS Departamento,
           p.FechaPrestamo,
           p.FechaDevolucionEsperada,
           p.Estado,
           -- Días de atraso (negativo = aún tiene días)
           DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE()) AS DiasAtraso,
           -- Conteo de herramientas pendientes
           (SELECT COUNT(*) FROM PrestamoDetalle pd
            WHERE  pd.PrestamoId   = p.PrestamoId
              AND  pd.FechaDevuelta IS NULL) AS HerramientasPendientes
    FROM   Prestamo     p
    INNER  JOIN Empleado     e ON p.EmpleadoId      = e.EmpleadoId
    LEFT   JOIN Departamento d ON e.DepartamentoId  = d.DepartamentoId
    WHERE  p.Estado <> 'Cerrado'
      AND  EXISTS (
               SELECT 1 FROM PrestamoDetalle pd
               WHERE  pd.PrestamoId   = p.PrestamoId
                 AND  pd.FechaDevuelta IS NULL
           )
    ORDER  BY
           -- Vencidos primero, luego por fecha más antigua
           CASE WHEN p.Estado = 'Vencido' THEN 0 ELSE 1 END ASC,
           p.FechaDevolucionEsperada ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE sp_Prestamo_CerrarSiCompleto
    @PrestamoId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Si no hay ningún detalle pendiente, cerrar el préstamo
    IF NOT EXISTS (
        SELECT 1 FROM PrestamoDetalle
        WHERE  PrestamoId   = @PrestamoId
          AND  FechaDevuelta IS NULL
    )
    BEGIN
        UPDATE Prestamo
        SET    Estado      = 'Cerrado',
               FechaCierre = GETDATE()
        WHERE  PrestamoId  = @PrestamoId;
    END
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Devolucion_RegistrarTodas
    @PrestamoId            INT,
    @EstadoDevolucion      VARCHAR(20),
    @ObservacionDevolucion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @EstadoDevolucion NOT IN ('Bueno', 'Dañado', 'Perdido')
        THROW 50000, 'Estado de devolución no válido. Use: Bueno, Dañado o Perdido.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1 FROM Prestamo WITH (UPDLOCK)
            WHERE  PrestamoId = @PrestamoId
              AND  Estado     <> 'Cerrado'
        )
            THROW 50000, 'El préstamo no existe o ya está cerrado.', 1;

        DECLARE @Pendientes TABLE (PrestamoDetalleId INT PRIMARY KEY, UnidadId INT);

        INSERT INTO @Pendientes
        SELECT PrestamoDetalleId, UnidadId
        FROM   PrestamoDetalle
        WHERE  PrestamoId    = @PrestamoId
          AND  FechaDevuelta IS NULL;

        UPDATE PrestamoDetalle
        SET    FechaDevuelta         = GETDATE(),
               EstadoDevolucion      = @EstadoDevolucion,
               ObservacionDevolucion = @ObservacionDevolucion
        WHERE  PrestamoDetalleId IN (SELECT PrestamoDetalleId FROM @Pendientes);

        UPDATE HerramientaUnidad
        SET    Estado = CASE @EstadoDevolucion
                            WHEN 'Bueno'  THEN 'Disponible'
                            WHEN 'Dañado' THEN 'Dañada'
                            ELSE               'Perdida'
                        END
        WHERE  UnidadId IN (SELECT UnidadId FROM @Pendientes);

        EXEC sp_Prestamo_CerrarSiCompleto @PrestamoId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT 'OK' AS Resultado, @PrestamoId AS PrestamoId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- DEVOLUCIONES  (solo afectan a la unidad devuelta)
--   Bueno → Disponible · Dañado → Dañada · Perdido → Perdida
-- ============================================================
CREATE   PROCEDURE sp_Devolucion_RegistrarUna
    @PrestamoDetalleId       INT,
    @EstadoDevolucion        VARCHAR(20),
    @ObservacionDevolucion   VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @EstadoDevolucion NOT IN ('Bueno', 'Dañado', 'Perdido')
        THROW 50000, 'Estado de devolución no válido. Use: Bueno, Dañado o Perdido.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UnidadId INT, @PrestamoId INT;

        SELECT @UnidadId   = UnidadId,
               @PrestamoId = PrestamoId
        FROM   PrestamoDetalle WITH (UPDLOCK)
        WHERE  PrestamoDetalleId = @PrestamoDetalleId
          AND  FechaDevuelta     IS NULL;

        IF @UnidadId IS NULL
            THROW 50000, 'Este artículo ya fue devuelto o no existe.', 1;

        UPDATE PrestamoDetalle
        SET    FechaDevuelta          = GETDATE(),
               EstadoDevolucion       = @EstadoDevolucion,
               ObservacionDevolucion  = @ObservacionDevolucion
        WHERE  PrestamoDetalleId      = @PrestamoDetalleId;

        UPDATE HerramientaUnidad
        SET    Estado = CASE @EstadoDevolucion
                            WHEN 'Bueno'  THEN 'Disponible'
                            WHEN 'Dañado' THEN 'Dañada'
                            ELSE               'Perdida'
                        END
        WHERE  UnidadId = @UnidadId;

        EXEC sp_Prestamo_CerrarSiCompleto @PrestamoId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT 'OK' AS Resultado, @PrestamoId AS PrestamoId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Devolucion_RegistrarVarias
    @PrestamoId INT,
    @Items      XML
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Devolver TABLE (
        PrestamoDetalleId INT PRIMARY KEY,
        Estado            VARCHAR(20),
        Nota              NVARCHAR(250),
        UnidadId          INT NULL
    );

    INSERT INTO @Devolver (PrestamoDetalleId, Estado, Nota)
    SELECT x.item.value('@id', 'INT'),
           x.item.value('@estado', 'VARCHAR(20)'),
           NULLIF(LTRIM(RTRIM(x.item.value('@nota', 'NVARCHAR(250)'))), N'')
    FROM   @Items.nodes('/items/item') AS x(item);

    IF NOT EXISTS (SELECT 1 FROM @Devolver)
        THROW 50000, 'Seleccione al menos una herramienta a devolver.', 1;

    IF EXISTS (SELECT 1 FROM @Devolver WHERE Estado IS NULL OR Estado NOT IN ('Bueno', 'Dañado', 'Perdido'))
        THROW 50000, 'Estado de devolución no válido. Use: Bueno, Dañado o Perdido.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Prestamo WITH (UPDLOCK)
                       WHERE PrestamoId = @PrestamoId AND Estado <> 'Cerrado')
            THROW 50000, 'El préstamo no existe o ya está cerrado.', 1;

        UPDATE d
        SET    d.UnidadId = pd.UnidadId
        FROM   @Devolver d
        INNER  JOIN PrestamoDetalle pd WITH (UPDLOCK)
                ON pd.PrestamoDetalleId = d.PrestamoDetalleId
               AND pd.PrestamoId        = @PrestamoId
               AND pd.FechaDevuelta     IS NULL;

        IF EXISTS (SELECT 1 FROM @Devolver WHERE UnidadId IS NULL)
            THROW 50000, 'Una o más herramientas ya fueron devueltas o no pertenecen a este préstamo.', 1;

        UPDATE pd
        SET    pd.FechaDevuelta         = GETDATE(),
               pd.EstadoDevolucion      = d.Estado,
               pd.ObservacionDevolucion = d.Nota
        FROM   PrestamoDetalle pd
        INNER  JOIN @Devolver  d ON d.PrestamoDetalleId = pd.PrestamoDetalleId;

        UPDATE u
        SET    u.Estado = CASE d.Estado
                              WHEN 'Bueno'  THEN 'Disponible'
                              WHEN 'Dañado' THEN 'Dañada'
                              ELSE               'Perdida'
                          END
        FROM   HerramientaUnidad u
        INNER  JOIN @Devolver   d ON d.UnidadId = u.UnidadId;

        EXEC sp_Prestamo_CerrarSiCompleto @PrestamoId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    -- Resumen para el mensaje de confirmación
    SELECT SUM(CASE WHEN Estado = 'Bueno'  THEN 1 ELSE 0 END) AS Buenas,
           SUM(CASE WHEN Estado = 'Dañado' THEN 1 ELSE 0 END) AS Dañadas,
           SUM(CASE WHEN Estado = 'Perdido' THEN 1 ELSE 0 END) AS Perdidas,
           (SELECT COUNT(*) FROM PrestamoDetalle
            WHERE  PrestamoId = @PrestamoId AND FechaDevuelta IS NULL) AS Pendientes,
           CAST(CASE WHEN EXISTS (SELECT 1 FROM Prestamo
                                  WHERE PrestamoId = @PrestamoId AND Estado = 'Cerrado')
                     THEN 1 ELSE 0 END AS BIT) AS PrestamoCerrado
    FROM   @Devolver;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Crea @Cantidad unidades nuevas (Disponible) numeradas a continuación de la última.
-- Si se llama dentro de otra transacción, queda anidada en ella.
CREATE   PROCEDURE sp_Herramienta_AgregarUnidades
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- @StockTotal mayor al actual agrega unidades nuevas.
-- Para reducir el stock hay que dar de baja (o marcar perdidas) unidades concretas.
-- @FotoNombre NULL = conservar la foto actual (la foto se cambia con sp_Herramienta_ActualizarFoto).
CREATE   PROCEDURE sp_Herramienta_Actualizar
    @HerramientaId   INT,
    @Nombre          VARCHAR(120),
    @Caracteristicas VARCHAR(400) = NULL,
    @CategoriaId     INT          = NULL,
    @MarcaId         INT          = NULL,
    @UbicacionId     INT          = NULL,
    @StockTotal      INT          = NULL,
    @FotoNombre      NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId)
        THROW 50000, 'La herramienta especificada no existe.', 1;

    DECLARE @StockActual INT = (SELECT StockTotal FROM vw_HerramientaStock WHERE HerramientaId = @HerramientaId);

    IF @StockTotal IS NOT NULL AND @StockTotal < @StockActual
        THROW 50000, 'Para reducir el stock, dé de baja o marque como perdidas las unidades concretas desde "Unidades".', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Herramienta
        SET    Nombre          = @Nombre,
               Caracteristicas = @Caracteristicas,
               CategoriaId     = @CategoriaId,
               MarcaId         = @MarcaId,
               UbicacionId     = @UbicacionId,
               FotoNombre      = ISNULL(@FotoNombre, FotoNombre)
        WHERE  HerramientaId  = @HerramientaId;

        IF @StockTotal > @StockActual
        BEGIN
            DECLARE @Nuevas INT = @StockTotal - @StockActual;
            EXEC sp_Herramienta_AgregarUnidades @HerramientaId, @Nuevas;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Solo la foto (botón "Cambiar foto" de FrmHerramientas). NULL = quitar la foto.
CREATE   PROCEDURE sp_Herramienta_ActualizarFoto
    @HerramientaId INT,
    @FotoNombre    NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Herramienta SET FotoNombre = @FotoNombre WHERE HerramientaId = @HerramientaId;

    IF @@ROWCOUNT = 0
        THROW 50000, 'La herramienta especificada no existe.', 1;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Herramienta_Buscar
    @Termino VARCHAR(120)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre,
           h.Caracteristicas,
           CASE WHEN h.PrestamoHabilitado = 1 THEN 'Habilitado' ELSE 'Suspendido' END AS Prestamo,
           s.StockTotal,
           s.StockDisponible,
           s.StockPrestado,
           s.StockMantenimiento,
           s.StockDañado,
           h.Activa,
           h.PrestamoHabilitado,
           c.Nombre       AS Categoria,
           m.NombreMarca  AS Marca,
           u.Nombre       AS Ubicacion,
           h.CategoriaId,
           h.MarcaId,
           h.UbicacionId
    FROM   Herramienta    h
    INNER  JOIN vw_HerramientaStock    s ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca                m ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            u ON h.UbicacionId   = u.UbicacionId
    WHERE  h.Activa = 1
      AND (h.Nombre  LIKE '%' + @Termino + '%'
        OR h.Codigo  LIKE '%' + @Termino + '%')
    ORDER  BY h.Nombre ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- Cambia el estado de unidades de una herramienta.
--   @UnidadIds = '3,7,8'  → solo esas unidades (todas deben poder cambiar)
--   @UnidadIds = NULL     → todo el grupo (se omiten las que no pueden cambiar)
-- Estados manuales: Disponible, Dañada, Perdida, Baja.
-- Prestada / En Mantenimiento solo cambian por préstamo, devolución o mantenimiento.
-- Dar de baja el grupo completo además desactiva la herramienta.
-- ============================================================
CREATE   PROCEDURE sp_Unidad_CambiarEstado
    @HerramientaId INT,
    @UnidadIds     VARCHAR(MAX)  = NULL,
    @NuevoEstado   VARCHAR(20),
    @Observacion   NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @NuevoEstado NOT IN ('Disponible', 'Dañada', 'Perdida', 'Baja')
        THROW 50000, 'Estado no válido. Use: Disponible, Dañada, Perdida o Baja.', 1;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
        THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Objetivo TABLE (UnidadId INT PRIMARY KEY, Estado VARCHAR(20));

        IF @UnidadIds IS NULL
        BEGIN
            INSERT INTO @Objetivo (UnidadId, Estado)
            SELECT UnidadId, Estado
            FROM   HerramientaUnidad WITH (UPDLOCK, HOLDLOCK)
            WHERE  HerramientaId = @HerramientaId
              AND  Estado <> 'Baja';

            IF @NuevoEstado = 'Baja'
               AND EXISTS (SELECT 1 FROM @Objetivo WHERE Estado IN ('Prestada', 'En Mantenimiento'))
                THROW 50000, 'No se puede dar de baja todo el grupo: hay unidades prestadas o en mantenimiento.', 1;
        END
        ELSE
        BEGIN
            DECLARE @Pedidas TABLE (UnidadId INT PRIMARY KEY);
            INSERT INTO @Pedidas (UnidadId)
            SELECT DISTINCT TRY_CAST(value AS INT)
            FROM   STRING_SPLIT(@UnidadIds, ',')
            WHERE  TRY_CAST(value AS INT) IS NOT NULL;

            INSERT INTO @Objetivo (UnidadId, Estado)
            SELECT u.UnidadId, u.Estado
            FROM   HerramientaUnidad u WITH (UPDLOCK, HOLDLOCK)
            INNER  JOIN @Pedidas p ON p.UnidadId = u.UnidadId
            WHERE  u.HerramientaId = @HerramientaId;

            IF NOT EXISTS (SELECT 1 FROM @Pedidas)
               OR (SELECT COUNT(*) FROM @Objetivo) <> (SELECT COUNT(*) FROM @Pedidas)
                THROW 50000, 'Una o más unidades no pertenecen a esta herramienta.', 1;

            IF EXISTS (SELECT 1 FROM @Objetivo WHERE Estado IN ('Prestada', 'En Mantenimiento', 'Baja'))
                THROW 50000, 'Hay unidades prestadas, en mantenimiento o ya dadas de baja. Registre primero la devolución o cierre el mantenimiento.', 1;
        END

        UPDATE u
        SET    Estado        = @NuevoEstado,
               FechaBaja     = CASE WHEN @NuevoEstado = 'Baja' THEN SYSDATETIME() END,
               Observaciones = ISNULL(NULLIF(@Observacion, N''), u.Observaciones)
        FROM   HerramientaUnidad u
        INNER  JOIN @Objetivo    o ON o.UnidadId = u.UnidadId
        WHERE  o.Estado NOT IN ('Prestada', 'En Mantenimiento', 'Baja')
          AND  o.Estado <> @NuevoEstado;

        DECLARE @Afectadas INT = @@ROWCOUNT;

        IF @UnidadIds IS NULL AND @NuevoEstado = 'Baja'
            UPDATE Herramienta
            SET    Activa = 0, PrestamoHabilitado = 0
            WHERE  HerramientaId = @HerramientaId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT @Afectadas                                  AS Afectadas,
           (SELECT COUNT(*) FROM @Objetivo) - @Afectadas AS Omitidas;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Baja del grupo completo (todas sus unidades). Para una sola unidad usar sp_Unidad_CambiarEstado.
CREATE   PROCEDURE sp_Herramienta_DarDeBaja
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;
    EXEC sp_Unidad_CambiarEstado @HerramientaId = @HerramientaId, @UnidadIds = NULL, @NuevoEstado = 'Baja';
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Suspende o reanuda el préstamo de TODO el grupo (las unidades conservan su estado).
CREATE   PROCEDURE sp_Herramienta_HabilitarPrestamo
    @HerramientaId INT,
    @Habilitar     BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
        THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

    UPDATE Herramienta
    SET    PrestamoHabilitado = @Habilitar
    WHERE  HerramientaId = @HerramientaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Inserción / actualización: @FotoNombre opcional ───────────

CREATE   PROCEDURE sp_Herramienta_Insertar
    @Nombre          VARCHAR(120),
    @Caracteristicas VARCHAR(400) = NULL,
    @CategoriaId     INT          = NULL,
    @MarcaId         INT          = NULL,
    @UbicacionId     INT          = NULL,
    @StockTotal      INT          = 1,
    @FotoNombre      NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Herramienta (
            Nombre, Caracteristicas,
            CategoriaId, MarcaId, UbicacionId, Activa, PrestamoHabilitado, FotoNombre
        )
        VALUES (
            @Nombre, @Caracteristicas,
            @CategoriaId, @MarcaId, @UbicacionId, 1, 1, @FotoNombre
        );

        DECLARE @HerramientaId INT = SCOPE_IDENTITY();

        EXEC sp_Herramienta_AgregarUnidades @HerramientaId, @StockTotal;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT HerramientaId, Codigo FROM Herramienta WHERE HerramientaId = @HerramientaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Herramienta_ObtenerCatalogoPrestamo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre,
           c.Nombre              AS Categoria,
           m.NombreMarca         AS Marca,
           ub.Nombre             AS Ubicacion,
           ISNULL(s.StockDisponible, 0) AS StockDisponible,
           ISNULL(s.StockTotal, 0)      AS StockTotal,
           h.FotoNombre
    FROM   Herramienta               h
    LEFT   JOIN vw_HerramientaStock  s  ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c  ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca                m  ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            ub ON h.UbicacionId   = ub.UbicacionId
    WHERE  h.Activa             = 1
      AND  h.PrestamoHabilitado = 1
    ORDER  BY h.Nombre ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Una fila por unidad disponible para préstamo.
CREATE   PROCEDURE sp_Herramienta_ObtenerDisponibles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UnidadId,
           h.HerramientaId,
           u.CodigoUnidad  AS Codigo,
           h.Nombre,
           c.Nombre        AS Categoria,
           m.NombreMarca   AS Marca,
           ub.Nombre       AS Ubicacion
    FROM   vw_HerramientaUnidad u
    INNER  JOIN Herramienta          h  ON h.HerramientaId = u.HerramientaId
    LEFT   JOIN CategoriaHerramienta c  ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca                m  ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            ub ON h.UbicacionId   = ub.UbicacionId
    WHERE  u.Estado             = 'Disponible'
      AND  h.Activa             = 1
      AND  h.PrestamoHabilitado = 1
    ORDER  BY h.Nombre ASC, u.Numero ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Herramienta_ObtenerPorId
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre,
           h.Caracteristicas,
           CASE WHEN h.PrestamoHabilitado = 1 THEN 'Habilitado' ELSE 'Suspendido' END AS Prestamo,
           s.StockTotal,
           s.StockDisponible,
           s.StockPrestado,
           s.StockMantenimiento,
           s.StockDañado,
           s.StockPerdido,
           s.StockBaja,
           h.Activa,
           h.PrestamoHabilitado,
           c.Nombre       AS Categoria,
           m.NombreMarca  AS Marca,
           u.Nombre       AS Ubicacion,
           h.CategoriaId,
           h.MarcaId,
           h.UbicacionId,
           h.FotoNombre
    FROM   Herramienta    h
    INNER  JOIN vw_HerramientaStock    s ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca                m ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            u ON h.UbicacionId   = u.UbicacionId
    WHERE  h.HerramientaId = @HerramientaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Consultas: incluyen FotoNombre ─────────────────────────────

CREATE   PROCEDURE sp_Herramienta_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre,
           h.Caracteristicas,
           CASE WHEN h.PrestamoHabilitado = 1 THEN 'Habilitado' ELSE 'Suspendido' END AS Prestamo,
           s.StockTotal,
           s.StockDisponible,
           s.StockPrestado,
           s.StockMantenimiento,
           s.StockDañado,
           h.Activa,
           h.PrestamoHabilitado,
           c.Nombre       AS Categoria,
           m.NombreMarca  AS Marca,
           u.Nombre       AS Ubicacion,
           h.CategoriaId,
           h.MarcaId,
           h.UbicacionId,
           h.FotoNombre
    FROM   Herramienta    h
    INNER  JOIN vw_HerramientaStock    s ON s.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca                m ON h.MarcaId       = m.MarcaId
    LEFT   JOIN Ubicacion            u ON h.UbicacionId   = u.UbicacionId
    WHERE  h.Activa = 1
    ORDER  BY h.Nombre ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Herramienta_ObtenerUnidades
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UnidadId,
           u.CodigoUnidad,
           u.Estado,
           pr.Empleado      AS PrestadaA,
           pr.FechaDevolucionEsperada,
           u.FechaAlta,
           u.FechaBaja,
           u.Observaciones
    FROM   vw_HerramientaUnidad u
    OUTER  APPLY (
        SELECT TOP 1 e.Nombre AS Empleado, p.FechaDevolucionEsperada
        FROM   PrestamoDetalle pd
        INNER  JOIN Prestamo   p ON p.PrestamoId = pd.PrestamoId
        INNER  JOIN Empleado   e ON e.EmpleadoId = p.EmpleadoId
        WHERE  pd.UnidadId = u.UnidadId
          AND  pd.FechaDevuelta IS NULL
    ) pr
    WHERE  u.HerramientaId = @HerramientaId
    ORDER  BY u.Numero;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Mantenimiento_ObtenerActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.MantenimientoId,
           m.FechaInicio,
           m.TipoMantenimiento,
           m.Descripcion,
           m.TipoServicio,
           COALESCE(e.Nombre, p.Nombre, m.RealizadoPor) AS RealizadoPor,
           h.HerramientaId,
           m.UnidadId,
           un.CodigoUnidad AS CodigoHerramienta,
           h.Nombre        AS Herramienta,
           c.Nombre        AS Categoria,
           mk.NombreMarca  AS Marca,
           u.Nombre        AS Ubicacion,
           DATEDIFF(DAY, m.FechaInicio, GETDATE()) AS DiasEnMantenimiento
    FROM   Mantenimiento          m
    INNER  JOIN Herramienta          h  ON m.HerramientaId = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad un ON m.UnidadId      = un.UnidadId
    LEFT   JOIN Empleado             e  ON m.EmpleadoId    = e.EmpleadoId
    LEFT   JOIN Proveedor            p  ON m.ProveedorId   = p.ProveedorId
    LEFT   JOIN CategoriaHerramienta c  ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca                mk ON h.MarcaId       = mk.MarcaId
    LEFT   JOIN Ubicacion            u  ON h.UbicacionId   = u.UbicacionId
    WHERE  m.FechaFin IS NULL
    ORDER  BY m.FechaInicio ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Historial de una herramienta. Los alias son los encabezados de la ventana de historial.
CREATE   PROCEDURE sp_Mantenimiento_ObtenerPorHerramienta
    @HerramientaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.CodigoUnidad                                 AS Unidad,
           m.FechaInicio                                  AS Inicio,
           m.FechaFin                                     AS Cierre,
           DATEDIFF(DAY, m.FechaInicio, ISNULL(m.FechaFin, GETDATE())) AS [Días],
           m.TipoMantenimiento                            AS Tipo,
           m.TipoServicio                                 AS Servicio,
           COALESCE(e.Nombre, p.Nombre, m.RealizadoPor)   AS [Realizado por],
           m.Descripcion                                  AS [Descripción],
           m.NotasCierre                                  AS [Notas de cierre],
           CASE WHEN m.FechaFin IS NULL THEN 'En curso' ELSE ISNULL(m.Resultado, 'Finalizado') END AS Resultado,
           m.CostoMateriales                              AS Materiales,
           m.CostoManoObra                                AS [Mano de obra],
           m.Costo                                        AS [Costo total],
           CASE WHEN m.EnGarantia = 1 THEN 'Sí' ELSE 'No' END AS [Garantía],
           m.FolioFactura                                 AS Factura,
           de.Nombre                                      AS [Daño reportado por]
    FROM   Mantenimiento m
    INNER  JOIN vw_HerramientaUnidad u  ON u.UnidadId            = m.UnidadId
    LEFT   JOIN Empleado             e  ON e.EmpleadoId          = m.EmpleadoId
    LEFT   JOIN Proveedor            p  ON p.ProveedorId         = m.ProveedorId
    LEFT   JOIN PrestamoDetalle      pd ON pd.PrestamoDetalleId  = m.PrestamoDetalleId
    LEFT   JOIN Prestamo             pr ON pr.PrestamoId         = pd.PrestamoId
    LEFT   JOIN Empleado             de ON de.EmpleadoId         = pr.EmpleadoId
    WHERE  m.HerramientaId = @HerramientaId
    ORDER  BY m.FechaInicio DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ════════════════════════════════════════════════════════════
-- PROCEDIMIENTOS DE MANTENIMIENTO
-- ════════════════════════════════════════════════════════════

-- Abre un mantenimiento por unidad.
--   @UnidadIds = '3,7'  → esas unidades; pueden ser de distintas herramientas
--                         (todas deben estar Disponibles o Dañadas)
--   @UnidadIds = NULL   → todas las unidades Disponibles o Dañadas de @HerramientaId
--   Interno → @EmpleadoId obligatorio · Externo → @ProveedorId obligatorio
CREATE   PROCEDURE sp_Mantenimiento_RegistrarEntrada
    @HerramientaId      INT          = NULL,
    @UnidadIds          VARCHAR(MAX) = NULL,
    @TipoMantenimiento  VARCHAR(50),
    @Descripcion        VARCHAR(400) = NULL,
    @TipoServicio       VARCHAR(10)  = 'Interno',
    @EmpleadoId         INT          = NULL,
    @ProveedorId        INT          = NULL,
    @RegistradoPorId    INT          = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @TipoMantenimiento NOT IN ('Preventivo', 'Correctivo', 'Calibración')
        THROW 50000, 'Tipo de mantenimiento no válido. Use: Preventivo, Correctivo o Calibración.', 1;

    IF @TipoServicio NOT IN ('Interno', 'Externo')
        THROW 50000, 'Tipo de servicio no válido. Use: Interno o Externo.', 1;

    IF @TipoServicio = 'Interno'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Empleado WHERE EmpleadoId = @EmpleadoId AND Activo = 1)
            THROW 50000, 'Seleccione el empleado que realizará el mantenimiento.', 1;
        SET @ProveedorId = NULL;
    END
    ELSE
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE ProveedorId = @ProveedorId AND Activo = 1)
            THROW 50000, 'Seleccione el proveedor que realizará el mantenimiento.', 1;
        SET @EmpleadoId = NULL;
    END

    IF @HerramientaId IS NULL AND @UnidadIds IS NULL
        THROW 50000, 'Indique la herramienta o las unidades a enviar a mantenimiento.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Objetivo TABLE (UnidadId INT PRIMARY KEY, HerramientaId INT, Estado VARCHAR(20));

        IF @UnidadIds IS NULL
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM Herramienta WHERE HerramientaId = @HerramientaId AND Activa = 1)
                THROW 50000, 'La herramienta no existe o está dada de baja.', 1;

            INSERT INTO @Objetivo (UnidadId, HerramientaId, Estado)
            SELECT UnidadId, HerramientaId, Estado
            FROM   HerramientaUnidad WITH (UPDLOCK, HOLDLOCK)
            WHERE  HerramientaId = @HerramientaId
              AND  Estado IN ('Disponible', 'Dañada');

            IF NOT EXISTS (SELECT 1 FROM @Objetivo)
                THROW 50000, 'El grupo no tiene unidades disponibles o dañadas para enviar a mantenimiento.', 1;
        END
        ELSE
        BEGIN
            DECLARE @Pedidas TABLE (UnidadId INT PRIMARY KEY);
            INSERT INTO @Pedidas (UnidadId)
            SELECT DISTINCT TRY_CAST(value AS INT)
            FROM   STRING_SPLIT(@UnidadIds, ',')
            WHERE  TRY_CAST(value AS INT) IS NOT NULL;

            INSERT INTO @Objetivo (UnidadId, HerramientaId, Estado)
            SELECT u.UnidadId, u.HerramientaId, u.Estado
            FROM   HerramientaUnidad u WITH (UPDLOCK, HOLDLOCK)
            INNER  JOIN @Pedidas    p ON p.UnidadId      = u.UnidadId
            INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
            WHERE  h.Activa = 1
              AND  (@HerramientaId IS NULL OR u.HerramientaId = @HerramientaId)
              AND  u.Estado IN ('Disponible', 'Dañada');

            IF NOT EXISTS (SELECT 1 FROM @Pedidas)
               OR (SELECT COUNT(*) FROM @Objetivo) <> (SELECT COUNT(*) FROM @Pedidas)
                THROW 50000, 'Solo pueden entrar a mantenimiento unidades Disponibles o Dañadas de herramientas activas.', 1;
        END

        INSERT INTO Mantenimiento (
            HerramientaId, UnidadId, FechaInicio, TipoMantenimiento, Descripcion,
            TipoServicio, EmpleadoId, ProveedorId, RegistradoPorId, PrestamoDetalleId
        )
        SELECT o.HerramientaId, o.UnidadId, GETDATE(), @TipoMantenimiento, @Descripcion,
               @TipoServicio, @EmpleadoId, @ProveedorId, @RegistradoPorId, r.PrestamoDetalleId
        FROM   @Objetivo o
        OUTER  APPLY (SELECT * FROM fn_UnidadReporteDanio(o.UnidadId) WHERE o.Estado = 'Dañada') r;

        DECLARE @Unidades INT = @@ROWCOUNT;

        UPDATE HerramientaUnidad
        SET    Estado = 'En Mantenimiento'
        WHERE  UnidadId IN (SELECT UnidadId FROM @Objetivo);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT @Unidades AS Unidades;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Cierra un mantenimiento activo.
--   @Resultado: 'Disponible' (reparada) o 'Baja' (irreparable, sale del stock)
--   Costo = materiales + mano de obra
CREATE   PROCEDURE sp_Mantenimiento_RegistrarSalida
    @MantenimientoId INT,
    @NotasCierre     VARCHAR(400)  = NULL,
    @CostoMateriales DECIMAL(10,2) = NULL,
    @CostoManoObra   DECIMAL(10,2) = NULL,
    @EnGarantia      BIT           = 0,
    @FolioFactura    VARCHAR(50)   = NULL,
    @Resultado       VARCHAR(20)   = 'Disponible',
    @CerradoPorId    INT           = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Resultado NOT IN ('Disponible', 'Baja')
        THROW 50000, 'Resultado no válido. Use: Disponible o Baja.', 1;

    IF ISNULL(@CostoMateriales, 0) < 0 OR ISNULL(@CostoManoObra, 0) < 0
        THROW 50000, 'Los costos no pueden ser negativos.', 1;

    IF @EnGarantia = 1 AND ISNULL(@CostoMateriales, 0) + ISNULL(@CostoManoObra, 0) > 0
        THROW 50000, 'Un mantenimiento en garantía no tiene costo para la empresa.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UnidadId INT, @HerramientaId INT, @TipoServicio VARCHAR(10);

        SELECT @UnidadId      = UnidadId,
               @HerramientaId = HerramientaId,
               @TipoServicio  = TipoServicio
        FROM   Mantenimiento WITH (UPDLOCK)
        WHERE  MantenimientoId = @MantenimientoId
          AND  FechaFin        IS NULL;

        IF @UnidadId IS NULL
            THROW 50000, 'El mantenimiento no existe o ya fue cerrado.', 1;

        IF @TipoServicio = 'Interno' AND ISNULL(@CostoManoObra, 0) > 0
            THROW 50000, 'La mano de obra de un servicio interno no se registra: ya la cubre el salario del empleado.', 1;

        UPDATE Mantenimiento
        SET    FechaFin        = GETDATE(),
               NotasCierre     = NULLIF(@NotasCierre, ''),
               CostoMateriales = @CostoMateriales,
               CostoManoObra   = CASE WHEN @TipoServicio = 'Externo' THEN @CostoManoObra END,
               Costo           = ISNULL(@CostoMateriales, 0) + ISNULL(@CostoManoObra, 0),
               EnGarantia      = @EnGarantia,
               FolioFactura    = CASE WHEN @TipoServicio = 'Externo' THEN NULLIF(@FolioFactura, '') END,
               Resultado       = CASE WHEN @Resultado = 'Baja' THEN 'Baja' ELSE 'Reparada' END,
               CerradoPorId    = @CerradoPorId
        WHERE  MantenimientoId = @MantenimientoId;

        UPDATE HerramientaUnidad
        SET    Estado    = @Resultado,
               FechaBaja = CASE WHEN @Resultado = 'Baja' THEN SYSDATETIME() END
        WHERE  UnidadId  = @UnidadId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    SELECT 'OK' AS Resultado, @HerramientaId AS HerramientaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Unidades que pueden entrar a mantenimiento (Disponibles o Dañadas de herramientas activas).
-- Las Dañadas son la bandeja de pendientes: van primero, con quién y cuándo reportó el daño.
CREATE   PROCEDURE sp_Mantenimiento_UnidadesElegibles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UnidadId,
           u.HerramientaId,
           u.CodigoUnidad,
           h.Nombre  AS Herramienta,
           c.Nombre  AS Categoria,
           ub.Nombre AS Ubicacion,
           u.Estado,
           r.PrestamoDetalleId,
           r.FechaReporte,
           r.ReportadoPor,
           CASE WHEN u.Estado = 'Dañada' THEN COALESCE(r.Nota, u.Observaciones) END AS NotaReporte,
           DATEDIFF(DAY, r.FechaReporte, GETDATE()) AS DiasEsperando
    FROM   vw_HerramientaUnidad u
    INNER  JOIN Herramienta          h  ON h.HerramientaId = u.HerramientaId
    LEFT   JOIN CategoriaHerramienta c  ON c.CategoriaId   = h.CategoriaId
    LEFT   JOIN Ubicacion            ub ON ub.UbicacionId  = h.UbicacionId
    OUTER  APPLY (SELECT * FROM fn_UnidadReporteDanio(u.UnidadId) WHERE u.Estado = 'Dañada') r
    WHERE  h.Activa = 1
      AND  u.Estado IN ('Disponible', 'Dañada')
    ORDER  BY CASE WHEN u.Estado = 'Dañada' THEN 0 ELSE 1 END,
              r.FechaReporte, h.Nombre, u.Numero;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 3. Actualizar marca
CREATE   PROCEDURE sp_Marca_Actualizar
    @MarcaId     INT,
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Marca WHERE MarcaId = @MarcaId)
    BEGIN
        RAISERROR('La marca especificada no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Marca WHERE NombreMarca = @Nombre AND MarcaId <> @MarcaId)
    BEGIN
        RAISERROR('Ya existe otra marca con ese nombre.', 16, 1);
        RETURN;
    END

    UPDATE Marca
    SET    NombreMarca      = @Nombre,
           Descripcion = @Descripcion
    WHERE  MarcaId     = @MarcaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Marca_Eliminar
    @MarcaId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Herramienta SET MarcaId = NULL WHERE MarcaId = @MarcaId;
        DELETE FROM Marca WHERE MarcaId = @MarcaId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 2. Insertar marca
CREATE   PROCEDURE sp_Marca_Insertar
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(400) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Marca WHERE NombreMarca = @Nombre)
    BEGIN
        RAISERROR('Ya existe una marca con ese nombre.', 16, 1);
        RETURN;
    END

    INSERT INTO Marca (NombreMarca, Descripcion)
    VALUES (@Nombre, @Descripcion);

    SELECT SCOPE_IDENTITY() AS MarcaId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Marcas ───────────────────────────────────────────────────
CREATE   PROCEDURE sp_Marca_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.MarcaId,
           m.NombreMarca AS Nombre,
           m.Descripcion,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.MarcaId = m.MarcaId AND h.Activa = 1) AS Herramientas
    FROM   Marca m
    ORDER  BY m.NombreMarca ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Prestamo_ObtenerDetalle
    @PrestamoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PrestamoId,
           p.FechaPrestamo,
           p.FechaDevolucionEsperada,
           p.FechaCierre,
           p.Estado,
           p.Observaciones,
           e.EmpleadoId,
           e.Nombre   AS Empleado,
           e.Codigo   AS CodigoEmpleado,
           a.Nombre   AS AprobadoPor,
           d.Nombre   AS Departamento
    FROM   Prestamo p
    INNER  JOIN Empleado    e ON p.EmpleadoId    = e.EmpleadoId
    LEFT   JOIN Empleado    a ON p.AprobadoPorId = a.EmpleadoId
    LEFT   JOIN Departamento d ON e.DepartamentoId = d.DepartamentoId
    WHERE  p.PrestamoId = @PrestamoId;

    SELECT pd.PrestamoDetalleId,
           pd.HerramientaId,
           pd.UnidadId,
           u.CodigoUnidad        AS CodigoHerramienta,
           h.Nombre              AS Herramienta,
           pd.EstadoDevolucion,
           pd.FechaDevuelta,
           pd.ObservacionDevolucion,
           CASE WHEN pd.FechaDevuelta IS NULL THEN 'Pendiente' ELSE 'Devuelta' END AS Situacion
    FROM   PrestamoDetalle pd
    INNER  JOIN Herramienta          h ON pd.HerramientaId = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON pd.UnidadId      = u.UnidadId
    WHERE  pd.PrestamoId = @PrestamoId
    ORDER  BY h.Nombre ASC, u.Numero ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- 21. Obtener préstamos activos de un empleado
-- ============================================================
CREATE   PROCEDURE sp_Prestamo_ObtenerPorEmpleado
    @EmpleadoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PrestamoId,
           p.FechaPrestamo,
           p.FechaDevolucionEsperada,
           p.Estado,
           -- Marcar visualmente si está vencido aunque el estado no se haya actualizado aún
           CASE
               WHEN p.Estado = 'Activo' AND p.FechaDevolucionEsperada < GETDATE()
               THEN 'Vencido'
               ELSE p.Estado
           END AS EstadoReal,
           DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE()) AS DiasAtraso,
           (SELECT COUNT(*) FROM PrestamoDetalle pd
            WHERE  pd.PrestamoId   = p.PrestamoId
              AND  pd.FechaDevuelta IS NULL) AS PendientesDevolucion
    FROM   Prestamo p
    WHERE  p.EmpleadoId = @EmpleadoId
      AND  p.Estado    <> 'Cerrado'
    ORDER  BY p.FechaPrestamo DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- 19. Obtener préstamos con filtros opcionales
-- ============================================================
CREATE   PROCEDURE sp_Prestamo_ObtenerTodos
    @Estado      VARCHAR(20)  = NULL,   -- 'Activo','Cerrado','Vencido' o NULL para todos
    @EmpleadoId  INT          = NULL,   -- NULL para todos
    @FechaDesde  DATETIME     = NULL,
    @FechaHasta  DATETIME     = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PrestamoId,
           p.FechaPrestamo,
           p.FechaDevolucionEsperada,
           p.FechaCierre,
           p.Estado,
           p.Observaciones,
           e.Nombre  AS Empleado,
           e.Codigo  AS CodigoEmpleado,
           a.Nombre  AS AprobadoPor,
           -- Cantidad de herramientas en el préstamo
           (SELECT COUNT(*) FROM PrestamoDetalle pd
            WHERE pd.PrestamoId = p.PrestamoId) AS TotalHerramientas,
           -- Cantidad pendiente de devolver
           (SELECT COUNT(*) FROM PrestamoDetalle pd
            WHERE pd.PrestamoId = p.PrestamoId
              AND pd.FechaDevuelta IS NULL)      AS PendientesDevolucion
    FROM   Prestamo p
    INNER  JOIN Empleado e ON p.EmpleadoId    = e.EmpleadoId
    LEFT   JOIN Empleado a ON p.AprobadoPorId = a.EmpleadoId
    WHERE  (@Estado     IS NULL OR p.Estado     = @Estado)
      AND  (@EmpleadoId IS NULL OR p.EmpleadoId = @EmpleadoId)
      AND  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR p.FechaPrestamo <= @FechaHasta)
    ORDER  BY p.FechaPrestamo DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Préstamos: el aprobador sale del usuario en sesión ───────
--   @Herramientas: '<herramientas><item id="UnidadId"/>...</herramientas>'
CREATE   PROCEDURE sp_Prestamo_Registrar
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Proveedor_Actualizar
    @ProveedorId INT,
    @Nombre      VARCHAR(120),
    @Telefono    VARCHAR(30)  = NULL,
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE ProveedorId = @ProveedorId AND Activo = 1)
        THROW 50000, 'El proveedor especificado no existe.', 1;

    IF EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = @Nombre AND ProveedorId <> @ProveedorId)
        THROW 50000, 'Ya existe otro proveedor con ese nombre.', 1;

    UPDATE Proveedor
    SET    Nombre = @Nombre, Telefono = @Telefono, Descripcion = @Descripcion
    WHERE  ProveedorId = @ProveedorId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Con mantenimientos registrados solo se desactiva, para conservar el historial
CREATE   PROCEDURE sp_Proveedor_Eliminar
    @ProveedorId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Mantenimiento WHERE ProveedorId = @ProveedorId)
        UPDATE Proveedor SET Activo = 0 WHERE ProveedorId = @ProveedorId;
    ELSE
        DELETE FROM Proveedor WHERE ProveedorId = @ProveedorId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Si el nombre pertenece a un proveedor dado de baja, se reactiva con los datos nuevos
CREATE   PROCEDURE sp_Proveedor_Insertar
    @Nombre      VARCHAR(120),
    @Telefono    VARCHAR(30)  = NULL,
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = @Nombre AND Activo = 1)
        THROW 50000, 'Ya existe un proveedor con ese nombre.', 1;

    IF EXISTS (SELECT 1 FROM Proveedor WHERE Nombre = @Nombre)
        UPDATE Proveedor
        SET    Activo = 1, Telefono = @Telefono, Descripcion = @Descripcion
        WHERE  Nombre = @Nombre;
    ELSE
        INSERT INTO Proveedor (Nombre, Telefono, Descripcion)
        VALUES (@Nombre, @Telefono, @Descripcion);
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Proveedores ──────────────────────────────────────────────
CREATE   PROCEDURE sp_Proveedor_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProveedorId,
           p.Nombre,
           p.Telefono,
           p.Descripcion,
           (SELECT COUNT(*) FROM Mantenimiento m WHERE m.ProveedorId = p.ProveedorId) AS Mantenimientos
    FROM   Proveedor p
    WHERE  p.Activo = 1
    ORDER  BY p.Nombre ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- 37. Reporte de costos de mantenimiento
--     Por herramienta y por período
-- ============================================================
CREATE   PROCEDURE sp_Reporte_CostosMantenimiento
    @FechaDesde    DATETIME = NULL,
    @FechaHasta    DATETIME = NULL,
    @HerramientaId INT      = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.HerramientaId,
        h.Codigo                        AS CodigoHerramienta,
        h.Nombre                        AS Herramienta,
        c.Nombre                        AS Categoria,
        m.NombreMarca                        AS Marca,
        COUNT(mt.MantenimientoId)       AS TotalMantenimientos,
        SUM(CASE WHEN mt.TipoMantenimiento = 'Preventivo'
                 THEN 1 ELSE 0 END)    AS TotalPreventivos,
        SUM(CASE WHEN mt.TipoMantenimiento = 'Correctivo'
                 THEN 1 ELSE 0 END)    AS TotalCorrectivos,
        SUM(ISNULL(mt.Costo, 0))       AS CostoTotal,
        AVG(ISNULL(mt.Costo, 0))       AS CostoPromedio,
        MAX(mt.Costo)                   AS CostoMaximo,
        AVG(DATEDIFF(DAY,
            mt.FechaInicio,
            ISNULL(mt.FechaFin, GETDATE())
        ))                              AS PromedioDiasMantenimiento
    FROM   Mantenimiento          mt
    INNER  JOIN Herramienta       h  ON mt.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca             m  ON h.MarcaId        = m.MarcaId
    WHERE  (@FechaDesde    IS NULL OR mt.FechaInicio  >= @FechaDesde)
      AND  (@FechaHasta    IS NULL OR mt.FechaInicio  <= DATEADD(DAY, 1, @FechaHasta))
      AND  (@HerramientaId IS NULL OR mt.HerramientaId = @HerramientaId)
      AND  mt.FechaFin IS NOT NULL    -- Solo mantenimientos cerrados
    GROUP  BY h.HerramientaId, h.Codigo, h.Nombre, c.Nombre, m.NombreMarca
    ORDER  BY CostoTotal DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- @Condicion: 'Dañada' | 'Perdida' | NULL
CREATE   PROCEDURE sp_Reporte_DanadasPerdidas
    @FechaDesde DATE        = NULL,
    @FechaHasta DATE        = NULL,
    @Condicion  VARCHAR(20) = NULL,
    @EmpleadoId INT         = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT h.Nombre        AS Herramienta,
           u.CodigoUnidad,
           CASE pd.EstadoDevolucion WHEN 'Dañado' THEN 'Dañada' ELSE 'Perdida' END AS Condicion,
           pd.ObservacionDevolucion AS Nota,
           p.PrestamoId,
           e.Codigo        AS CodigoEmpleado,
           e.Nombre        AS Empleado,
           d.Nombre        AS Departamento,
           pd.FechaDevuelta AS FechaDevolucion
    FROM   PrestamoDetalle pd
    INNER  JOIN Prestamo             p ON p.PrestamoId    = pd.PrestamoId
    INNER  JOIN Empleado             e ON e.EmpleadoId    = p.EmpleadoId
    LEFT   JOIN Departamento         d ON d.DepartamentoId = e.DepartamentoId
    INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = pd.UnidadId
    WHERE  pd.EstadoDevolucion IN ('Dañado', 'Perdido')
      AND  (@FechaDesde IS NULL OR pd.FechaDevuelta >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR pd.FechaDevuelta <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@Condicion IS NULL
            OR (@Condicion = 'Dañada'  AND pd.EstadoDevolucion = 'Dañado')
            OR (@Condicion = 'Perdida' AND pd.EstadoDevolucion = 'Perdido'))
      AND  (@EmpleadoId IS NULL OR p.EmpleadoId = @EmpleadoId)
    ORDER  BY pd.FechaDevuelta DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- 35. Reporte de empleados con atrasos
--     Empleados que tienen o han tenido préstamos vencidos
-- ============================================================
CREATE   PROCEDURE sp_Reporte_EmpleadosConAtrasos
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.EmpleadoId,
        e.Codigo                AS CodigoEmpleado,
        e.Nombre                AS Empleado,
        d.Nombre                AS Departamento,
        -- Atrasos históricos (préstamos ya cerrados que se devolvieron tarde)
        SUM(CASE
                WHEN p.Estado = 'Cerrado'
                 AND p.FechaCierre > p.FechaDevolucionEsperada
                THEN 1 ELSE 0
            END)                AS AtrasosHistoricos,
        -- Atrasos activos (préstamos aún abiertos y vencidos)
        SUM(CASE
                WHEN p.Estado IN ('Activo','Vencido')
                 AND p.FechaDevolucionEsperada < GETDATE()
                THEN 1 ELSE 0
            END)                AS AtrasosActivos,
        -- Total de préstamos en el período
        COUNT(DISTINCT p.PrestamoId) AS TotalPrestamos,
        -- Días de atraso acumulados
        SUM(CASE
                WHEN p.FechaCierre > p.FechaDevolucionEsperada
                THEN DATEDIFF(DAY, p.FechaDevolucionEsperada, p.FechaCierre)
                WHEN p.Estado IN ('Activo','Vencido')
                 AND p.FechaDevolucionEsperada < GETDATE()
                THEN DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE())
                ELSE 0
            END)                AS DiasAtrasoAcumulados
    FROM   Empleado       e
    INNER  JOIN Prestamo      p ON e.EmpleadoId      = p.EmpleadoId
    LEFT   JOIN Departamento  d ON e.DepartamentoId  = d.DepartamentoId
    WHERE  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR p.FechaPrestamo <= DATEADD(DAY, 1, @FechaHasta))
    GROUP  BY e.EmpleadoId, e.Codigo, e.Nombre, d.Nombre
    HAVING SUM(CASE
                   WHEN p.Estado = 'Cerrado'
                    AND p.FechaCierre > p.FechaDevolucionEsperada
                   THEN 1
                   WHEN p.Estado IN ('Activo','Vencido')
                    AND p.FechaDevolucionEsperada < GETDATE()
                   THEN 1
                   ELSE 0
               END) > 0
    ORDER  BY AtrasosActivos DESC, DiasAtrasoAcumulados DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Una fila por unidad dañada, perdida o en mantenimiento correctivo
CREATE   PROCEDURE sp_Reporte_HerramientasDañadas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.HerramientaId,
        u.UnidadId,
        u.CodigoUnidad            AS CodigoHerramienta,
        h.Nombre                  AS Herramienta,
        u.Estado,
        c.Nombre                  AS Categoria,
        m.NombreMarca             AS Marca,
        lp.FechaPrestamo          AS FechaUltimoPrestamo,
        lp.FechaDevolucionEsperada,
        pd.FechaDevuelta          AS FechaReporteDaño,
        pd.ObservacionDevolucion  AS ObservacionDaño,
        le.Nombre                 AS EmpleadoResponsable,
        ld.Nombre                 AS DepartamentoResponsable,
        CASE WHEN ma.MantenimientoId IS NOT NULL
             THEN 'Sí' ELSE 'No'
        END                       AS EnMantenimiento,
        ma.FechaInicio            AS FechaInicioMantenimiento
    FROM   vw_HerramientaUnidad   u
    INNER  JOIN Herramienta       h  ON h.HerramientaId    = u.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId    = c.CategoriaId
    LEFT   JOIN Marca             m  ON h.MarcaId          = m.MarcaId
    OUTER  APPLY (
        SELECT TOP 1 pd2.PrestamoId,
                     pd2.FechaDevuelta,
                     pd2.ObservacionDevolucion
        FROM   PrestamoDetalle pd2
        WHERE  pd2.UnidadId          = u.UnidadId
          AND  pd2.EstadoDevolucion  IN ('Dañado', 'Perdido')
        ORDER  BY pd2.FechaDevuelta  DESC
    ) pd
    LEFT   JOIN Prestamo      lp ON lp.PrestamoId      = pd.PrestamoId
    LEFT   JOIN Empleado      le ON lp.EmpleadoId      = le.EmpleadoId
    LEFT   JOIN Departamento  ld ON le.DepartamentoId  = ld.DepartamentoId
    LEFT   JOIN Mantenimiento ma ON ma.UnidadId        = u.UnidadId
                                AND ma.FechaFin        IS NULL
    WHERE  h.Activa = 1
      AND (u.Estado IN ('Dañada', 'Perdida')
        OR (u.Estado = 'En Mantenimiento' AND ma.TipoMantenimiento = 'Correctivo'))
    ORDER  BY u.Estado ASC, h.Nombre ASC, u.Numero ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- 34. Reporte de herramientas más prestadas
--     Muestra cuántas veces se ha prestado cada herramienta
--     en un período opcional
-- ============================================================
CREATE   PROCEDURE sp_Reporte_HerramientasMasPrestadas
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL,
    @Top        INT      = 10    -- cuántas mostrar, por defecto las 10 primeras
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Top)
        h.HerramientaId,
        h.Codigo                            AS CodigoHerramienta,
        h.Nombre                            AS Herramienta,
        c.Nombre                            AS Categoria,
        m.NombreMarca                            AS Marca,
        COUNT(pd.PrestamoDetalleId)         AS VecesPrestada,
        AVG(DATEDIFF(DAY,
            p.FechaPrestamo,
            ISNULL(pd.FechaDevuelta, GETDATE())
        ))                                  AS PromedioDiasPrestado,
        SUM(CASE WHEN pd.EstadoDevolucion = 'Dañado'  THEN 1 ELSE 0 END) AS VecesDañada,
        SUM(CASE WHEN pd.EstadoDevolucion = 'Perdido' THEN 1 ELSE 0 END) AS VecesPerdida
    FROM   PrestamoDetalle        pd
    INNER  JOIN Prestamo          p  ON pd.PrestamoId    = p.PrestamoId
    INNER  JOIN Herramienta       h  ON pd.HerramientaId = h.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId  = c.CategoriaId
    LEFT   JOIN Marca             m  ON h.MarcaId        = m.MarcaId
    WHERE  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR p.FechaPrestamo <= DATEADD(DAY, 1, @FechaHasta))
    GROUP  BY
        h.HerramientaId, h.Codigo, h.Nombre,
        c.Nombre, m.NombreMarca
    ORDER  BY VecesPrestada DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- @Estado: 'Activo' (sin cerrar) | 'Cerrado' | NULL
CREATE   PROCEDURE sp_Reporte_HistorialMantenimiento
    @FechaDesde DATE        = NULL,
    @FechaHasta DATE        = NULL,
    @Tipo       VARCHAR(50) = NULL,
    @Estado     VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.MantenimientoId,
           m.HerramientaId,
           h.Nombre             AS Herramienta,
           u.CodigoUnidad,
           m.TipoMantenimiento  AS Tipo,
           m.TipoServicio       AS Servicio,
           COALESCE(e.Nombre, p.Nombre, m.RealizadoPor) AS RealizadoPor,
           m.FechaInicio,
           m.FechaFin,
           m.CostoMateriales,
           m.CostoManoObra,
           m.Costo,
           m.EnGarantia,
           m.Resultado,
           m.Descripcion,
           DATEDIFF(DAY, m.FechaInicio, ISNULL(m.FechaFin, GETDATE())) AS Duracion,
           CASE WHEN m.FechaFin IS NULL THEN 'Activo' ELSE 'Cerrado' END AS Estado
    FROM   Mantenimiento m
    INNER  JOIN Herramienta          h ON h.HerramientaId = m.HerramientaId
    INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = m.UnidadId
    LEFT   JOIN Empleado             e ON e.EmpleadoId    = m.EmpleadoId
    LEFT   JOIN Proveedor            p ON p.ProveedorId   = m.ProveedorId
    WHERE  (@FechaDesde IS NULL OR m.FechaInicio >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR m.FechaInicio <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@Tipo IS NULL OR m.TipoMantenimiento = @Tipo)
      AND  (@Estado IS NULL
            OR (@Estado = 'Activo'  AND m.FechaFin IS NULL)
            OR (@Estado = 'Cerrado' AND m.FechaFin IS NOT NULL))
    ORDER  BY m.FechaInicio DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- @Estado: 'Activo' | 'Cerrado' | 'Vencido' | NULL
CREATE   PROCEDURE sp_Reporte_HistorialPorPrestamo
    @EmpleadoId    INT         = NULL,
    @HerramientaId INT         = NULL,
    @FechaDesde    DATE        = NULL,
    @FechaHasta    DATE        = NULL,
    @Estado        VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Base AS (
        SELECT p.PrestamoId,
               e.Codigo   AS CodigoEmpleado,
               e.Nombre   AS Empleado,
               d.Nombre   AS Departamento,
               p.FechaPrestamo,
               p.FechaDevolucionEsperada,
               p.FechaCierre,
               ap.Nombre  AS AprobadoPor,
               CASE WHEN p.Estado = 'Cerrado' THEN 'Cerrado'
                    WHEN CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE) THEN 'Vencido'
                    ELSE 'Activo' END AS Estado,
               (SELECT STRING_AGG(h.Nombre + N' ' + u.CodigoUnidad, N', ')
                       WITHIN GROUP (ORDER BY h.Nombre, u.Numero)
                FROM   PrestamoDetalle pd
                INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
                INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = pd.UnidadId
                WHERE  pd.PrestamoId = p.PrestamoId) AS Herramientas
        FROM   Prestamo p
        INNER  JOIN Empleado     e  ON e.EmpleadoId     = p.EmpleadoId
        LEFT   JOIN Departamento d  ON d.DepartamentoId = e.DepartamentoId
        LEFT   JOIN Empleado     ap ON ap.EmpleadoId    = p.AprobadoPorId
        WHERE  (@EmpleadoId IS NULL OR p.EmpleadoId = @EmpleadoId)
          AND  (@HerramientaId IS NULL OR EXISTS (SELECT 1 FROM PrestamoDetalle pd
                                                  WHERE pd.PrestamoId = p.PrestamoId
                                                    AND pd.HerramientaId = @HerramientaId))
          AND  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
          AND  (@FechaHasta IS NULL OR p.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))
    )
    SELECT * FROM Base
    WHERE  (@Estado IS NULL OR Estado = @Estado)
    ORDER  BY FechaPrestamo DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- REPORTES
-- ============================================================
CREATE   PROCEDURE sp_Reporte_HistorialPrestamos
    @EmpleadoId    INT          = NULL,
    @HerramientaId INT          = NULL,
    @FechaDesde    DATETIME     = NULL,
    @FechaHasta    DATETIME     = NULL,
    @Estado        VARCHAR(20)  = NULL,
    @DepartamentoId INT         = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.PrestamoId,
        p.FechaPrestamo,
        p.FechaDevolucionEsperada,
        p.FechaCierre,
        p.Estado                        AS EstadoPrestamo,
        e.Codigo                        AS CodigoEmpleado,
        e.Nombre                        AS Empleado,
        d.Nombre                        AS Departamento,
        ap.Nombre                       AS AprobadoPor,
        un.CodigoUnidad                 AS CodigoHerramienta,
        h.Nombre                        AS Herramienta,
        c.Nombre                        AS Categoria,
        m.NombreMarca                   AS Marca,
        pd.EstadoDevolucion,
        pd.FechaDevuelta,
        pd.ObservacionDevolucion,
        DATEDIFF(DAY, p.FechaPrestamo,
            ISNULL(pd.FechaDevuelta, GETDATE()))    AS DiasPrestado,
        CASE
            WHEN pd.FechaDevuelta IS NULL
             AND p.FechaDevolucionEsperada < GETDATE()
            THEN DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE())
            WHEN pd.FechaDevuelta IS NOT NULL
             AND pd.FechaDevuelta > p.FechaDevolucionEsperada
            THEN DATEDIFF(DAY, p.FechaDevolucionEsperada, pd.FechaDevuelta)
            ELSE 0
        END                             AS DiasAtraso
    FROM   Prestamo               p
    INNER  JOIN PrestamoDetalle   pd ON p.PrestamoId      = pd.PrestamoId
    INNER  JOIN Empleado          e  ON p.EmpleadoId      = e.EmpleadoId
    LEFT   JOIN Empleado          ap ON p.AprobadoPorId   = ap.EmpleadoId
    LEFT   JOIN Departamento      d  ON e.DepartamentoId  = d.DepartamentoId
    INNER  JOIN Herramienta       h  ON pd.HerramientaId  = h.HerramientaId
    INNER  JOIN vw_HerramientaUnidad un ON pd.UnidadId    = un.UnidadId
    LEFT   JOIN CategoriaHerramienta c ON h.CategoriaId   = c.CategoriaId
    LEFT   JOIN Marca             m  ON h.MarcaId         = m.MarcaId
    WHERE  (@EmpleadoId     IS NULL OR p.EmpleadoId      = @EmpleadoId)
      AND  (@HerramientaId  IS NULL OR pd.HerramientaId  = @HerramientaId)
      AND  (@FechaDesde     IS NULL OR p.FechaPrestamo   >= @FechaDesde)
      AND  (@FechaHasta     IS NULL OR p.FechaPrestamo   <= DATEADD(DAY, 1, @FechaHasta))
      AND  (@Estado         IS NULL OR p.Estado          = @Estado)
      AND  (@DepartamentoId IS NULL OR e.DepartamentoId  = @DepartamentoId)
    ORDER  BY p.FechaPrestamo DESC, h.Nombre ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- @Mostrar: 'Todas' | 'StockBajo' (< 30 % disponible) | 'SinDisponibles'
CREATE   PROCEDURE sp_Reporte_InventarioCategoria
    @CategoriaId INT         = NULL,
    @Mostrar     VARCHAR(20) = 'Todas'
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Inv AS (
        SELECT h.HerramientaId,
               h.Codigo,
               h.Nombre  AS Herramienta,
               c.Nombre  AS Categoria,
               s.StockTotal         AS Total,
               s.StockDisponible    AS Disponibles,
               s.StockPrestado      AS Prestadas,
               s.StockMantenimiento AS EnMantenimiento,
               s.StockDañado        AS Danadas,
               CAST(CASE WHEN s.StockTotal > 0 THEN 100.0 * s.StockDisponible / s.StockTotal ELSE 0 END
                    AS DECIMAL(5,1)) AS PctDisponible
        FROM   Herramienta h
        INNER  JOIN vw_HerramientaStock  s ON s.HerramientaId = h.HerramientaId
        LEFT   JOIN CategoriaHerramienta c ON c.CategoriaId   = h.CategoriaId
        WHERE  h.Activa = 1
          AND  (@CategoriaId IS NULL OR h.CategoriaId = @CategoriaId)
    )
    SELECT * FROM Inv
    WHERE  ISNULL(@Mostrar, 'Todas') = 'Todas'
       OR  (@Mostrar = 'StockBajo'      AND PctDisponible < 30)
       OR  (@Mostrar = 'SinDisponibles' AND Disponibles = 0)
    ORDER  BY PctDisponible ASC, Herramienta;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Reporte_KPIs
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Hoy  DATE = CAST(GETDATE() AS DATE);
    DECLARE @Ayer DATE = DATEADD(DAY, -1, @Hoy);

    ;WITH Abiertos AS (
        SELECT p.PrestamoId, p.FechaPrestamo, CAST(p.FechaDevolucionEsperada AS DATE) AS Limite
        FROM   Prestamo p
        WHERE  p.Estado <> 'Cerrado'
          AND  EXISTS (SELECT 1 FROM PrestamoDetalle pd
                       WHERE pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL)
    ),
    Unidades AS (
        SELECT u.Estado
        FROM   HerramientaUnidad u
        INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
        WHERE  h.Activa = 1
    )
    SELECT
        (SELECT COUNT(*) FROM Abiertos WHERE Limite <  @Hoy)                    AS Vencidos,
        (SELECT COUNT(*) FROM Abiertos WHERE Limite =  @Ayer)                   AS VencidosHoy,
        (SELECT COUNT(*) FROM Abiertos)                                         AS Activos,
        (SELECT COUNT(*) FROM Abiertos WHERE CAST(FechaPrestamo AS DATE) = @Hoy) AS ActivosHoy,
        (SELECT COUNT(*) FROM Unidades WHERE Estado = 'Disponible')             AS Disponibles,
        (SELECT COUNT(*) FROM PrestamoDetalle
         WHERE  CAST(FechaDevuelta AS DATE) = @Hoy AND EstadoDevolucion = 'Bueno') AS DevueltasHoy,
        (SELECT COUNT(*) FROM Unidades WHERE Estado = 'En Mantenimiento')       AS EnMantenimiento,
        (SELECT COUNT(*) FROM Mantenimiento WHERE CAST(FechaInicio AS DATE) = @Hoy) AS MantenimientoHoy,
        (SELECT COUNT(*) FROM Unidades WHERE Estado = 'Dañada')                 AS Danadas,
        (SELECT COUNT(*) FROM PrestamoDetalle
         WHERE  CAST(FechaDevuelta AS DATE) = @Hoy AND EstadoDevolucion = 'Dañado') AS DanadasHoy;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Reporte_PrestamosPorEmpleado
    @FechaDesde     DATE = NULL,
    @FechaHasta     DATE = NULL,
    @DepartamentoId INT  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.EmpleadoId,
           e.Codigo AS CodigoEmpleado,
           e.Nombre AS Empleado,
           d.Nombre AS Departamento,
           COUNT(*) AS TotalPrestamos,
           SUM(CASE WHEN p.Estado <> 'Cerrado' THEN 1 ELSE 0 END) AS Activos,
           SUM(CASE WHEN p.Estado <> 'Cerrado'
                     AND CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE)
                    THEN 1 ELSE 0 END) AS Vencidos,
           SUM(CASE WHEN p.Estado = 'Cerrado' THEN 1 ELSE 0 END) AS Devueltos,
           (SELECT COUNT(DISTINCT pd.HerramientaId)
            FROM   PrestamoDetalle pd
            INNER  JOIN Prestamo p2 ON p2.PrestamoId = pd.PrestamoId
            WHERE  p2.EmpleadoId = e.EmpleadoId
              AND  (@FechaDesde IS NULL OR p2.FechaPrestamo >= @FechaDesde)
              AND  (@FechaHasta IS NULL OR p2.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))) AS HerramientasUnicas
    FROM   Prestamo p
    INNER  JOIN Empleado     e ON e.EmpleadoId     = p.EmpleadoId
    LEFT   JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    WHERE  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR p.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@DepartamentoId IS NULL OR e.DepartamentoId = @DepartamentoId)
    GROUP  BY e.EmpleadoId, e.Codigo, e.Nombre, d.Nombre
    ORDER  BY TotalPrestamos DESC, e.Nombre;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Reporte_PrestamosVencidos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.PrestamoId,
        p.FechaPrestamo,
        p.FechaDevolucionEsperada,
        DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE()) AS DiasAtraso,
        e.Codigo    AS CodigoEmpleado,
        e.Nombre    AS Empleado,
        d.Nombre    AS Departamento,
        (
            SELECT STRING_AGG(h2.Nombre + N' (' + u2.CodigoUnidad + N')', ', ')
            FROM   PrestamoDetalle pd2
            INNER  JOIN Herramienta          h2 ON pd2.HerramientaId = h2.HerramientaId
            INNER  JOIN vw_HerramientaUnidad u2 ON pd2.UnidadId      = u2.UnidadId
            WHERE  pd2.PrestamoId   = p.PrestamoId
              AND  pd2.FechaDevuelta IS NULL
        )           AS HerramientasPendientes,
        (
            SELECT COUNT(*)
            FROM   PrestamoDetalle pd3
            WHERE  pd3.PrestamoId   = p.PrestamoId
              AND  pd3.FechaDevuelta IS NULL
        )           AS CantidadPendiente
    FROM   Prestamo      p
    INNER  JOIN Empleado     e ON p.EmpleadoId     = e.EmpleadoId
    LEFT   JOIN Departamento d ON e.DepartamentoId = d.DepartamentoId
    WHERE  p.Estado IN ('Activo', 'Vencido')
      AND  p.FechaDevolucionEsperada < GETDATE()
      AND  EXISTS (
               SELECT 1 FROM PrestamoDetalle pd
               WHERE  pd.PrestamoId   = p.PrestamoId
                 AND  pd.FechaDevuelta IS NULL
           )
    ORDER  BY DiasAtraso DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Reporte_RankingHerramientas
    @FechaDesde  DATE = NULL,
    @FechaHasta  DATE = NULL,
    @CategoriaId INT  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT h.HerramientaId,
           h.Codigo,
           h.Nombre  AS Herramienta,
           c.Nombre  AS Categoria,
           ISNULL(s.StockTotal, 0) AS TotalUnidades,
           COUNT(DISTINCT p.PrestamoId) AS TotalPrestamos,
           -- días por unidad prestada (hasta su devolución, o hasta hoy si sigue prestada)
           CAST(AVG(CAST(DATEDIFF(DAY, p.FechaPrestamo, ISNULL(pd.FechaDevuelta, GETDATE())) AS FLOAT))
                AS DECIMAL(8,1)) AS DiasPromedio
    FROM   PrestamoDetalle pd
    INNER  JOIN Prestamo             p ON p.PrestamoId    = pd.PrestamoId
    INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
    LEFT   JOIN CategoriaHerramienta c ON c.CategoriaId   = h.CategoriaId
    LEFT   JOIN vw_HerramientaStock  s ON s.HerramientaId = h.HerramientaId
    WHERE  (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND  (@FechaHasta IS NULL OR p.FechaPrestamo <  DATEADD(DAY, 1, @FechaHasta))
      AND  (@CategoriaId IS NULL OR h.CategoriaId = @CategoriaId)
    GROUP  BY h.HerramientaId, h.Codigo, h.Nombre, c.Nombre, s.StockTotal
    ORDER  BY TotalPrestamos DESC, DiasPromedio DESC, h.Nombre;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Reporte_Vencidos
    @EmpleadoId  INT = NULL,
    @CategoriaId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PrestamoId,
           e.Codigo   AS CodigoEmpleado,
           e.Nombre   AS Empleado,
           d.Nombre   AS Departamento,
           p.FechaDevolucionEsperada,
           DATEDIFF(DAY, CAST(p.FechaDevolucionEsperada AS DATE), CAST(GETDATE() AS DATE)) AS DiasVencido,
           (SELECT STRING_AGG(h.Nombre + N' ' + u.CodigoUnidad, N', ')
                   WITHIN GROUP (ORDER BY h.Nombre, u.Numero)
            FROM   PrestamoDetalle pd
            INNER  JOIN Herramienta          h ON h.HerramientaId = pd.HerramientaId
            INNER  JOIN vw_HerramientaUnidad u ON u.UnidadId      = pd.UnidadId
            WHERE  pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL) AS Herramientas
    FROM   Prestamo p
    INNER  JOIN Empleado     e ON e.EmpleadoId     = p.EmpleadoId
    LEFT   JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
    WHERE  p.Estado <> 'Cerrado'
      AND  CAST(p.FechaDevolucionEsperada AS DATE) < CAST(GETDATE() AS DATE)
      AND  EXISTS (SELECT 1 FROM PrestamoDetalle pd
                   WHERE pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL)
      AND  (@EmpleadoId IS NULL OR p.EmpleadoId = @EmpleadoId)
      AND  (@CategoriaId IS NULL OR EXISTS (
               SELECT 1 FROM PrestamoDetalle pd
               INNER  JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
               WHERE  pd.PrestamoId = p.PrestamoId AND pd.FechaDevuelta IS NULL
                 AND  h.CategoriaId = @CategoriaId))
    ORDER  BY DiasVencido DESC, p.PrestamoId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Roles ────────────────────────────────────────────────────
CREATE   PROCEDURE sp_Rol_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT RolId, Nombre, Descripcion, AdministraUsuarios
    FROM   Rol
    ORDER  BY AdministraUsuarios, Nombre;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 9. Actualizar ubicación
CREATE   PROCEDURE sp_Ubicacion_Actualizar
    @UbicacionId INT,
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Ubicacion WHERE UbicacionId = @UbicacionId)
    BEGIN
        RAISERROR('La ubicación especificada no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = @Nombre AND UbicacionId <> @UbicacionId)
    BEGIN
        RAISERROR('Ya existe otra ubicación con ese nombre.', 16, 1);
        RETURN;
    END

    UPDATE Ubicacion
    SET    Nombre      = @Nombre,
           Descripcion = @Descripcion
    WHERE  UbicacionId = @UbicacionId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Ubicacion_Eliminar
    @UbicacionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Herramienta SET UbicacionId = NULL WHERE UbicacionId = @UbicacionId;
        DELETE FROM Ubicacion WHERE UbicacionId = @UbicacionId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 8. Insertar ubicación
CREATE   PROCEDURE sp_Ubicacion_Insertar
    @Nombre      VARCHAR(120),
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Ubicacion WHERE Nombre = @Nombre)
    BEGIN
        RAISERROR('Ya existe una ubicación con ese nombre.', 16, 1);
        RETURN;
    END

    INSERT INTO Ubicacion (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);

    SELECT SCOPE_IDENTITY() AS UbicacionId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Ubicacion_ObtenerTodas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UbicacionId,
           u.Nombre,
           u.Descripcion,
           (SELECT COUNT(*) FROM Herramienta h
            WHERE  h.UbicacionId = u.UbicacionId AND h.Activa = 1) AS Herramientas
    FROM   Ubicacion u
    ORDER  BY u.Nombre ASC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Administración de usuarios ───────────────────────────────
CREATE   PROCEDURE sp_Usuario_ValidarAdministrador
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Guarda un hash nuevo y elimina la contraseña en texto plano.
-- Lo usan: la migración al iniciar sesión, "Cambiar mi contraseña" y el restablecimiento.
CREATE   PROCEDURE sp_Usuario_GuardarPassword
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Usuario_Insertar
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ── Login ────────────────────────────────────────────────────
CREATE   PROCEDURE sp_Usuario_ObtenerParaLogin
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE sp_Usuario_ObtenerTodos
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Restablece con una contraseña temporal (olvido de contraseña)
CREATE   PROCEDURE sp_Usuario_RestablecerPassword
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

SET NOEXEC OFF;
GO
