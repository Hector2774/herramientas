# Modelo de datos

Base de datos **PROMACO_Herramientas** (SQL Server, intercalación `Modern_Spanish_CI_AS`).
El script completo está en [`Database/instalacion/000_esquema.sql`](../Database/instalacion/000_esquema.sql).

## Diagrama entidad-relación

Versión en imagen (para documentos impresos o presentaciones): [`diagrama-er.png`](diagrama-er.png).

```mermaid
erDiagram
    Departamento ||--o{ Empleado : "agrupa"
    Empleado ||--o| Usuario : "tiene cuenta"
    Rol ||--o{ Usuario : "define permisos"

    CategoriaHerramienta |o--o{ Herramienta : "clasifica"
    Marca |o--o{ Herramienta : "fabrica"
    Ubicacion |o--o{ Herramienta : "almacena"
    Herramienta ||--o{ HerramientaUnidad : "tiene unidades"

    Empleado ||--o{ Prestamo : "solicita"
    Empleado |o--o{ Prestamo : "aprueba"
    Prestamo ||--|{ PrestamoDetalle : "incluye"
    HerramientaUnidad ||--o{ PrestamoDetalle : "se presta en"

    HerramientaUnidad ||--o{ Mantenimiento : "recibe"
    PrestamoDetalle |o--o{ Mantenimiento : "origina (daño)"
    Proveedor |o--o{ Mantenimiento : "repara (externo)"
    Empleado |o--o{ Mantenimiento : "repara (interno)"
    Usuario |o--o{ Mantenimiento : "registra / cierra"

    Departamento {
        int DepartamentoId PK
        varchar Nombre
        tinyint Activo
    }
    Empleado {
        int EmpleadoId PK
        nvarchar Codigo UK "código de RRHH"
        nvarchar Nombre
        bit Activo
        int DepartamentoId FK
    }
    Rol {
        int RolId PK
        varchar Nombre UK
        varchar Descripcion
        bit AdministraUsuarios "permiso del rol"
    }
    Usuario {
        int UsuarioId PK
        nvarchar Username UK
        varchar PasswordHash "PBKDF2-SHA256"
        bit DebeCambiarPassword "contraseña temporal"
        bit Activo
        int EmpleadoId FK,UK "un usuario por empleado"
        int RolId FK
    }
    CategoriaHerramienta {
        int CategoriaId PK
        varchar Nombre UK
        varchar Descripcion
    }
    Marca {
        int MarcaId PK
        varchar NombreMarca
        varchar Descripcion
    }
    Ubicacion {
        int UbicacionId PK
        varchar Nombre
        varchar Descripcion
    }
    Herramienta {
        int HerramientaId PK
        varchar Codigo UK "HER-0001, automático"
        nvarchar Nombre
        nvarchar Caracteristicas
        bit Activa "0 = dada de baja"
        bit PrestamoHabilitado "0 = préstamo suspendido"
        nvarchar FotoNombre "archivo en Fotos"
        int CategoriaId FK
        int MarcaId FK
        int UbicacionId FK
    }
    HerramientaUnidad {
        int UnidadId PK
        int HerramientaId FK
        int Numero "HER-0001-03"
        varchar Estado "Disponible, Prestada, En Mantenimiento, Dañada, Perdida, Baja"
        datetime2 FechaAlta
        datetime2 FechaBaja
        nvarchar Observaciones
    }
    Prestamo {
        int PrestamoId PK
        int EmpleadoId FK "quien recibe"
        int AprobadoPorId FK "empleado del usuario en sesión"
        datetime2 FechaPrestamo
        datetime FechaDevolucionEsperada
        datetime2 FechaCierre
        varchar Estado "Activo, Vencido, Cerrado"
        nvarchar Observaciones
    }
    PrestamoDetalle {
        int PrestamoDetalleId PK
        int PrestamoId FK
        int UnidadId FK
        int HerramientaId FK
        datetime2 FechaDevuelta "NULL = pendiente"
        varchar EstadoDevolucion "Pendiente, Bueno, Dañado, Perdido"
        nvarchar ObservacionDevolucion
    }
    Proveedor {
        int ProveedorId PK
        varchar Nombre
        varchar Telefono
        varchar Descripcion
        bit Activo
    }
    Mantenimiento {
        int MantenimientoId PK
        int UnidadId FK
        int HerramientaId FK
        varchar TipoMantenimiento "Preventivo, Correctivo, Calibración"
        varchar TipoServicio "Interno o Externo"
        int EmpleadoId FK "técnico interno"
        int ProveedorId FK "taller externo"
        int PrestamoDetalleId FK "devolución que lo originó"
        datetime FechaInicio
        datetime FechaFin
        decimal CostoMateriales "L."
        decimal CostoManoObra "L., solo externo"
        decimal Costo "total en L."
        bit EnGarantia
        varchar FolioFactura
        varchar Resultado "Reparada o Baja"
        int RegistradoPorId FK
        int CerradoPorId FK
    }
    SincronizacionEmpleados {
        int SincronizacionId PK
        datetime2 Fecha
        int Nuevos
        int Actualizados
        int TotalActivos
    }
```

## Decisiones de diseño

- **Herramienta vs. unidad.** `Herramienta` es el tipo (catálogo) y `HerramientaUnidad` cada pieza física
  con su propio estado. El stock no se guarda: se calcula contando unidades (vista `vw_HerramientaStock`).
  Así una pieza dañada no bloquea a las demás del mismo tipo.
- **Préstamo por unidad.** Cada línea de `PrestamoDetalle` apunta a una unidad concreta y se devuelve por
  separado, con su condición. El préstamo se cierra solo cuando no quedan unidades pendientes.
  `HerramientaId` acompaña a `UnidadId` en una llave foránea compuesta que garantiza que la unidad
  pertenece a esa herramienta.
- **Trazabilidad del aprobador.** `Prestamo.AprobadoPorId` guarda al empleado que aprobó en ese
  momento, no al usuario: si la cuenta cambia después, el historial no se altera.
- **Roles normalizados.** El permiso de administrar usuarios es un dato del rol (`Rol.AdministraUsuarios`),
  no su nombre.
- **Contraseñas.** Solo se guarda el hash PBKDF2-SHA256 con sal (`Usuario.PasswordHash`). La columna
  `Usuario.Password` es de la versión anterior: queda vacía en cuanto cada usuario inicia sesión.
- **Nada se borra.** Herramientas, unidades y usuarios se dan de baja o se desactivan; préstamos y
  mantenimientos los siguen referenciando.
