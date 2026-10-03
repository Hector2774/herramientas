// Services/PrestamoService.cs
// Cubre: Préstamos y Devoluciones

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using PromacoHerra.Data;
using PromacoHerra.Models;
using Microsoft.Data.SqlClient;

namespace PromacoHerra.Services
{
    // ══════════════════════════════════════════════════════════════
    // PRÉSTAMOS
    // ══════════════════════════════════════════════════════════════
    public static class PrestamoService
    {
        /// <summary>
        /// Registra un préstamo nuevo con las unidades físicas seleccionadas.
        /// Lo aprueba el empleado ligado al usuario en sesión.
        /// Devuelve el PrestamoId generado, o 0 si falló.
        /// </summary>
        public static int Registrar(
            int empleadoId,
            DateTime fechaDevolucionEsperada,
            string observaciones,
            List<int> unidadIds)
        {
            // Construir el XML con los IDs de las unidades
            var xml = new StringBuilder("<herramientas>");
            foreach (var id in unidadIds)
                xml.Append($"<item id='{id}'/>");
            xml.Append("</herramientas>");

            var result = Db.ExecuteSPScalar("sp_Prestamo_Registrar",
                Db.Param("@EmpleadoId", empleadoId),
                Db.Param("@UsuarioId", Sesion.UsuarioId),
                Db.Param("@FechaDevolucionEsperada", fechaDevolucionEsperada),
                Db.Param("@Observaciones", observaciones),
                Db.Param("@Herramientas",
                    SqlDbType.Xml, xml.ToString()));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Obtiene préstamos con filtros opcionales.
        /// Pasa null en cualquier filtro para ignorarlo.
        /// </summary>
        public static DataTable ObtenerTodos(
            string estado = null,
            int? empleadoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null) =>
            Db.QuerySP("sp_Prestamo_ObtenerTodos",
                Db.Param("@Estado", (object)estado ?? DBNull.Value),
                Db.Param("@EmpleadoId", (object)empleadoId ?? DBNull.Value),
                Db.Param("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                Db.Param("@FechaHasta", (object)fechaHasta ?? DBNull.Value));

        /// <summary>
        /// Devuelve dos DataTables: [0] encabezado, [1] detalle de herramientas.
        /// </summary>
        public static DataSet ObtenerDetalle(int prestamoId)
        {
            var ds = Db.QuerySPMultiple("sp_Prestamo_ObtenerDetalle",
                Db.Param("@PrestamoId", prestamoId));
            return ds;
        }

        public static DataTable ObtenerPorEmpleado(int empleadoId) =>
            Db.QuerySP("sp_Prestamo_ObtenerPorEmpleado",
                Db.Param("@EmpleadoId", empleadoId));

        /// <summary>
        /// Marca como Vencidos los préstamos que superaron su fecha límite.
        /// Devuelve cuántos fueron marcados.
        /// Llamar al iniciar la app o al abrir el formulario de Devoluciones.
        /// </summary>
        public static int MarcarVencidos()
        {
            var result = Db.ExecuteSPScalar("sp_Prestamo_MarcarVencidos");
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }

    // ══════════════════════════════════════════════════════════════
    // DEVOLUCIONES
    // ══════════════════════════════════════════════════════════════
    public static class DevolucionService
    {

        public static int MarcarVencidosYContar()
        {
            // Primero marca los vencidos
            Db.ExecuteSP("sp_Prestamo_MarcarVencidos");

            // Luego cuenta cuántos quedaron marcados hoy
            var dt = Db.Query(@"
        SELECT COUNT(*) AS Total
        FROM Prestamo
        WHERE Estado = 'Vencido'
          AND CAST(FechaDevolucionEsperada AS DATE) = CAST(GETDATE() AS DATE)");

            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["Total"]) : 0;
        }
        /// <summary>
        /// Devuelve una herramienta específica de un préstamo.
        /// estadoDevolucion: 'Bueno', 'Dañado' o 'Perdido'
        /// </summary>
        public static void RegistrarUna(
            int prestamoDetalleId,
            string estadoDevolucion,
            string observacion = null) =>
            Db.ExecuteSP("sp_Devolucion_RegistrarUna",
                Db.Param("@PrestamoDetalleId", prestamoDetalleId),
                Db.Param("@EstadoDevolucion", estadoDevolucion),
                Db.Param("@ObservacionDevolucion", observacion));

        /// <summary>
        /// Devuelve todas las herramientas pendientes de un préstamo
        /// con el mismo estado de devolución.
        /// </summary>
        public static void RegistrarTodas(
            int prestamoId,
            string estadoDevolucion,
            string observacion = null) =>
            Db.ExecuteSP("sp_Devolucion_RegistrarTodas",
                Db.Param("@PrestamoId", prestamoId),
                Db.Param("@EstadoDevolucion", estadoDevolucion),
                Db.Param("@ObservacionDevolucion", observacion));

        /// <summary>
        /// Lista de préstamos con herramientas pendientes de devolver.
        /// Ya llama internamente a sp_Prestamo_MarcarVencidos.
        /// </summary>
        public static DataTable ObtenerPrestamosActivos() =>
            Db.QuerySP("sp_Devolucion_ObtenerPrestamosActivos");

        /// <summary>
        /// Herramientas pendientes de un préstamo específico.
        /// </summary>
        public static DataTable ObtenerDetallePendiente(int prestamoId) =>
            Db.QuerySP("sp_Devolucion_ObtenerDetallePendiente",
                Db.Param("@PrestamoId", prestamoId));

        public static List<PrestamoActivo> ListarPrestamosActivos()
        {
            var lista = new List<PrestamoActivo>();
            foreach (DataRow r in ObtenerPrestamosActivos().Rows)
            {
                lista.Add(new PrestamoActivo
                {
                    PrestamoId = Convert.ToInt32(r["PrestamoId"]),
                    Empleado = r["Empleado"].ToString() ?? "",
                    CodigoEmpleado = r["CodigoEmpleado"].ToString() ?? "",
                    Departamento = r["Departamento"] as string ?? "",
                    FechaPrestamo = Convert.ToDateTime(r["FechaPrestamo"]),
                    FechaDevolucionEsperada = Convert.ToDateTime(r["FechaDevolucionEsperada"]),
                    Pendientes = Convert.ToInt32(r["HerramientasPendientes"]),
                    ColorIndex = lista.Count
                });
            }
            return lista;
        }

        public static List<DetallePendiente> ListarDetallePendiente(int prestamoId)
        {
            var lista = new List<DetallePendiente>();
            foreach (DataRow r in ObtenerDetallePendiente(prestamoId).Rows)
            {
                lista.Add(new DetallePendiente
                {
                    PrestamoDetalleId = Convert.ToInt32(r["PrestamoDetalleId"]),
                    UnidadId = Convert.ToInt32(r["UnidadId"]),
                    Codigo = r["CodigoHerramienta"].ToString() ?? "",
                    Herramienta = r["Herramienta"].ToString() ?? "",
                    Marca = r["Marca"] as string ?? "",
                    Categoria = r["Categoria"] as string ?? "",
                    FotoNombre = r["FotoNombre"] as string
                });
            }
            return lista;
        }

        /// <summary>
        /// Devuelve varias unidades de un préstamo en una sola transacción,
        /// cada una con su condición (Bueno / Dañado / Perdido) y su nota.
        /// Cierra el préstamo si ya no quedan pendientes.
        /// </summary>
        public static ResultadoDevolucion RegistrarVarias(int prestamoId, IEnumerable<DevolucionItem> items)
        {
            var xml = new XElement("items",
                items.Select(i => new XElement("item",
                    new XAttribute("id", i.PrestamoDetalleId),
                    new XAttribute("estado", i.Condicion.ToString()),
                    new XAttribute("nota", i.Nota ?? ""))));

            var dt = Db.QuerySP("sp_Devolucion_RegistrarVarias",
                Db.Param("@PrestamoId", prestamoId),
                Db.Param("@Items", SqlDbType.Xml, xml.ToString()));

            var r = dt.Rows[0];
            return new ResultadoDevolucion
            {
                Buenas = Convert.ToInt32(r["Buenas"]),
                Dañadas = Convert.ToInt32(r["Dañadas"]),
                Perdidas = Convert.ToInt32(r["Perdidas"]),
                Pendientes = Convert.ToInt32(r["Pendientes"]),
                PrestamoCerrado = Convert.ToBoolean(r["PrestamoCerrado"])
            };
        }
    }
}
