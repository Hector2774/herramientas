# Guion de la demostración

**Sistema de Control de Herramientas · PROMACO**
Duración sugerida: **15 a 20 minutos** + preguntas.

La idea central que debe quedar: *antes no se sabía quién tenía cada herramienta, en qué estado regresaba
ni cuánto costaba mantenerla; ahora cada pieza tiene historial y cada movimiento tiene un responsable.*

---

## Antes de empezar (preparación, el día anterior)

- [ ] SQL Server encendido y la base `PROMACO_Herramientas` con los datos de demostración cargados.
- [ ] La aplicación compilada y probada una vez de principio a fin (seguir este guion completo).
- [ ] Tener a mano tu usuario y contraseña de administrador.
- [ ] Elegir de antemano para la parte en vivo:
  - un **empleado** de bodega que hará el préstamo (que no sea el empleado de tu usuario: nadie aprueba su propio préstamo);
  - una **herramienta con unidades disponibles**, de preferencia eléctrica y con foto (por ejemplo el taladro percutor).
- [ ] **Respaldar la base** justo antes de la presentación: la demo en vivo crea registros reales y así
  puedes volver al estado limpio después (SSMS → clic derecho en la base → Tareas → Copia de seguridad).
- [ ] Pantalla en resolución completa, notificaciones apagadas, la app maximizada.

---

## 1. Apertura: el problema (1 min)

> "PROMACO presta herramientas todos los días a personal de bodega, taller y operaciones. Sin un registro
> no se sabe quién tiene cada pieza, cuáles están vencidas, cuáles regresaron dañadas ni cuánto se gasta
> en repararlas, y las pérdidas no tienen responsable. Este sistema lleva ese control **por unidad física**."

## 2. Inicio de sesión (1 min)

1. Abrir la aplicación: mostrar el **login con la identidad de PROMACO**.
2. Entrar con tu usuario.

> "Las contraseñas no se guardan: se guarda un *hash* PBKDF2 con sal, como hacen los sistemas bancarios.
> Ni siquiera un administrador de la base puede ver la contraseña de nadie."

## 3. Dashboard: la operación de un vistazo (2 min) — *Inicio*

Señalar de izquierda a derecha:

1. **Indicadores:** préstamos vencidos (rojo), activos, unidades disponibles, en mantenimiento y dañadas.
2. **Estado de las unidades** (dona) y **actividad de los últimos 30 días** (préstamos vs. devoluciones).
3. **Préstamos activos por departamento:** las bodegas son las que más usan herramientas.
4. **Próximas devoluciones** con los **VENCIDOS** marcados, y **Alertas activas**.

> "Al abrir el sistema, el encargado de bodega sabe en segundos qué tiene que perseguir hoy."

## 4. Inventario por unidad (2 min) — *Herramientas*

1. Seleccionar una herramienta: **foto**, datos y el stock en tarjetas (total, disponibles, prestadas,
   en mantenimiento, dañadas).
2. Abrir **Unidades / Stock**: cada pieza tiene su código (`HER-0001-03`) y su propio estado.

> "Esta es la decisión de diseño más importante: si una unidad se daña, **no se bloquean las demás** del
> mismo tipo. El stock no se escribe a mano: se calcula contando unidades."

3. (Opcional) Mostrar el interruptor **Préstamos activos / suspendidos** para sacar un tipo de
   herramienta de circulación sin darla de baja.

## 5. EN VIVO: un préstamo completo (6 min)

Es la parte más importante: muestra las reglas de negocio funcionando, no solo pantallas.

### 5.1 Registrar el préstamo — *Préstamos*

1. Elegir el **empleado** (lupa).
2. Señalar **APROBACIÓN Y FECHAS**: el aprobador es el empleado del usuario en sesión, no se escribe.
3. En el catálogo, presionar **+** en la herramienta elegida: el sistema asigna la siguiente unidad
   disponible. Mostrar **Ver unidades ›** para elegir una pieza concreta.
4. **Guardar préstamo**.

> "Queda registrado quién se la llevó, quién la entregó y cuándo debe volver. Si alguien intenta
> aprobarse su propio préstamo, el sistema lo impide."

### 5.2 Devolverla dañada — *Devoluciones*

1. Seleccionar el préstamo recién creado (a la izquierda).
2. En la unidad, elegir la condición **Dañado** y escribir una nota (por ejemplo *"Cable pelado"*).
3. **Devolver seleccionadas** y leer el mensaje de confirmación en voz alta:
   *las unidades dañadas no vuelven al stock hasta pasar por mantenimiento.*

> "En papel, una herramienta dañada vuelve al estante y el siguiente empleado se la lleva así. Aquí queda
> bloqueada automáticamente."

### 5.3 Repararla — *Herramientas → pestaña Mantenimiento*

1. La unidad aparece en la bandeja de **pendientes**, con quién la devolvió y su nota.
2. Seleccionarla → **Enviar a mantenimiento** → elegir **Externo** y el proveedor → **Abrir mantenimiento**.
3. Seleccionar el mantenimiento activo → **Cerrar mantenimiento**:
   - Resultado **Reparada**.
   - Materiales **L. 350.50** y mano de obra **L. 1,200.00** → el total se calcula solo (**L. 1,550.50**).
   - Folio de factura del proveedor.

> "Se registra lo que le cuesta a la empresa en lempiras. Si fuera un técnico interno, la mano de obra no
> se cobra porque ya la cubre su salario; si está en garantía, el costo es cero."

4. **Ver historial**: todos los mantenimientos de esa herramienta, con costo total, duración promedio y,
   al seleccionar uno, el desglose, la factura y **qué empleado la devolvió dañada**.

## 6. Reportes (2 min) — *Reportes*

1. Mostrar las tarjetas de reportes: historial de préstamos, vencidos, por empleado, herramientas más
   usadas, mantenimiento, dañadas y perdidas, inventario.
2. Abrir **Historial de mantenimiento** → **Generar reporte**: aparece el mantenimiento recién cerrado y
   el **costo total del período**.
3. Exportar a **PDF** (y mencionar **Excel**).

> "Esto es lo que la gerencia necesita: cuánto se gasta en reparar, qué herramientas fallan más y qué
> personas acumulan atrasos o daños."

## 7. Usuarios y seguridad (2 min) — *Usuarios*

1. **Nuevo usuario**: elegir un empleado, nombre de usuario y rol **Operador**.
2. El sistema genera una **contraseña temporal** (ya copiada al portapapeles).
3. (Si hay tiempo) Salir, entrar con ese usuario y mostrar que **obliga a cambiar la contraseña** y que
   el menú **Usuarios no aparece** para un operador.

> "Los usuarios no se borran, se desactivan: los préstamos que aprobaron deben seguir teniendo
> responsable. Y siempre queda al menos un administrador."

## 8. Cierre (1 min)

> "En resumen: control por unidad, trazabilidad de quién entrega y quién recibe, devoluciones que
> bloquean lo dañado, costos de mantenimiento en lempiras y reportes para decidir. Todo con la lógica
> en la base de datos, contraseñas protegidas y un instalador que permite montarlo en cualquier equipo."

Mostrar, si lo piden, el repositorio: README, diagrama entidad-relación y scripts de instalación.

---

## Si algo falla en vivo

| Situación | Qué hacer |
|---|---|
| El préstamo no se guarda | Revisar que el empleado no sea el de tu usuario y que la fecha de devolución sea posterior a hoy. Leer el mensaje: el sistema explica la regla. |
| No aparece la herramienta en el catálogo | No tiene unidades disponibles o su préstamo está suspendido: elegir otra. |
| "No se pudo conectar con la base de datos" | SQL Server está detenido: iniciarlo (Servicios de Windows → SQL Server) y volver a intentar. Aprovechar: *"el sistema no se cae, avisa qué revisar"*. |
| La sincronización de empleados falla | Es normal fuera de la red de la empresa: la API de RRHH solo responde ahí. No usarla en la demo. |

---

## Preguntas probables y cómo responderlas

**¿Por qué separar herramienta y unidad?**
Porque el préstamo, el daño y la reparación le pasan a una pieza física, no al tipo. Con un contador por
tipo, una pieza dañada bloqueaba todas o se perdía la pista de cuál era.

**¿Por qué la lógica está en procedimientos almacenados?**
Las reglas (disponibilidad, que nadie se autoapruebe, cierre automático del préstamo) se validan dentro de
una transacción en la base. Así se cumplen aunque dos personas operen al mismo tiempo, y no hay SQL armado
con texto, lo que evita inyección SQL.

**¿Cómo se protegen las contraseñas?**
PBKDF2-SHA256 con 600,000 iteraciones y una sal distinta por usuario; se compara en tiempo constante.
Las contraseñas antiguas en texto plano se migraron solas en el siguiente inicio de sesión.

**¿Qué pasa si se borra un empleado o un usuario?**
No se borran: se desactivan. Los préstamos y mantenimientos los siguen referenciando y el historial no se
pierde.

**¿Cómo se calcula el costo de un mantenimiento?**
Materiales más mano de obra, esta última solo en servicios externos (el técnico interno ya cobra su
salario). En garantía el costo es cero. Todo en lempiras, con centavos.

**¿La base de datos está normalizada?**
Sí: catálogos separados (categorías, marcas, ubicaciones, proveedores, roles), el stock se calcula en una
vista en lugar de guardarse, y los roles son una tabla con el permiso como dato (`AdministraUsuarios`).
El diagrama está en `docs/diagrama-er.md`.

**¿De dónde salen los datos que se ven?**
Los empleados vienen del sistema de RRHH. El historial de seis meses es una simulación que respeta las
mismas reglas que la aplicación, para poder mostrar reportes con información (`Database/demo`).

**¿Qué mejorarías después?**
Bloqueo de la cuenta tras varios intentos fallidos, configuración de la conexión en un archivo en lugar del
código, pruebas automatizadas y alertas por correo de préstamos vencidos.
