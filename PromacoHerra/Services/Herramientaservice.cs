// Services/HerramientaService.cs
// Cubre: Herramientas, Categorías, Marcas y Ubicaciones

using System.Data;
using PromacoHerra.Data;
using Microsoft.Data.SqlClient;

namespace PromacoHerra.Services
{
    // ══════════════════════════════════════════════════════════════
    // CATEGORÍAS
    // ══════════════════════════════════════════════════════════════
    public static class CategoriaService
    {
        public static DataTable ObtenerTodas() =>
            Db.QuerySP("sp_Categoria_ObtenerTodas");

        public static void Insertar(string nombre, string descripcion) =>
            Db.ExecuteSP("sp_Categoria_Insertar",
                Db.Param("@Nombre", nombre),
                Db.Param("@Descripcion", descripcion));

        public static void Actualizar(int id, string nombre, string descripcion) =>
            Db.ExecuteSP("sp_Categoria_Actualizar",
                Db.Param("@CategoriaId", id),
                Db.Param("@Nombre", nombre),
                Db.Param("@Descripcion", descripcion));
    }

    // ══════════════════════════════════════════════════════════════
    // MARCAS
    // ══════════════════════════════════════════════════════════════
    public static class MarcaService
    {
        public static DataTable ObtenerTodas() =>
            Db.QuerySP("sp_Marca_ObtenerTodas");

        public static void Insertar(string nombre, string descripcion) =>
            Db.ExecuteSP("sp_Marca_Insertar",
                Db.Param("@Nombre", nombre),
                Db.Param("@Descripcion", descripcion));

        public static void Actualizar(int id, string nombre, string descripcion) =>
            Db.ExecuteSP("sp_Marca_Actualizar",
                Db.Param("@MarcaId", id),
                Db.Param("@Nombre", nombre),
                Db.Param("@Descripcion", descripcion));
    }

    // ══════════════════════════════════════════════════════════════
    // UBICACIONES
    // ══════════════════════════════════════════════════════════════
    public static class UbicacionService
    {
        public static DataTable ObtenerTodas() =>
            Db.QuerySP("sp_Ubicacion_ObtenerTodas");

        public static void Insertar(string nombre, string descripcion) =>
            Db.ExecuteSP("sp_Ubicacion_Insertar",
                Db.Param("@Nombre", nombre),
                Db.Param("@Descripcion", descripcion));

        public static void Actualizar(int id, string nombre, string descripcion) =>
            Db.ExecuteSP("sp_Ubicacion_Actualizar",
                Db.Param("@UbicacionId", id),
                Db.Param("@Nombre", nombre),
                Db.Param("@Descripcion", descripcion));
    }

    // ══════════════════════════════════════════════════════════════
    // HERRAMIENTAS
    // ══════════════════════════════════════════════════════════════
    public static class HerramientaService
    {
        public static DataTable ObtenerTodas() =>
            Db.QuerySP("sp_Herramienta_ObtenerTodas");

        public static DataTable ObtenerPorId(int id) =>
            Db.QuerySP("sp_Herramienta_ObtenerPorId",
                Db.Param("@HerramientaId", id));

        public static DataTable ObtenerDisponibles() =>
            Db.QuerySP("sp_Herramienta_ObtenerDisponibles");

        public static DataTable Buscar(string termino) =>
            Db.QuerySP("sp_Herramienta_Buscar",
                Db.Param("@Termino", termino));

        public static int Insertar(
            string codigo, string nombre, string caracteristicas,
            int? categoriaId, int? marcaId, int? ubicacionId, int stockTotal)
        {
            var result = Db.ExecuteSPScalar("sp_Herramienta_Insertar",
                Db.Param("@Codigo", codigo),
                Db.Param("@Nombre", nombre),
                Db.Param("@Caracteristicas", caracteristicas),
                Db.Param("@CategoriaId", (object)categoriaId ?? DBNull.Value),
                Db.Param("@MarcaId", (object)marcaId ?? DBNull.Value),
                Db.Param("@UbicacionId", (object)ubicacionId ?? DBNull.Value),
                Db.Param("@StockTotal", stockTotal));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        public static void Actualizar(
            int id, string codigo, string nombre, string caracteristicas,
            int? categoriaId, int? marcaId, int? ubicacionId, int stockTotal) =>
            Db.ExecuteSP("sp_Herramienta_Actualizar",
                Db.Param("@HerramientaId", id),
                Db.Param("@Codigo", codigo),
                Db.Param("@Nombre", nombre),
                Db.Param("@Caracteristicas", caracteristicas),
                Db.Param("@CategoriaId", (object)categoriaId ?? DBNull.Value),
                Db.Param("@MarcaId", (object)marcaId ?? DBNull.Value),
                Db.Param("@UbicacionId", (object)ubicacionId ?? DBNull.Value),
                Db.Param("@StockTotal", stockTotal));

        public static void DarDeBaja(int id) =>
            Db.ExecuteSP("sp_Herramienta_DarDeBaja",
                Db.Param("@HerramientaId", id));
    }
}