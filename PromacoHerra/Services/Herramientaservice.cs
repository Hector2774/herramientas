// Services/HerramientaService.cs
// Cubre: Herramientas, Categorías, Marcas y Ubicaciones

using System.Data;
using PromacoHerra.Data;
using PromacoHerra.Models;
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

        /// <summary>Elimina la categoría; sus herramientas quedan sin categoría.</summary>
        public static void Eliminar(int id) =>
            Db.ExecuteSP("sp_Categoria_Eliminar", Db.Param("@CategoriaId", id));
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

        /// <summary>Elimina la marca; sus herramientas quedan sin marca.</summary>
        public static void Eliminar(int id) =>
            Db.ExecuteSP("sp_Marca_Eliminar", Db.Param("@MarcaId", id));
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

        /// <summary>Elimina la ubicación; sus herramientas quedan sin ubicación.</summary>
        public static void Eliminar(int id) =>
            Db.ExecuteSP("sp_Ubicacion_Eliminar", Db.Param("@UbicacionId", id));
    }

    // ══════════════════════════════════════════════════════════════
    // HERRAMIENTAS
    // ══════════════════════════════════════════════════════════════
    public static class HerramientaService
    {
        public static DataTable ObtenerTodas() =>
            Db.QuerySP("sp_Herramienta_ObtenerTodas");

        /// <summary>Herramientas activas con su stock, para la lista de FrmHerramientas.</summary>
        public static List<HerramientaResumen> Listar()
        {
            var lista = new List<HerramientaResumen>();
            foreach (DataRow r in ObtenerTodas().Rows)
            {
                lista.Add(new HerramientaResumen
                {
                    HerramientaId = Convert.ToInt32(r["HerramientaId"]),
                    Codigo = r["Codigo"].ToString() ?? "",
                    Nombre = r["Nombre"].ToString() ?? "",
                    Caracteristicas = r["Caracteristicas"] as string ?? "",
                    Categoria = r["Categoria"] as string ?? "",
                    Marca = r["Marca"] as string ?? "",
                    Ubicacion = r["Ubicacion"] as string ?? "",
                    CategoriaId = r["CategoriaId"] as int?,
                    MarcaId = r["MarcaId"] as int?,
                    UbicacionId = r["UbicacionId"] as int?,
                    PrestamoHabilitado = Convert.ToBoolean(r["PrestamoHabilitado"]),
                    StockTotal = Convert.ToInt32(r["StockTotal"]),
                    StockDisponible = Convert.ToInt32(r["StockDisponible"]),
                    StockPrestado = Convert.ToInt32(r["StockPrestado"]),
                    StockMantenimiento = Convert.ToInt32(r["StockMantenimiento"]),
                    StockDañado = Convert.ToInt32(r["StockDañado"])
                });
            }
            return lista;
        }

        public static DataTable ObtenerPorId(int id) =>
            Db.QuerySP("sp_Herramienta_ObtenerPorId",
                Db.Param("@HerramientaId", id));

        public static DataTable ObtenerDisponibles() =>
            Db.QuerySP("sp_Herramienta_ObtenerDisponibles");

        /// <summary>
        /// Catálogo para la pantalla de préstamos: una tarjeta por herramienta prestable
        /// (incluidas las que no tienen unidades disponibles) con sus unidades disponibles.
        /// </summary>
        public static List<HerramientaCatalogoItem> ObtenerCatalogoPrestamo()
        {
            var catalogo = new List<HerramientaCatalogoItem>();
            var porId = new Dictionary<int, HerramientaCatalogoItem>();

            foreach (DataRow r in Db.QuerySP("sp_Herramienta_ObtenerCatalogoPrestamo").Rows)
            {
                var item = new HerramientaCatalogoItem
                {
                    HerramientaId = Convert.ToInt32(r["HerramientaId"]),
                    Codigo = r["Codigo"].ToString() ?? "",
                    Nombre = r["Nombre"].ToString() ?? "",
                    Categoria = r["Categoria"] as string ?? "",
                    Marca = r["Marca"] as string ?? "",
                    Ubicacion = r["Ubicacion"] as string ?? "",
                    StockTotal = Convert.ToInt32(r["StockTotal"])
                };
                catalogo.Add(item);
                porId[item.HerramientaId] = item;
            }

            // Una fila por unidad disponible, ya ordenadas por número de unidad
            foreach (DataRow r in ObtenerDisponibles().Rows)
            {
                if (!porId.TryGetValue(Convert.ToInt32(r["HerramientaId"]), out var item)) continue;
                item.Unidades.Add(new UnidadDisponible
                {
                    UnidadId = Convert.ToInt32(r["UnidadId"]),
                    Codigo = r["Codigo"].ToString() ?? "",
                    Ubicacion = r["Ubicacion"] as string ?? ""
                });
            }

            return catalogo;
        }

        public static DataTable Buscar(string termino) =>
            Db.QuerySP("sp_Herramienta_Buscar",
                Db.Param("@Termino", termino));

        /// <summary>
        /// El código (HER-0001, HER-0002, ...) lo genera la base de datos.
        /// </summary>
        public static int Insertar(
            string nombre, string caracteristicas,
            int? categoriaId, int? marcaId, int? ubicacionId, int stockTotal)
        {
            var result = Db.ExecuteSPScalar("sp_Herramienta_Insertar",
                Db.Param("@Nombre", nombre),
                Db.Param("@Caracteristicas", caracteristicas),
                Db.Param("@CategoriaId", (object)categoriaId ?? DBNull.Value),
                Db.Param("@MarcaId", (object)marcaId ?? DBNull.Value),
                Db.Param("@UbicacionId", (object)ubicacionId ?? DBNull.Value),
                Db.Param("@StockTotal", stockTotal));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// stockTotal null = no tocar el stock (las unidades se administran en FrmUnidades).
        /// </summary>
        public static void Actualizar(
            int id, string nombre, string caracteristicas,
            int? categoriaId, int? marcaId, int? ubicacionId, int? stockTotal = null) =>
            Db.ExecuteSP("sp_Herramienta_Actualizar",
                Db.Param("@HerramientaId", id),
                Db.Param("@Nombre", nombre),
                Db.Param("@Caracteristicas", caracteristicas),
                Db.Param("@CategoriaId", (object)categoriaId ?? DBNull.Value),
                Db.Param("@MarcaId", (object)marcaId ?? DBNull.Value),
                Db.Param("@UbicacionId", (object)ubicacionId ?? DBNull.Value),
                Db.Param("@StockTotal", (object?)stockTotal ?? DBNull.Value));

        /// <summary>
        /// Da de baja el grupo completo (todas sus unidades) y lo oculta.
        /// Para dar de baja una sola unidad usar CambiarEstadoUnidades.
        /// </summary>
        public static void DarDeBaja(int id) =>
            Db.ExecuteSP("sp_Herramienta_DarDeBaja",
                Db.Param("@HerramientaId", id));

        /// <summary>
        /// Suspende (false) o reanuda (true) el préstamo de todo el grupo.
        /// </summary>
        public static void HabilitarPrestamo(int id, bool habilitar) =>
            Db.ExecuteSP("sp_Herramienta_HabilitarPrestamo",
                Db.Param("@HerramientaId", id),
                Db.Param("@Habilitar", habilitar));

        // ── Unidades físicas ───────────────────────────────────────

        public static DataTable ObtenerUnidades(int herramientaId) =>
            Db.QuerySP("sp_Herramienta_ObtenerUnidades",
                Db.Param("@HerramientaId", herramientaId));

        public static void AgregarUnidades(int herramientaId, int cantidad) =>
            Db.ExecuteSP("sp_Herramienta_AgregarUnidades",
                Db.Param("@HerramientaId", herramientaId),
                Db.Param("@Cantidad", cantidad));

        /// <summary>
        /// Cambia el estado de unidades: 'Disponible', 'Dañada', 'Perdida' o 'Baja'.
        /// unidadIds null = todo el grupo (se omiten las prestadas / en mantenimiento).
        /// Devuelve cuántas unidades cambiaron y cuántas se omitieron.
        /// </summary>
        public static (int Afectadas, int Omitidas) CambiarEstadoUnidades(
            int herramientaId, IEnumerable<int>? unidadIds, string nuevoEstado, string? observacion = null)
        {
            var dt = Db.QuerySP("sp_Unidad_CambiarEstado",
                Db.Param("@HerramientaId", herramientaId),
                Db.Param("@UnidadIds", unidadIds == null ? DBNull.Value : string.Join(",", unidadIds)),
                Db.Param("@NuevoEstado", nuevoEstado),
                Db.Param("@Observacion", (object?)observacion ?? DBNull.Value));

            return dt.Rows.Count > 0
                ? (Convert.ToInt32(dt.Rows[0]["Afectadas"]), Convert.ToInt32(dt.Rows[0]["Omitidas"]))
                : (0, 0);
        }
    }
}