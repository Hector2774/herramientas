// Models/EmpleadoDTO.cs
using System.Text.Json.Serialization;

namespace PromacoHerra.Models
{
    public class EmpleadoDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("departamento")]
        public string Departamento { get; set; }
    }
}