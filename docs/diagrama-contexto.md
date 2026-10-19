# Diagrama de contexto (nivel 0)

**Sistema de Control de Herramientas · PROMACO**

El diagrama de contexto muestra el sistema completo como **un solo proceso** y las **entidades externas**
que intercambian información con él. No muestra cómo funciona por dentro: eso corresponde a los
[diagrama de flujo de datos de nivel 1](diagrama-nivel1.md) y a los [diagramas de actividad](diagramas-actividad.md).

Versión en imagen (para documentos impresos o presentaciones): [`diagrama-contexto.png`](diagrama-contexto.png).

Notación:

- Círculo: el sistema, como proceso único (proceso 0).
- Rectángulo: entidad externa (persona, área o sistema que está fuera del alcance del sistema).
- Flecha: flujo de datos; la etiqueta dice qué información viaja y la punta indica hacia dónde.

```mermaid
flowchart LR
    OP["Operador<br/>(encargado de bodega)"]
    AD["Administrador"]
    RH["Web service de empleados"]
    GE["Gerencia"]

    SYS(("0<br/>Sistema de Control<br/>de Herramientas<br/>PROMACO"))

    OP -- "Credenciales y nueva contraseña<br/>Orden de sincronizar empleados<br/>Herramientas, piezas y catálogos<br/>Préstamos (empleado, piezas, fecha de devolución)<br/>Devoluciones (condición y nota)<br/>Mantenimientos (tipo, resultado, costo en L.)<br/>Criterios de reportes" --> SYS
    SYS -- "Resultado del inicio de sesión<br/>Lista de empleados y resumen de sincronización<br/>Panel de inicio y alertas de vencidos<br/>Inventario y disponibilidad por pieza<br/>Confirmaciones de préstamo y devolución<br/>Historial de mantenimiento<br/>Reportes en pantalla" --> OP

    AD -- "Credenciales y nueva contraseña<br/>Datos de usuarios y roles<br/>Restablecer contraseña<br/>Activar / desactivar usuarios" --> SYS
    SYS -- "Resultado del inicio de sesión<br/>Lista de usuarios y roles<br/>Contraseñas temporales" --> AD

    SYS -- "Solicitud de sincronización" --> RH
    RH -- "Lista de empleados<br/>(código, nombre, departamento, activo)" --> SYS

    SYS -- "Reportes en PDF y Excel<br/>(préstamos, vencidos, uso, mantenimiento,<br/>daños, pérdidas, inventario)" --> GE

    classDef entidad fill:#DDEBF7,stroke:#1F4E78,stroke-width:1.5px,color:#000
    classDef sistema fill:#1F4E78,stroke:#1F4E78,color:#fff,font-weight:bold
    class OP,AD,RH,GE entidad
    class SYS sistema
```

## Entidades externas

| Entidad | Qué le entrega al sistema | Qué recibe del sistema |
|---|---|---|
| **Operador** (encargado de bodega) | Credenciales y cambio de contraseña; orden de sincronizar empleados; registro de herramientas, piezas y catálogos (categorías, marcas, ubicaciones, proveedores); préstamos; devoluciones con su condición (Bueno, Dañado, Perdido); mantenimientos con su costo en lempiras; filtros de reportes. | Resultado del inicio de sesión, lista de empleados con el resumen de la sincronización, panel de inicio, alertas de préstamos vencidos, inventario y estado de cada pieza, confirmaciones, historial de mantenimiento y reportes. |
| **Administrador** | Lo mismo que el Operador y, además, los datos de usuarios, roles, restablecimientos de contraseña y activaciones. | Lo mismo que el Operador y, además, la lista de usuarios y las contraseñas temporales. |
| **Web service de empleados** | La lista de empleados de la empresa (por eso no se registran a mano). | La solicitud de sincronización. |
| **Gerencia** | — | Reportes exportados a PDF y Excel para tomar decisiones de compra, reposición y control de gastos. |

## Lo que queda fuera del diagrama, y por qué

- **Empleados que piden herramientas.** No usan el sistema: piden la herramienta en bodega y el
  Operador registra el préstamo a su nombre. Por eso sus datos entran a través del Operador y del
  Web service de empleados, no como una entidad aparte.
- **Proveedores de mantenimiento.** Tampoco usan el sistema: entregan la factura y el Operador registra
  el mantenimiento externo con su costo.
- **Base de datos (SQL Server).** Es parte del sistema, no una entidad externa; aparece en el
  [diagrama de nivel 1](diagrama-nivel1.md) dividida en almacenes de datos.

Para simplificar el diagrama, el Administrador aparece solo con los flujos que lo distinguen del
Operador; también puede realizar todo lo que hace el Operador.
