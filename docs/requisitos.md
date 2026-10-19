# Requisitos de hardware y software

**Sistema de Control de Herramientas · PROMACO**

El sistema es una aplicación de escritorio para Windows. Tiene dos componentes:

- **La aplicación**, que se instala en el equipo del encargado de herramientas y es donde se usan
  todas las pantallas.
- **El servidor de base de datos (SQL Server)**, donde se guarda toda la información: herramientas,
  piezas, préstamos, devoluciones, mantenimientos y usuarios.

En la configuración actual los dos componentes se instalan en **el mismo equipo**, que funciona como
puesto de control de la bodega. Por eso los requisitos de ese equipo suman los de la aplicación y los
del servidor de base de datos.

---

## 1. Requisitos de hardware

### 1.1 Equipo del puesto de control (aplicación y base de datos)

| Componente | Mínimo | Recomendado |
|---|---|---|
| Procesador | 64 bits (x64), doble núcleo, 1.4 GHz | 64 bits (x64), cuatro núcleos, 2.0 GHz o más |
| Memoria RAM | 4 GB | 8 GB |
| Almacenamiento libre | 10 GB | 20 GB en disco de estado sólido (SSD) |
| Pantalla | 1920 × 1080 píxeles, con la escala de Windows al 100 % | 1920 × 1080 píxeles o más, de 21 pulgadas o más |
| Red | Tarjeta de red Ethernet o Wi-Fi | Conexión por cable a la red interna de la empresa |
| Periféricos | Teclado y ratón | Teclado y ratón |

**Justificación:**

- **Procesador y memoria.** SQL Server pide como mínimo un procesador x64 de 1.4 GHz. La memoria de
  4 GB alcanza para Windows, SQL Server y la aplicación al mismo tiempo; con 8 GB el equipo responde con
  holgura aunque el encargado tenga abiertos otros programas, como Excel o el navegador.
- **Almacenamiento.** La instalación de SQL Server ocupa alrededor de 6 GB, y la aplicación junto con
  sus componentes, menos de 100 MB. La base de datos es pequeña: con seis meses de operación simulada
  ocupa menos de 100 MB. Las fotos de las herramientas pesan pocos cientos de KB cada una. El resto del
  espacio queda para el crecimiento de los datos, las copias de seguridad y los reportes exportados.
  Un disco SSD reduce el tiempo de arranque del sistema y de los reportes.
- **Pantalla.** La ventana principal del sistema tiene un tamaño fijo, pensado para una pantalla Full
  HD (1920 × 1080). En pantallas de menor resolución, o con la escala de Windows por encima del 100 %,
  la ventana no cabe completa.
- **Red.** La red se necesita para sincronizar la lista de empleados con el sistema de Recursos
  Humanos, que solo responde dentro de la red interna de la empresa. El resto de funciones no requiere
  conexión.

### 1.2 Equipos opcionales

| Equipo | Uso |
|---|---|
| Impresora | Imprimir los reportes exportados en PDF para firmas o archivo físico. |
| Unidad de respaldo (disco externo o carpeta en red) | Guardar copias de seguridad periódicas de la base de datos. |
| Cámara o teléfono | Tomar las fotos de las herramientas que se cargan en el sistema. |

---

## 2. Requisitos de software

### 2.1 Software necesario para usar el sistema

| Software | Versión | Uso |
|---|---|---|
| Sistema operativo | Windows 10 o Windows 11, de 64 bits | La aplicación es de escritorio y solo funciona en Windows. |
| Microsoft .NET Desktop Runtime | 10 | Entorno necesario para ejecutar la aplicación. Es gratuito y lo distribuye Microsoft. |
| Microsoft SQL Server | 2019 o posterior. La edición gratuita **Express** es suficiente | Motor de base de datos donde se guarda la información y se aplican las reglas del negocio. |
| SQL Server Management Studio o la herramienta `sqlcmd` | Versión actual | Crear la base de datos durante la instalación y hacer copias de seguridad. |
| Lector de PDF | Cualquiera, por ejemplo Microsoft Edge o Adobe Acrobat Reader | Abrir los reportes exportados a PDF. |
| Hoja de cálculo | Microsoft Excel o una alternativa compatible, como LibreOffice Calc | Abrir los reportes exportados a Excel. |

**Configuración necesaria:**

- La base de datos se crea con la intercalación `Modern_Spanish_CI_AS`, para que las búsquedas y el
  ordenamiento traten correctamente las tildes y la ñ.
- La aplicación se conecta a SQL Server con la **autenticación de Windows**, así que no guarda
  contraseñas de la base de datos. La cuenta de Windows del equipo debe tener permiso sobre la base
  `PROMACO_Herramientas`.
- La edición Express permite bases de datos de hasta 10 GB. Por el volumen de información que maneja el
  sistema, este límite no se alcanzaría en muchos años de operación.

### 2.2 Software utilizado en el desarrollo

| Software | Versión | Uso |
|---|---|---|
| Windows 11 Pro | 64 bits | Sistema operativo del equipo de desarrollo. |
| Lenguaje C# sobre .NET | .NET 10 | Lenguaje y plataforma en los que se programó la aplicación. |
| Windows Forms | Incluido en .NET 10 | Tecnología de las pantallas de escritorio. |
| Visual Studio | Versión compatible con .NET 10 | Entorno de desarrollo para programar, diseñar las pantallas y depurar. |
| Microsoft SQL Server | 2022, edición Developer | Base de datos durante el desarrollo. El sistema se ajustó para que también funcione en SQL Server 2019. |
| SQL Server Management Studio y `sqlcmd` | Versión actual | Diseño de la base de datos y ejecución de los scripts. |
| Git | Versión actual | Control de versiones del código fuente. |

### 2.3 Librerías utilizadas por la aplicación

Se incluyen dentro de la aplicación, así que el usuario no tiene que instalarlas.

| Librería | Versión | Uso en el sistema |
|---|---|---|
| Microsoft.Data.SqlClient | 7.0.0 | Conexión entre la aplicación y SQL Server. |
| ClosedXML | 0.105.0 | Exportación de reportes a Excel. |
| iText | 9.5.0 | Exportación de reportes a PDF. |
| FontAwesome.Sharp | 6.6.0 | Íconos de la interfaz. |
| Newtonsoft.Json | 13.0.3 | Versión corregida de un componente que usa iText, para evitar una vulnerabilidad conocida de la versión anterior. |

---

## 3. Resumen

| | Requisito |
|---|---|
| **Hardware** | Equipo con procesador x64, 4 GB de RAM (8 GB recomendado), 10 GB libres en disco, pantalla Full HD y conexión a la red interna. |
| **Software** | Windows 10 u 11 de 64 bits, .NET 10 Desktop Runtime y SQL Server 2019 o posterior (Express es suficiente). |
