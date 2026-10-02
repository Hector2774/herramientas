// Services/MantenimientoService.cs
// Cubre: Mantenimiento, Proveedores y Reportes

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
        public const string Interno = "Interno";
        public const string Externo = "Externo";
        public static readonly string[] Tipos = { "Correctivo", "Preventivo", "Calibración" };

        /// <summary>
        /// Registra la entrada a mantenimiento de una o más unidades (pueden ser de distintas herramientas).
        /// Interno → lo realiza un empleado (empleadoId); Externo → un proveedor (proveedorId).
        /// Devuelve cuántas unidades entraron a mantenimiento.
        /// </summary>
        public static int RegistrarEntrada(
            IEnumerable<int> unidadIds,
            string tipoMantenimiento,
            string tipoServicio,
            int? empleadoId,
            int? proveedorId,
            string? descripcion)
        {
            var result = Db.ExecuteSPScalar("sp_Mantenimiento_RegistrarEntrada",
                Db.Param("@UnidadIds", string.Join(",", unidadIds)),
                Db.Param("@TipoMantenimiento", tipoMantenimiento),
                Db.Param("@Descripcion", string.IsNullOrWhiteSpace(descripcion) ? null : descripcion),
                Db.Param("@TipoServicio", tipoServicio),
                Db.Param("@EmpleadoId", empleadoId),
                Db.Param("@ProveedorId", proveedorId),
                Db.Param("@RegistradoPorId", Sesion.Activa ? Sesion.UsuarioId : null));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Cierra un mantenimiento activo.
        /// reparada = true  → la unidad vuelve a Disponible.
        /// reparada = false → irreparable: la unidad se da de baja y sale del stock.
        /// El costo total (materiales + mano de obra) lo calcula el SP. La mano de obra
        /// y el folio de factura solo aplican a servicios externos.
        /// </summary>
        public static void RegistrarSalida(
            int mantenimientoId,
            bool reparada,
            decimal? costoMateriales,
            decimal? costoManoObra,
            bool enGarantia,
            string? folioFactura,
            string? notasCierre) =>
            Db.ExecuteSP("sp_Mantenimiento_RegistrarSalida",
                Db.Param("@MantenimientoId", mantenimientoId),
                Db.Param("@NotasCierre", notasCierre),
                Db.Param("@CostoMateriales", costoMateriales),
                Db.Param("@CostoManoObra", costoManoObra),
                Db.Param("@EnGarantia", enGarantia),
                Db.Param("@FolioFactura", folioFactura),
                Db.Param("@Resultado", reparada ? "Disponible" : "Baja"),
                Db.Param("@CerradoPorId", Sesion.Activa ? Sesion.UsuarioId : null));

        /// <summary>
        /// Unidades que pueden entrar a mantenimiento (Disponibles o Dañadas). Las Dañadas
        /// van primero y traen quién reportó el daño, cuándo y su nota.
        /// </summary>
        public static DataTable UnidadesElegibles() =>
            Db.QuerySP("sp_Mantenimiento_UnidadesElegibles");

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
    // PROVEEDORES (talleres, servicio autorizado, laboratorios de calibración)
    // ══════════════════════════════════════════════════════════════
    public static class ProveedorService
    {
        public static DataTable ObtenerTodos() =>
            Db.QuerySP("sp_Proveedor_ObtenerTodos");

        public static void Insertar(string nombre, string telefono, string descripcion) =>
            Db.ExecuteSP("sp_Proveedor_Insertar",
                Db.Param("@Nombre", nombre),
                Db.Param("@Telefono", telefono),
                Db.Param("@Descripcion", descripcion));

        public static void Actualizar(int id, string nombre, string telefono, string descripcion) =>
            Db.ExecuteSP("sp_Proveedor_Actualizar",
                Db.Param("@ProveedorId", id),
                Db.Param("@Nombre", nombre),
                Db.Param("@Telefono", telefono),
                Db.Param("@Descripcion", descripcion));

        /// <summary>Elimina el proveedor; si ya tiene mantenimientos solo se desactiva.</summary>
        public static void Eliminar(int id) =>
            Db.ExecuteSP("sp_Proveedor_Eliminar", Db.Param("@ProveedorId", id));
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
