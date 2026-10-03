namespace PromacoHerra.Models
{
    // Un préstamo con herramientas pendientes de devolver (lista izquierda de Devoluciones)
    public class PrestamoActivo
    {
        public int PrestamoId { get; set; }
        public string Empleado { get; set; } = string.Empty;
        public string CodigoEmpleado { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public int Pendientes { get; set; }

        // Color del avatar, fijo por posición en la lista cargada
        public int ColorIndex { get; set; }

        public string Codigo => $"PR-{PrestamoId:0000}";

        // Días hasta la fecha límite (negativo = vencido), comparando solo fechas
        public int DiasRestantes => (FechaDevolucionEsperada.Date - DateTime.Today).Days;
        public bool Vencido => DiasRestantes < 0;
    }

    // Una unidad prestada que falta devolver
    public class DetallePendiente
    {
        public int PrestamoDetalleId { get; set; }
        public int UnidadId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Herramienta { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string? FotoNombre { get; set; }
    }

    public enum CondicionDevolucion { Bueno, Dañado, Perdido }

    // Lo que se envía al registrar: qué unidad, en qué condición y con qué nota
    public class DevolucionItem
    {
        public int PrestamoDetalleId { get; set; }
        public CondicionDevolucion Condicion { get; set; }
        public string? Nota { get; set; }
    }

    public class ResultadoDevolucion
    {
        public int Buenas { get; set; }
        public int Dañadas { get; set; }
        public int Perdidas { get; set; }
        public int Pendientes { get; set; }
        public bool PrestamoCerrado { get; set; }
        public int Total => Buenas + Dañadas + Perdidas;
    }
}
