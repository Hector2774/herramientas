// Services/PrestamoService.cs
// Cubre: Préstamos y Devoluciones

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using PromacoHerra.Data;
using Microsoft.Data.SqlClient;

namespace PromacoHerra.Services
{
    // ══════════════════════════════════════════════════════════════
    // PRÉSTAMOS
    // ══════════════════════════════════════════════════════════════
    public static class PrestamoService
    {
        /// <summary>
        /// Registra un préstamo nuevo con sus herramientas.
        /// Devuelve el PrestamoId generado, o 0 si falló.
        /// </summary>
        public static int Registrar(
            int empleadoId,
            int aprobadoPorId,
            DateTime fechaDevolucionEsperada,
            string observaciones,
            List<int> herramientaIds)
        {
            // Construir el XML con los IDs de herramientas
            var xml = new StringBuilder("<herramientas>");
            foreach (var id in herramientaIds)
                xml.Append($"<item id='{id}'/>");
            xml.Append("</herramientas>");

            var result = Db.ExecuteSPScalar("sp_Prestamo_Registrar",
                Db.Param("@EmpleadoId", empleadoId),
                Db.Param("@AprobadoPorId", aprobadoPorId),
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
    }
}