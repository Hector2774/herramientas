# PROMACO · Sistema de Control de Herramientas

Aplicación de escritorio para controlar el inventario de herramientas de PROMACO: qué herramientas
hay, dónde están, quién las tiene prestadas, en qué estado regresan y cuánto cuesta mantenerlas.

## El problema que resuelve

Las herramientas de la empresa se prestan a diario a empleados de distintos departamentos. Sin un
registro, no se sabe quién tiene cada pieza, cuáles están vencidas, cuáles regresaron dañadas ni cuánto
se gasta en repararlas, y las pérdidas no tienen responsable. El sistema lleva ese control **por unidad
física** (cada pieza tiene su propio código, por ejemplo `HER-0001-03`) y deja registro de quién aprobó
cada préstamo.

## Funcionalidades

| Módulo | Qué hace |
|---|---|
| **Inicio** | Dashboard: préstamos activos y vencidos, unidades disponibles, en mantenimiento y dañadas, actividad mensual y préstamos por departamento. |
| **Herramientas** | Catálogo con foto, categoría, marca y ubicación; stock por unidad y su estado. Catálogos de categorías, marcas, ubicaciones y proveedores. Bandeja de mantenimiento: activos y unidades dañadas pendientes de reparar. |
| **Empleados** | Lista de empleados sincronizada desde el sistema de RRHH. |
| **Préstamos** | Catálogo de herramientas disponibles; se eligen unidades concretas y la fecha de devolución. |
| **Devoluciones** | Devolución por unidad con su condición (Bueno, Dañado o Perdido) y nota; se puede devolver parte de un préstamo. |
| **Reportes** | Historial de préstamos, vencidos, préstamos por empleado, herramientas más usadas, historial y costos de mantenimiento, dañadas y perdidas, inventario por categoría. Exportación a **PDF** y **Excel**. |
| **Usuarios** | Alta y edición de usuarios, roles, restablecimiento de contraseña, activar y desactivar (solo administradores). |
| **Mi contraseña** | Cada usuario cambia su propia contraseña. |

### Reglas de negocio principales

- **Stock por unidad.** Cada pieza tiene su estado: Disponible, Prestada, En Mantenimiento, Dañada, Perdida o Baja. Una pieza dañada no bloquea a las demás del mismo tipo.
- **Préstamos.** Nadie puede aprobar su propio préstamo: el aprobador es el empleado del usuario que inició sesión. La fecha de devolución debe ser posterior a hoy, y los préstamos fuera de plazo se marcan como vencidos.
- **Devoluciones.** Las unidades dañadas no vuelven al stock hasta pasar por mantenimiento; las perdidas se descuentan. El préstamo se cierra solo cuando no le quedan unidades pendientes.
- **Mantenimiento.** Preventivo, correctivo o calibración; interno (técnico de la empresa) o externo (proveedor). Se registra lo que cuesta a la empresa en quetzales (Q): materiales, más mano de obra solo si es externo. En garantía no hay costo. Una unidad irreparable se da de baja.
- **Nada se borra.** Herramientas, unidades y usuarios se dan de baja o se desactivan, para conservar el historial.

### Seguridad

- Contraseñas guardadas como hash **PBKDF2-SHA256** con sal por usuario; nunca en texto plano.
- Contraseñas temporales (usuario nuevo o restablecido) que se deben cambiar en el primer ingreso.
- Roles en tabla propia (Administrador, Operador). Solo los roles que administran usuarios ven el módulo Usuarios, y los procedimientos almacenados también lo validan.
- Todo el acceso a datos pasa por procedimientos almacenados con parámetros (sin SQL concatenado).

## Tecnología

- **C# / .NET 10**, Windows Forms, con controles propios de estilo Material.
- **SQL Server** (desarrollado en SQL Server 2022), lógica de negocio en procedimientos almacenados con transacciones.
- Librerías: Microsoft.Data.SqlClient, FontAwesome.Sharp (íconos), ClosedXML (Excel), iText (PDF).

### Arquitectura

```
Formularios (Frm*.cs, Controls/)   interfaz y validaciones de pantalla
        │
Servicios (Services/)              reglas de la aplicación, hash de contraseñas, sesión
        │
Acceso a datos (Data/Db.cs)        conexión y ejecución de procedimientos almacenados
        │
SQL Server                         tablas, vistas y procedimientos almacenados
```

El modelo de datos y sus decisiones de diseño están en [`docs/diagrama-er.md`](docs/diagrama-er.md).

## Instalación

### Requisitos

- Windows 10 u 11
- [SDK de .NET 10](https://dotnet.microsoft.com/download) (o Visual Studio con la carga de trabajo de escritorio .NET)
- SQL Server 2019 o posterior (la edición Express o Developer sirve), con autenticación de Windows
- `sqlcmd` o SQL Server Management Studio para ejecutar los scripts

### 1. Crear la base de datos

```sql
CREATE DATABASE PROMACO_Herramientas COLLATE Modern_Spanish_CI_AS;
```

Luego ejecutar los dos scripts de `Database/instalacion/` **sobre esa base**, en orden:

```bash
sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i Database/instalacion/000_esquema.sql
sqlcmd -S localhost -E -C -I -f 65001 -b -d PROMACO_Herramientas -i Database/instalacion/001_datos_iniciales.sql
```

En SSMS: abrir cada script, seleccionar la base `PROMACO_Herramientas` y ejecutar.

> `Database/migraciones/` guarda la historia de cambios de la base (scripts 002 a 017). Una instalación
> nueva **no** los necesita: ya están incluidos en `000_esquema.sql`.

### 2. Configurar la conexión

La aplicación se conecta a `localhost` con autenticación de Windows. Si SQL Server está en otra
instancia (por ejemplo `localhost\SQLEXPRESS`), cambiar la cadena de conexión en
[`PromacoHerra/Data/Db.cs`](PromacoHerra/Data/Db.cs).

### 3. Compilar y ejecutar

```bash
dotnet run --project PromacoHerra/PromacoHerra.csproj
```

O abrir `PromacoHerra.slnx` en Visual Studio y presionar F5.

### 4. Primer ingreso

| Usuario | Contraseña inicial |
|---|---|
| `admin` | `Promaco2026` |

Es una contraseña temporal: el sistema pide crear una nueva antes de entrar. Desde **Usuarios** se crean
las cuentas del resto del personal, cada una ligada a un empleado.

### Notas

- **Empleados.** Se cargan desde la API de RRHH con *Sincronizar empleados* (pantalla Empleados). La
  dirección de la API está en `PromacoHerra/Services/EmpleadoService.cs` y solo responde dentro de la
  red de la empresa.
- **Fotos de herramientas.** Se guardan en la carpeta `Fotos\` junto al ejecutable y no forman parte del
  repositorio. Sin fotos, el sistema muestra un ícono genérico; se agregan desde Herramientas → *Cambiar foto*.

## Estructura del repositorio

```
PromacoHerra/          código de la aplicación
├── Frm*.cs            pantallas
├── Controls/          controles visuales reutilizables
├── Services/          lógica de la aplicación
├── Data/Db.cs         acceso a datos
├── Models/            clases de datos
└── recursos/          logo
Database/
├── instalacion/       scripts para instalar desde cero
└── migraciones/       historial de cambios de la base
docs/                  diagrama entidad-relación y modelo de datos
```
