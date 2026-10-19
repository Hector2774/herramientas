# Descripción general del proyecto

**Sistema de Control de Herramientas · PROMACO**

## 1. Contexto

PROMACO es una empresa hondureña que cuenta con herramientas y equipos menores para uso interno
(taladros, esmeriles, llaves, equipo de medición, entre otros). Estas herramientas no se venden: se
prestan a los empleados de distintos departamentos (bodegas, taller, operaciones) para realizar
trabajos específicos y, al terminar, deben devolverse.

## 2. Situación anterior

El control de estos préstamos se llevaba en hojas de papel o en archivos de Excel que no estaban
centralizados. Cada encargado llevaba su propio registro, en su propio formato y en su propio equipo o
cuaderno. Esto provocaba:

- **Pérdida de información.** Hojas extraviadas, archivos sobrescritos o desactualizados, y registros
  que dependían de una sola persona.
- **Desconocimiento del estado y la ubicación de cada herramienta.** No se sabía con certeza qué
  herramientas estaban disponibles, cuáles estaban prestadas, a quién, ni dónde se encontraban.
- **Falta de control sobre los vencimientos.** No había forma sencilla de saber qué préstamos ya
  debían haberse devuelto.
- **Herramientas dañadas que volvían a circular.** Una herramienta podía regresar dañada, volver al
  estante y ser prestada de nuevo a otro empleado sin que nadie lo notara.
- **Pérdidas sin responsable.** Cuando una herramienta desaparecía, no había un registro confiable de
  quién la tuvo por última vez ni de quién autorizó el préstamo.
- **Gastos de reparación invisibles.** No se sabía cuánto dinero se gastaba en reparar herramientas ni
  cuáles fallaban con más frecuencia.
- **Información poco útil para decidir.** La gerencia no contaba con datos consolidados para decidir
  qué herramientas reponer, cuáles comprar en mayor cantidad o qué empleados acumulaban atrasos o daños.

Ante esa necesidad surgió la idea de crear este sistema.

## 3. Propósito

Desarrollar un sistema que centralice el control de las herramientas de uso interno de PROMACO, de modo
que en un solo lugar se pueda saber **qué herramientas hay, dónde están, quién las tiene, en qué estado
regresan y cuánto cuesta mantenerlas**, dejando un historial confiable de cada movimiento y de la
persona responsable.

### Objetivos específicos

1. Llevar un inventario centralizado de las herramientas, controlado por cada pieza física y no solo
   por tipo de herramienta.
2. Registrar los préstamos indicando qué empleado se lleva cada herramienta, quién lo autoriza y en qué
   fecha debe devolverla.
3. Registrar las devoluciones indicando en qué condición regresa cada pieza (buena, dañada o perdida).
4. Impedir que una herramienta dañada vuelva a prestarse antes de ser reparada.
5. Dar seguimiento al mantenimiento de las herramientas y a su costo.
6. Ofrecer una vista rápida del estado de la operación y reportes que apoyen la toma de decisiones.
7. Restringir el acceso al sistema a usuarios autorizados, cada uno con su rol.

## 4. Problemas que resuelve

| Problema anterior | Cómo lo resuelve el sistema |
|---|---|
| Registros en papel o Excel dispersos y que se pierden | Toda la información queda en un solo sistema, compartido por los encargados. |
| No se sabía dónde estaba ni en qué estado se encontraba cada herramienta | Cada pieza tiene un código propio, una ubicación y un estado actual (disponible, prestada, en mantenimiento, dañada, perdida o dada de baja). |
| No se sabía quién tenía cada herramienta | Cada préstamo registra al empleado que la recibe y al empleado que lo aprueba. |
| Préstamos vencidos sin seguimiento | El sistema marca automáticamente los préstamos que pasaron su fecha de devolución y los muestra al iniciar. |
| Herramientas dañadas que se volvían a prestar | Una pieza devuelta como dañada queda bloqueada hasta pasar por mantenimiento. |
| Pérdidas sin responsable | Queda constancia de quién tenía la pieza y quién autorizó el préstamo. |
| No se conocía el gasto en reparaciones | Cada mantenimiento registra su tipo, quién lo realizó y su costo en lempiras. |
| Falta de información para la gerencia | Reportes de préstamos, vencidos, uso por empleado, herramientas más usadas, mantenimiento, daños, pérdidas e inventario, exportables a PDF y Excel. |

## 5. Cómo funciona (lógica del proceso)

### 5.1 Inventario por pieza

Una herramienta (por ejemplo, "Taladro percutor") puede tener varias piezas físicas iguales. El sistema
controla **cada pieza por separado**, con su propio código y estado. Así, si una pieza se daña, las demás
del mismo tipo siguen disponibles para préstamo, y siempre se sabe exactamente cuál pieza tiene cada
persona. La cantidad disponible no se anota a mano: el sistema la obtiene contando las piezas en cada
estado.

Las herramientas se organizan por categoría, marca, ubicación y proveedor, e incluyen fotografía para
identificarlas con facilidad. Si por alguna razón no se quiere prestar un tipo de herramienta durante un
tiempo, se puede suspender su préstamo sin darla de baja.

### 5.2 Préstamo

1. El encargado selecciona al empleado que solicita las herramientas.
2. Elige las herramientas y las piezas concretas que se entregan.
3. Indica la fecha en que deben devolverse, que siempre debe ser posterior al día del préstamo.
4. El préstamo queda aprobado a nombre del encargado que tiene la sesión abierta.

**Regla importante:** nadie puede aprobar su propio préstamo. Esto evita que una persona se entregue
herramientas a sí misma sin control.

### 5.3 Devolución

Al recibir las herramientas, el encargado indica para cada pieza en qué condición regresa:

- **Buena:** la pieza vuelve a estar disponible.
- **Dañada:** la pieza queda apartada y pasa a la lista de pendientes de mantenimiento; no puede
  prestarse de nuevo hasta ser reparada.
- **Perdida:** la pieza se descuenta del inventario y queda registrado quién la tenía.

Se puede agregar una nota (por ejemplo, "cable pelado"). Un préstamo puede devolverse por partes, y se
considera cerrado únicamente cuando se han devuelto todas sus piezas.

### 5.4 Mantenimiento

Las herramientas pueden recibir mantenimiento **preventivo**, **correctivo** o de **calibración**. Puede
realizarlo un técnico de la empresa (**interno**) o un proveedor (**externo**).

Al terminar se registra el resultado y lo que le costó a la empresa, en lempiras:

- Si es interno, se registra solo el costo de los materiales, porque la mano de obra ya la cubre el
  salario del técnico.
- Si es externo, se registran materiales y mano de obra, junto con la factura del proveedor.
- Si la herramienta está en garantía, el costo es cero.
- Si la pieza no tiene reparación, se da de baja.

De cada herramienta se puede consultar su historial de mantenimiento: cuántas veces se ha reparado,
cuánto ha costado en total y qué empleado la devolvió dañada.

### 5.5 Panel de inicio

Al ingresar, el sistema muestra un resumen de la operación: préstamos activos y vencidos, piezas
disponibles, en mantenimiento y dañadas, la actividad reciente de préstamos y devoluciones, los
préstamos por departamento, las próximas devoluciones y las alertas pendientes. De esta forma el
encargado sabe en pocos segundos qué debe atender ese día.

### 5.6 Reportes

El sistema genera reportes de historial de préstamos, préstamos vencidos, préstamos por empleado,
herramientas más usadas, historial y costos de mantenimiento, herramientas dañadas y perdidas, e
inventario por categoría. Todos pueden exportarse a PDF y Excel para presentarlos a la gerencia.

### 5.7 Empleados

La lista de empleados se obtiene del sistema de Recursos Humanos de la empresa, por lo que no es
necesario registrarlos a mano y se mantiene actualizada.

### 5.8 Usuarios y roles

Solo pueden ingresar al sistema los usuarios autorizados, cada uno ligado a un empleado de la empresa.
Existen dos roles:

- **Administrador:** usa todo el sistema y además administra los usuarios (crearlos, asignar roles,
  restablecer contraseñas, activarlos o desactivarlos).
- **Operador:** registra préstamos, devoluciones, mantenimiento y consulta reportes, pero no administra
  usuarios.

Los usuarios nuevos reciben una contraseña temporal que deben cambiar en su primer ingreso, y cada
usuario puede cambiar su propia contraseña.

### 5.9 Conservación del historial

En el sistema **nada se borra**. Las herramientas, piezas y usuarios que ya no se usan se dan de baja o
se desactivan, pero su historial se conserva. Lo mismo pasa con las categorías, marcas, ubicaciones y
proveedores que ya se usaron: se desactivan y las herramientas conservan su clasificación, para que los
reportes de períodos pasados no cambien. Solo se borra un registro de catálogo que nadie llegó a usar. Así, un préstamo antiguo siempre conserva a su
responsable, aunque esa persona ya no use el sistema.

## 6. Usuarios del sistema y beneficiarios

- **Encargados de bodega o de herramientas:** registran préstamos, devoluciones y mantenimientos, y
  dan seguimiento a los vencidos.
- **Administradores del sistema:** gestionan los usuarios y sus permisos.
- **Gerencia:** consulta reportes para conocer el uso, las pérdidas y el gasto en mantenimiento.
- **Empleados en general:** se benefician de recibir herramientas en buen estado y de un proceso de
  préstamo ordenado.

## 7. Beneficios para la empresa

- Información centralizada, disponible y sin depender de hojas o archivos sueltos.
- Conocimiento en todo momento del estado y la ubicación de cada herramienta.
- Responsables claros de cada préstamo, devolución, daño y pérdida.
- Menos herramientas dañadas en circulación y menos riesgos para el trabajador.
- Control del gasto en mantenimiento, expresado en lempiras.
- Datos concretos para decidir compras, reposiciones y medidas con empleados que acumulan atrasos o
  daños.

## 8. Idea central

> Antes no se sabía quién tenía cada herramienta, en qué estado regresaba ni cuánto costaba mantenerla.
> Ahora cada pieza tiene su historial y cada movimiento tiene un responsable.
