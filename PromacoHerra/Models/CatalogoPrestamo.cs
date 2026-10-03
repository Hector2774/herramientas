namespace PromacoHerra.Models
{
    // Una unidad física disponible para préstamo (ej. HER-0001-03)
    public class UnidadDisponible
    {
        public int UnidadId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string Estado { get; set; } = "Disponible";
    }

    // Una tarjeta del catálogo de préstamos: el tipo de herramienta y sus unidades disponibles
    public class HerramientaCatalogoItem
    {
        public int HerramientaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public int StockTotal { get; set; }
        public string? FotoNombre { get; set; }
        public List<UnidadDisponible> Unidades { get; } = new();

        public int Disponibles => Unidades.Count;
    }
}
