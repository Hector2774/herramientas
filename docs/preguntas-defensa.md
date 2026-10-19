# Preguntas posibles de la terna

**Sistema de Control de Herramientas · PROMACO**

Preguntas que haría un ingeniero de la terna después de ver la presentación y revisar el sistema.
Cada una trae una **pista** de por dónde va la respuesta, según lo que el sistema hace hoy. Las marcadas
con ⚠️ tocan puntos débiles reales del proyecto: es mejor tener la respuesta preparada (reconocer el
límite y decir cómo se resolvería) que improvisarla.

---

## 1. El cliente y el problema

1. **¿Qué es PROMACO, a qué se dedica y cuántos empleados tiene?**
   Pista: empresa hondureña; las herramientas no se venden, son de uso interno y se prestan a bodegas,
   taller y operaciones. Ten a mano números aproximados: empleados, departamentos y herramientas.

2. **¿Cómo se llevaba el control antes del sistema? ¿Quién lo hacía?**
   Pista: papel o Excel no centralizado; cada encargado tenía su formato y su archivo.

3. **¿Cómo levantaste los requisitos? ¿Con quién hablaste dentro de la empresa?**
   Pista: entrevistas con los encargados de bodega y taller, observación del proceso y revisión de los
   formatos en papel o Excel. Menciona nombres de puestos, no solo "el cliente".

4. **¿Cuánto dinero pierde la empresa por herramientas perdidas o mal reparadas? ¿Tienes un estimado?**
   Pista: si no hay cifra exacta, explica que antes no existía el dato, y que justamente el sistema
   ahora lo calcula (reporte de dañadas y perdidas, costo de mantenimiento en L.).

5. **¿Cuál fue el problema más grave que motivó el proyecto?**
   Pista: herramientas dañadas que volvían a circular y pérdidas sin responsable.

6. **¿El cliente ya usa el sistema? ¿Quién lo validó y cómo?**
   Pista: di con honestidad en qué etapa está (piloto, pruebas, en uso). Si hubo pruebas con usuarios,
   menciona qué cambió gracias a ellas.

7. **¿Por qué no compraron un software de inventario existente o usaron Excel compartido?**
   Pista: un ERP o software comercial tiene costo de licencia y no maneja el préstamo por pieza, el
   aprobador ni el flujo dañada → mantenimiento. Un Excel compartido no impide errores ni deja historial
   confiable.

8. **¿Qué indicadores muestran que el sistema resolvió el problema? ¿Cómo medirías el éxito?**
   Pista: porcentaje de préstamos vencidos, número de pérdidas por mes, tiempo promedio de reparación,
   costo de mantenimiento por herramienta. Todo sale de los reportes.

9. **¿Cuál es el alcance? ¿Qué quedó fuera?**
   Pista: está dentro préstamo, devolución, mantenimiento, reportes y usuarios. Quedan fuera las
   compras de herramientas, la contabilidad, la depreciación y las notificaciones reales.

---

## 2. Reglas de negocio

10. **¿Por qué controlar por pieza física y no solo por cantidad?**
    Pista: si una pieza se daña, las demás del mismo tipo siguen disponibles, y siempre se sabe cuál
    pieza exacta tiene cada persona. El stock no se escribe a mano: se calcula contando piezas.

11. **¿Qué pasa si un empleado pierde una herramienta? ¿Se le cobra?**
    Pista: el sistema deja constancia de quién la tenía y quién aprobó. El cobro es una decisión de
    RRHH o de la gerencia; el sistema da la evidencia, pero no descuenta del salario.

12. **¿Por qué nadie puede aprobar su propio préstamo?**
    Pista: segregación de funciones, un control interno básico. Evita que alguien se entregue
    herramientas a sí mismo sin que otra persona lo vea.

13. **¿Quién decide el plazo de devolución? ¿Hay un máximo?**
    Pista: el encargado lo indica; debe ser posterior a hoy. Si te preguntan por un máximo, puede ser
    una mejora configurable por herramienta o categoría.

14. **¿Qué pasa cuando un préstamo se vence? ¿Quién da seguimiento?**
    Pista: se marca como vencido automáticamente, aparece en rojo en el inicio y en el reporte de vencidos.
    ⚠️ El botón **"Notificar" hoy solo muestra un mensaje**: no envía correo ni guarda la notificación.
    Si lo muestras en la defensa, dilo como funcionalidad prevista (correo o mensaje al jefe del
    departamento), no como terminada.

15. **¿Se puede devolver solo una parte de un préstamo?**
    Pista: sí. Cada pieza se devuelve por separado, con su condición, y el préstamo se cierra solo cuando
    no le quedan piezas pendientes.

16. **¿Qué diferencia hay entre una pieza dañada, perdida y dada de baja?**
    Pista: dañada = apartada hasta repararse; perdida = se descuenta y queda el responsable; baja =
    irreparable o retirada, se conserva su historial.

17. **¿Por qué en un mantenimiento interno no se cobra la mano de obra?**
    Pista: el técnico ya recibe salario. Se registra el costo adicional para la empresa (materiales);
    en el externo se suma la factura del proveedor, y en garantía el costo es cero.

18. **¿Cómo decide la empresa si repara una herramienta o compra otra?**
    Pista: el historial de mantenimiento por herramienta (filtro nuevo en reportes) muestra el costo
    acumulado y la frecuencia de fallas. Si reparar cuesta más que una nueva, se da de baja.

19. **¿Por qué no se borra nada?**
    Pista: integridad del historial y auditoría. Un préstamo viejo debe seguir mostrando qué
    herramienta, qué empleado y quién aprobó, aunque ya no existan o estén inactivos.

20. **¿Qué pasa con un empleado que se va de la empresa y todavía tiene herramientas?**
    Pista: la sincronización con RRHH lo marca como inactivo, pero sus préstamos se conservan. Una mejora
    lógica: alertar si un empleado inactivo tiene préstamos abiertos (por ejemplo, antes de su
    liquidación).

21. **¿Qué es el mantenimiento preventivo y quién lo programa?**
    Pista: hoy se registra manualmente. Una mejora sería programarlo por fechas o por horas de uso.

---

## 3. Base de datos

22. **Explícame el modelo entidad-relación. ¿Cuáles son las tablas principales?**
    Pista: Herramienta (tipo) → HerramientaUnidad (pieza); Prestamo → PrestamoDetalle (una línea por
    pieza); Mantenimiento (ligado a la pieza y, si viene de una devolución, al detalle); Empleado,
    Usuario, Rol; catálogos de categorías, marcas, ubicaciones y proveedores.

23. **¿Está normalizada? ¿Hasta qué forma normal?**
    Pista: tercera forma normal. El stock no se guarda (se calcula en `vw_HerramientaStock`) para evitar
    datos duplicados que se desincronicen. Los roles están en su propia tabla.

24. **¿Por qué la llave foránea compuesta (UnidadId, HerramientaId) en PrestamoDetalle?**
    Pista: garantiza en la base de datos que la pieza pertenece a esa herramienta; no depende de que la
    aplicación lo valide.

25. **¿Por qué pusiste la lógica en procedimientos almacenados y no en C#?**
    Pista: las reglas se cumplen aunque alguien acceda a la base sin la aplicación, las transacciones
    quedan junto a los datos, los parámetros evitan la inyección SQL y hay menos viajes por la red.
    Contra: es más difícil de probar y versionar. Lo compensas con scripts de migración numerados.

26. **¿Qué pasa si dos encargados prestan la misma pieza al mismo tiempo?**
    Pista: los procedimientos usan transacciones y bloqueos (`UPDLOCK`) y verifican el estado de la pieza
    dentro de la transacción. Además, un índice único impide dos mantenimientos abiertos para la misma
    pieza. Prepárate para explicar qué es una condición de carrera.

27. **¿Qué restricciones (CHECK) tiene la base de datos?**
    Pista: estados válidos de la pieza y del préstamo, costos no negativos, fecha de fin posterior a la de
    inicio, un mantenimiento interno sin proveedor y uno externo sin técnico, y una fecha de baja
    obligatoria cuando el estado es Baja.

28. **¿Cómo manejas los cambios en la base de datos?**
    Pista: scripts de migración numerados (002 a 021) y un `000_esquema.sql` con el estado completo para
    instalaciones nuevas.

29. **¿Cómo se respalda la información? ¿Cada cuánto? ¿Y si se daña el disco?**
    ⚠️ El sistema no automatiza los respaldos. Propón un plan: respaldo completo diario con una tarea
    programada o un plan de mantenimiento de SQL Server, copia en otro equipo o en la nube, y una prueba
    periódica de restauración. Ojo: SQL Server Express no trae el Agente, así que se usa el Programador
    de tareas de Windows con `sqlcmd`.

30. **¿Cuánto va a crecer la base? ¿Aguanta SQL Server Express?**
    Pista: Express permite hasta 10 GB por base. Con cientos de préstamos al mes ocupa pocos MB al año.
    Las fotos se guardan como archivos, no dentro de la base, justamente para no inflarla.

31. **¿Por qué las fotos van en una carpeta y no en la base de datos?**
    Pista: la base queda liviana y los respaldos son rápidos. Contra: hay que respaldar la carpeta por
    separado y compartirla si hay varios equipos.

32. **¿Tienes índices? ¿Cómo decidiste dónde ponerlos?**
    Pista: en las llaves foráneas y en las consultas más frecuentes (estado del préstamo, pieza en
    detalle, herramienta en mantenimiento), más un índice filtrado para el mantenimiento abierto.

---

## 4. Arquitectura y tecnología

33. **¿Por qué una aplicación de escritorio y no web?**
    Pista: se usa en el puesto de bodega dentro de la red interna, no requiere servidor web ni hosting,
    responde más rápido y el equipo del cliente ya usa Windows. Contra: hay que instalarla en cada
    equipo. Una versión web o móvil puede ser la siguiente fase.

34. **¿Por qué C#/.NET y SQL Server?**
    Pista: ecosistema Microsoft que la empresa ya usa, SQL Server Express gratuito, buen soporte para
    Windows Forms, y es lo que dominas.

35. **Explica la arquitectura en capas.**
    Pista: formularios (interfaz) → servicios (reglas de la aplicación) → acceso a datos (`Db.cs`) →
    SQL Server (procedimientos, vistas, tablas).

36. **¿Puede usarse desde varias computadoras al mismo tiempo?**
    ⚠️ La cadena de conexión apunta a `localhost`, fija en `Db.cs`. Para usarla en varios equipos, SQL
    Server se instala en un servidor y se cambia la cadena (idealmente en un archivo de configuración,
    no en el código). La base ya soporta concurrencia gracias a las transacciones.

37. **¿Por qué la cadena de conexión está en el código y no en un archivo de configuración?**
    ⚠️ Es una mejora pendiente: moverla a `appsettings.json` o `App.config` para cambiar de servidor sin
    recompilar.

38. **¿Cómo funciona la sincronización con RRHH? ¿Qué pasa si la API no responde?**
    Pista: descarga los empleados de la API interna y los inserta o actualiza. Si no hay conexión,
    muestra un error y el sistema sigue funcionando con los empleados ya guardados.
    ⚠️ La dirección de la API (`http://192.168.0.15:8000`) está fija en el código y usa HTTP sin cifrar.
    Mejora: llevarla a configuración, usar HTTPS y autenticarse contra la API.

39. **¿Por qué no se pueden crear empleados a mano?**
    Pista: RRHH es la fuente oficial; así se evitan duplicados y empleados inventados.

40. **¿Qué librerías usaste y por qué?**
    Pista: Microsoft.Data.SqlClient (conexión), ClosedXML (Excel sin necesidad de tener Office), iText
    (PDF) y FontAwesome.Sharp (íconos). Actualizaste Newtonsoft.Json para corregir una vulnerabilidad
    conocida de una dependencia.

41. **¿Revisaste la licencia de iText?**
    Pista: iText usa la licencia AGPL. Para uso interno de la empresa no hay problema, pero si el sistema
    se vendiera habría que comprar la licencia comercial o cambiar a otra librería, como PdfSharp
    (licencia MIT) o QuestPDF (gratuita para empresas pequeñas).

42. **¿Cómo se instala en un equipo nuevo?**
    Pista: instalar .NET Desktop Runtime y SQL Server Express, ejecutar los scripts 000 y 001, y copiar
    la aplicación con su carpeta de fotos. Mejora: un instalador (MSIX o Inno Setup).

43. **¿Usaste control de versiones?**
    Pista: Git con un repositorio en GitHub; muestra el historial de commits.

---

## 5. Seguridad

44. **¿Cómo se guardan las contraseñas?**
    Pista: hash PBKDF2-SHA256 con sal por usuario y muchas iteraciones (600,000). Nunca en texto plano;
    ni el administrador puede verlas.

45. **¿Por qué PBKDF2 y no MD5 o SHA-256 directo?**
    Pista: MD5 y SHA son rápidos, y eso facilita los ataques de fuerza bruta. PBKDF2 es lento a propósito
    y la sal impide usar tablas precalculadas.

46. **¿Qué pasa si alguien intenta adivinar contraseñas muchas veces?**
    ⚠️ No hay bloqueo por intentos fallidos. Mejora: bloquear la cuenta temporalmente después de 5
    intentos y registrar los intentos.

47. **¿Cómo evitas la inyección SQL?**
    Pista: todo pasa por procedimientos almacenados con parámetros; no hay SQL armado concatenando texto
    del usuario.

48. **¿Qué puede hacer un Operador y qué un Administrador? ¿Dónde se valida?**
    Pista: solo el administrador gestiona usuarios. Se valida en la interfaz (el menú no aparece) **y**
    en los procedimientos almacenados. Validar solo en la interfaz no basta.

49. **¿Hay bitácora de auditoría? ¿Puedo saber quién modificó una herramienta o cerró un mantenimiento?**
    Pista: préstamos (aprobador) y mantenimientos (quién lo registró y quién lo cerró) guardan el
    responsable.
    ⚠️ No hay una bitácora general de cambios (quién editó qué dato y cuándo). Mejora: una tabla de
    auditoría o la función Temporal Tables de SQL Server.

50. **¿Qué pasa si alguien con acceso a la base modifica los datos directamente?**
    Pista: los CHECK y las llaves foráneas impiden datos inválidos, pero no impiden un cambio válido
    malintencionado. Se controla con permisos de SQL Server: los usuarios solo ejecutan procedimientos y
    no tienen acceso directo a las tablas.

51. **¿La conexión a la base está cifrada?**
    Pista: usa autenticación de Windows; `TrustServerCertificate=True` acepta el certificado sin
    validarlo, aceptable en una red interna. En producción se instalaría un certificado válido.

52. **¿Qué pasa si un usuario deja la sesión abierta y se va?**
    Pista: después de 15 minutos sin teclado ni mouse, el sistema cierra los diálogos abiertos, cierra la
    sesión y vuelve al login con un aviso. Así nadie puede prestar herramientas a nombre de quien dejó
    el equipo abierto. Lo que no se haya guardado en ese momento se pierde, igual que en la banca en
    línea.

---

## 6. Interfaz y usabilidad

53. **¿Cómo diseñaste la interfaz? ¿En qué te basaste?**
    Pista: identidad visual de PROMACO, estilo Material, colores por estado (rojo = vencido, naranja =
    mantenimiento, verde = disponible). Ver `docs/diseno-interfaz.md`.

54. **¿El personal de bodega sabe usarlo? ¿Necesita capacitación?**
    Pista: flujos cortos, buscadores en lugar de listas enormes, mensajes claros. Una capacitación breve y
    un manual de usuario.

55. **¿Por qué no usar código de barras o QR en las piezas?**
    Pista: es la mejora más natural. Cada pieza ya tiene un código único (`HER-0001-03`), así que solo
    falta imprimir etiquetas y usar un lector, que funciona como un teclado. Se agilizan préstamos,
    devoluciones e inventarios físicos.

56. **¿Funciona en pantallas pequeñas o con otra escala de Windows?**
    Pista: está diseñado para 1920×1080 con escala al 100 %; así está documentado en los requisitos.

57. **¿Cómo manejas los errores? ¿Qué ve el usuario si se cae la base de datos?**
    Pista: hay un manejo centralizado de errores (`Errores.cs`) que traduce el error técnico a un mensaje
    entendible, y se usa en todas las pantallas.

---

## 7. Reportes y toma de decisiones

58. **¿Qué decisión concreta puede tomar la gerencia con cada reporte?**
    Pista:
    - herramientas más usadas → qué comprar en mayor cantidad;
    - mantenimiento → cuánto se gasta y qué conviene reemplazar;
    - por empleado → quién acumula atrasos;
    - dañadas y perdidas → responsables e incidentes;
    - inventario → qué tiene poco stock.

59. **¿Los reportes se pueden filtrar?**
    Pista: por fechas (o todas las fechas), empleado, herramienta, estado, categoría, departamento y tipo
    de mantenimiento, según el reporte. Se exportan a PDF y Excel.

60. **¿Cómo calculas la "demanda" en el ranking de herramientas?**
    Pista: número de préstamos de cada herramienta en el período, comparado con la más prestada (100 %).

61. **¿Los costos incluyen impuestos? ¿Por qué con centavos?**
    Pista: se registra el total de la factura en lempiras, con dos decimales, porque las facturas los
    tienen.

---

## 8. Calidad y pruebas

62. **¿Cómo probaste el sistema?**
    Pista: pruebas manuales de cada flujo y datos simulados de seis meses que respetan las reglas.
    Verificaste la consistencia: ninguna pieza en dos préstamos a la vez y estados que coinciden con el
    último movimiento.
    ⚠️ No hay pruebas automatizadas (unitarias o de integración). Reconócelo y menciona cómo las harías:
    xUnit para los servicios y pruebas de los procedimientos con tSQLt o con una base de prueba.

63. **¿Qué datos usaste para la demostración? ¿Son reales?**
    Pista: los empleados vienen de RRHH; los préstamos y mantenimientos son simulados con reglas
    realistas. Sé transparente: si te preguntan, di que son datos simulados.

64. **¿Qué pasa si se ingresa una fecha de devolución anterior a hoy, o un costo negativo?**
    Pista: se valida en la pantalla y en la base de datos (procedimientos y CHECK).

65. **¿Cuál fue el error más difícil que encontraste y cómo lo resolviste?**
    Pista: prepara una anécdota real; por ejemplo, la compatibilidad con SQL Server 2019 (migración 017),
    los costos con centavos (018) o los catálogos que dejaban herramientas sin categoría al borrarse (019).

---

## 9. Gestión del proyecto y costos

66. **¿Qué metodología usaste?**
    Pista: iterativa e incremental; cada migración refleja una iteración con mejoras a partir de la
    retroalimentación.

67. **¿Cuánto tiempo tomó y cómo lo planificaste?**
    Pista: ten un cronograma con las fases: análisis, diseño, desarrollo, pruebas, documentación.

68. **¿Cuánto cuesta el proyecto y cuánto le ahorra a la empresa? ¿En cuánto tiempo se recupera?**
    Pista: usa `docs/presupuesto-costos.xlsx`. El software base es gratuito (SQL Server Express, .NET), así
    que el costo es sobre todo de desarrollo y equipo. Calcula el ahorro con pérdidas evitadas y
    reparaciones mejor controladas.

69. **¿Quién va a dar mantenimiento al sistema cuando termines?**
    Pista: el código está documentado y en un repositorio, con scripts de instalación y migración. Debe
    quedar una persona de TI de la empresa como responsable, o un contrato de soporte.

70. **¿Qué riesgos identificaste?**
    Pista: que el personal no registre los préstamos (resistencia al cambio), falla del equipo sin
    respaldo, caída de la API de RRHH o pérdida de la carpeta de fotos.

---

## 10. Mejoras y siguiente fase

71. **Si tuvieras seis meses más, ¿qué agregarías?**
    Pista, en orden de valor:
    1. Etiquetas con código QR y lector.
    2. Notificaciones reales de vencidos por correo o WhatsApp.
    3. Respaldos automáticos.
    4. Configuración externa (servidor, API).
    5. Mantenimiento preventivo programado.
    6. Bitácora de auditoría.
    7. Versión web o móvil para consultar.
    8. Firma digital del empleado al recibir.

72. **¿Se puede usar en otras sucursales?**
    Pista: con un servidor central y el campo de ubicación o sucursal; la base ya separa las ubicaciones.

73. **¿Se podría integrar con el sistema contable o de compras?**
    Pista: los costos de mantenimiento y las bajas son datos que contabilidad necesita. Se integraría con
    una exportación o una API.

74. **¿Cómo escalaría si en lugar de 80 piezas fueran 5,000?**
    Pista: el modelo por pieza escala bien; los buscadores ya existen para listas grandes, y los índices
    cubren las consultas principales.

---

## 11. Preguntas "trampa" o de criterio

75. **¿Por qué el aprobador es el empleado y no el usuario?**
    Pista: si la cuenta se desactiva o se reasigna, el historial sigue diciendo quién aprobó en ese
    momento.

76. **Si la pieza se dañó por desgaste normal y no por culpa del empleado, ¿el sistema lo culpa?**
    Pista: no. Si se descubre en el almacén, sin préstamo de por medio, se marca dañada desde "Unidades":
    queda registrada **sin responsable** (origen "Almacén", con quién la registró, la fecha y la nota) y
    aparece así en el reporte de dañadas y perdidas. Si se dañó durante un préstamo, el sistema registra
    quién la tenía y la nota de la devolución, pero la responsabilidad la decide una persona; los datos
    solo apoyan esa decisión.

77. **¿Qué pasa si el encargado registra la devolución como "Buena" para no meterse en problemas?**
    Pista: ningún sistema evita el fraude de quien registra. Ayudan las fotos al devolver, las auditorías
    de inventario y la firma del empleado.

78. **¿Por qué el préstamo se aprueba a nombre del usuario con sesión y no se elige el aprobador?**
    Pista: evita suplantar a otra persona; el responsable es quien está operando el sistema.

79. **¿Qué parte del sistema es la más débil hoy?**
    Pista: responde con honestidad. Por ejemplo: notificaciones simuladas, configuración fija en el
    código, sin pruebas automatizadas y respaldos manuales. Muestra que conoces los límites y cómo
    resolverlos.

80. **¿Qué aprendiste con este proyecto?**
    Pista: modelado de datos con reglas reales, seguridad de contraseñas, trabajo con un cliente real y
    tomar decisiones de diseño con lógica de negocio.

---

## Resumen: puntos débiles que conviene tener preparados

| Punto | Situación actual | Respuesta sugerida |
|---|---|---|
| Botón "Notificar" | Solo muestra un mensaje | "Está previsto enviar un correo al empleado y a su jefe; la interfaz ya está lista." |
| Cadena de conexión y URL de RRHH | Fijas en el código | "Pasan a un archivo de configuración para instalar en servidor sin recompilar." |
| API de RRHH | HTTP sin autenticación | "Red interna; en producción, HTTPS con token." |
| Respaldos | Manuales | "Tarea programada diaria con `sqlcmd` y copia fuera del equipo." |
| Pruebas | Manuales, con datos simulados | "Siguiente paso: xUnit para servicios y una base de prueba para los procedimientos." |
| Bloqueo por intentos fallidos | No existe | "Bloqueo temporal después de 5 intentos." |
| Bitácora de cambios | Solo responsables de préstamos y mantenimientos | "Tabla de auditoría o Temporal Tables." |
| Licencia de iText (AGPL) | Uso interno | "Para venderlo, licencia comercial o cambiar a PdfSharp." |
