// Services/DashboardService.cs
// Cubre: datos del tablero de inicio (FrmDashboard)

using System.Data;
using PromacoHerra.Data;

namespace PromacoHerra.Services
{
    public static class DashboardService
    {
        /// <summary>Unidades por estado (dona): Disponibles, Prestadas, Vencidas, EnMantenimiento, Danadas.</summary>
        public static DataRow DistribucionUnidades() => Db.QuerySP("sp_Dashboard_DistribucionUnidades").Rows[0];

        /// <summary>30 filas (una por día): Fecha, Prestamos, Devoluciones.</summary>
        public static DataTable ActividadMensual() => Db.QuerySP("sp_Dashboard_ActividadMensual");

        /// <summary>Préstamos abiertos por departamento (top 6): Departamento, Total.</summary>
        public static DataTable PrestamosPorDepartamento() => Db.QuerySP("sp_Dashboard_PrestamosPorDepartamento");

        /// <summary>Vencidas y próximas 7 días: Empleado, Herramientas, FechaDevolucionEsperada, DiasRestantes.</summary>
        public static DataTable ProximasDevoluciones() => Db.QuerySP("sp_Dashboard_ProximasDevoluciones");

        /// <summary>Herramientas con unidades pero ninguna disponible (top 5).</summary>
        public static DataTable HerramientasSinStock() => Db.QuerySP("sp_Dashboard_HerramientasSinStock");

        /// <summary>Préstamos y devoluciones de hoy: Tipo, Empleado, Herramientas, Fecha.</summary>
        public static DataTable ActividadReciente() => Db.QuerySP("sp_Dashboard_ActividadReciente");
    }
}
