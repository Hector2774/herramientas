// Services/MantenimientoService.cs
// Cubre: Mantenimiento y Reportes

using System;
using System.Data;
using PromacoHerra.Data;
using Microsoft.Data.SqlClient;

namespace PromacoHerra.Services
{
    // ══════════════════════════════════════════════════════════════
    // MANTENIMIENTO
    // ══════════════════════════════════════════════════════════════
    public static class MantenimientoService
    {
        /// <summary>
        /// Registra la entrada de una herramienta a mantenimiento.
        /// tipoMantenimiento: 'Preventivo' o 'Correctivo'
        /// Devuelve el MantenimientoId generado.
        /// </summary>
        public static int RegistrarEntrada(
            int herramientaId,
            string tipoMantenimiento,
            string descripcion = null,
            string realizadoPor = null)
        {
            var result = Db.ExecuteSPScalar("sp_Mantenimiento_RegistrarEntrada",
                Db.Param("@HerramientaId", herramientaId),
                Db.Param("@TipoMantenimiento", tipoMantenimiento),
                Db.Param("@Descripcion", descripcion),
                Db.Param("@RealizadoPor", realizadoPor));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Cierra un mantenimiento activo.
        /// Restaura el estado y stock disponible de la herramienta.
        /// </summary>
        public static void RegistrarSalida(
            int mantenimientoId,
            string descripcion = null,
            decimal? costo = null) =>
            Db.ExecuteSP("sp_Mantenimiento_RegistrarSalida",
                Db.Param("@MantenimientoId", mantenimientoId),
                Db.Param("@Descripcion", descripcion),
                Db.Param("@Costo", (object)costo ?? DBNull.Value));

        /// <summary>
        /// Historial completo de mantenimientos de una herramienta.
        /// </summary>
        public static DataTable ObtenerPorHerramienta(int herramientaId) =>
            Db.QuerySP("sp_Mantenimiento_ObtenerPorHerramienta",
                Db.Param("@HerramientaId", herramientaId));

        /// <summary>
        /// Lista de mantenimientos actualmente en curso.
        /// </summary>
        public static DataTable ObtenerActivos() =>
            Db.QuerySP("sp_Mantenimiento_ObtenerActivos");
    }

    // ══════════════════════════════════════════════════════════════
    // REPORTES
    // ══════════════════════════════════════════════════════════════
    public static class ReporteService
    {
        /// <summary>
        /// Historial de préstamos con todos los filtros opcionales.
        /// Pasa null en cualquier parámetro para ignorarlo.
        /// </summary>
        public static DataTable HistorialPrestamos(
            int? empleadoId = null,
            int? herramientaId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string estado = null,
            int? departamentoId = null) =>
            Db.QuerySP("sp_Reporte_HistorialPrestamos",
                Db.Param("@EmpleadoId", (object)empleadoId ?? DBNull.Value),
                Db.Param("@HerramientaId", (object)herramientaId ?? DBNull.Value),
                Db.Param("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                Db.Param("@FechaHasta", (object)fechaHasta ?? DBNull.Value),
                Db.Param("@Estado", (object)estado ?? DBNull.Value),
                Db.Param("@DepartamentoId", (object)departamentoId ?? DBNull.Value));

        public static DataTable PrestamosVencidos() =>
            Db.QuerySP("sp_Reporte_PrestamosVencidos");

        /// <summary>
        /// Herramientas más prestadas en un período.
        /// top: cuántas mostrar (por defecto 10).
        /// </summary>
        public static DataTable HerramientasMasPrestadas(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int top = 10) =>
            Db.QuerySP("sp_Reporte_HerramientasMasPrestadas",
                Db.Param("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                Db.Param("@FechaHasta", (object)fechaHasta ?? DBNull.Value),
                Db.Param("@Top", top));

        public static DataTable EmpleadosConAtrasos(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null) =>
            Db.QuerySP("sp_Reporte_EmpleadosConAtrasos",
                Db.Param("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                Db.Param("@FechaHasta", (object)fechaHasta ?? DBNull.Value));

        public static DataTable HerramientasDañadas() =>
            Db.QuerySP("sp_Reporte_HerramientasDañadas");

        public static DataTable CostosMantenimiento(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            int? herramientaId = null) =>
            Db.QuerySP("sp_Reporte_CostosMantenimiento",
                Db.Param("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                Db.Param("@FechaHasta", (object)fechaHasta ?? DBNull.Value),
                Db.Param("@HerramientaId", (object)herramientaId ?? DBNull.Value));
    }
}