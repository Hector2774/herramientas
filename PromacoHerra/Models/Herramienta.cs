namespace PromacoHerra.Models
{
    // Una herramienta (grupo) con su stock, para la lista y el detalle de FrmHerramientas
    public class HerramientaResumen
    {
        public int HerramientaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Caracteristicas { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public int? CategoriaId { get; set; }
        public int? MarcaId { get; set; }
        public int? UbicacionId { get; set; }
        public bool PrestamoHabilitado { get; set; }

        public int StockTotal { get; set; }
        public int StockDisponible { get; set; }
        public int StockPrestado { get; set; }
        public int StockMantenimiento { get; set; }
        public int StockDañado { get; set; }
    }
}
