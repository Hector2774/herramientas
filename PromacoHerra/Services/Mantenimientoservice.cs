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
        /// Registra la entrada a mantenimiento de unidades de una herramienta.
        /// unidadIds null = todas las unidades Disponibles o Dañadas del grupo.
        /// tipoMantenimiento: 'Preventivo' o 'Correctivo'
        /// Devuelve cuántas unidades entraron a mantenimiento.
        /// </summary>
        public static int RegistrarEntrada(
            int herramientaId,
            IEnumerable<int>? unidadIds,
            string tipoMantenimiento,
            string descripcion = null,
            string realizadoPor = null)
        {
            var result = Db.ExecuteSPScalar("sp_Mantenimiento_RegistrarEntrada",
                Db.Param("@HerramientaId", herramientaId),
                Db.Param("@UnidadIds", unidadIds == null ? DBNull.Value : string.Join(",", unidadIds)),
                Db.Param("@TipoMantenimiento", tipoMantenimiento),
                Db.Param("@Descripcion", descripcion),
                Db.Param("@RealizadoPor", realizadoPor));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Cierra un mantenimiento activo.
        /// reparada = true  → la unidad vuelve a Disponible.
        /// reparada = false → irreparable: la unidad se da de baja y sale del stock.
        /// </summary>
        public static void RegistrarSalida(
            int mantenimientoId,
            string descripcion = null,
            decimal? costo = null,
            bool reparada = true) =>
            Db.ExecuteSP("sp_Mantenimiento_RegistrarSalida",
                Db.Param("@MantenimientoId", mantenimientoId),
                Db.Param("@Descripcion", descripcion),
                Db.Param("@Costo", (object)costo ?? DBNull.Value),
                Db.Param("@Resultado", reparada ? "Disponible" : "Baja"));

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

        // ── Reportes rediseñados (FrmReportes) ─────────────────────
        private static object N(object? valor) => valor ?? DBNull.Value;

        /// <summary>Una fila con los KPIs del tablero y cuántos cambiaron hoy.</summary>
        public static DataRow KPIs() => Db.QuerySP("sp_Reporte_KPIs").Rows[0];

        /// <summary>Una fila por préstamo; estado: Activo / Cerrado / Vencido.</summary>
        public static DataTable HistorialPorPrestamo(int? empleadoId, int? herramientaId,
            DateTime? desde, DateTime? hasta, string? estado) =>
            Db.QuerySP("sp_Reporte_HistorialPorPrestamo",
                Db.Param("@EmpleadoId", N(empleadoId)),
                Db.Param("@HerramientaId", N(herramientaId)),
                Db.Param("@FechaDesde", N(desde?.Date)),
                Db.Param("@FechaHasta", N(hasta?.Date)),
                Db.Param("@Estado", N(estado)));

        public static DataTable Vencidos(int? empleadoId, int? categoriaId) =>
            Db.QuerySP("sp_Reporte_Vencidos",
                Db.Param("@EmpleadoId", N(empleadoId)),
                Db.Param("@CategoriaId", N(categoriaId)));

        public static DataTable PrestamosPorEmpleado(DateTime? desde, DateTime? hasta, int? departamentoId) =>
            Db.QuerySP("sp_Reporte_PrestamosPorEmpleado",
                Db.Param("@FechaDesde", N(desde?.Date)),
                Db.Param("@FechaHasta", N(hasta?.Date)),
                Db.Param("@DepartamentoId", N(departamentoId)));

        public static DataTable RankingHerramientas(DateTime? desde, DateTime? hasta, int? categoriaId) =>
            Db.QuerySP("sp_Reporte_RankingHerramientas",
                Db.Param("@FechaDesde", N(desde?.Date)),
                Db.Param("@FechaHasta", N(hasta?.Date)),
                Db.Param("@CategoriaId", N(categoriaId)));

        /// <summary>estado: 'Activo' (sin cerrar), 'Cerrado' o null.</summary>
        public static DataTable HistorialMantenimiento(DateTime? desde, DateTime? hasta, string? tipo, string? estado) =>
            Db.QuerySP("sp_Reporte_HistorialMantenimiento",
                Db.Param("@FechaDesde", N(desde?.Date)),
                Db.Param("@FechaHasta", N(hasta?.Date)),
                Db.Param("@Tipo", N(tipo)),
                Db.Param("@Estado", N(estado)));

        /// <summary>condicion: 'Dañada', 'Perdida' o null.</summary>
        public static DataTable DanadasPerdidas(DateTime? desde, DateTime? hasta, string? condicion, int? empleadoId) =>
            Db.QuerySP("sp_Reporte_DanadasPerdidas",
                Db.Param("@FechaDesde", N(desde?.Date)),
                Db.Param("@FechaHasta", N(hasta?.Date)),
                Db.Param("@Condicion", N(condicion)),
                Db.Param("@EmpleadoId", N(empleadoId)));

        /// <summary>mostrar: 'Todas', 'StockBajo' (&lt; 30 %) o 'SinDisponibles'.</summary>
        public static DataTable InventarioCategoria(int? categoriaId, string mostrar) =>
            Db.QuerySP("sp_Reporte_InventarioCategoria",
                Db.Param("@CategoriaId", N(categoriaId)),
                Db.Param("@Mostrar", mostrar));

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