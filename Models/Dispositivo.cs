using System;
using System.ComponentModel.DataAnnotations;

namespace EnergySaver.Models // <--- ESTO ES LO QUE TE FALTA
{
    public class Dispositivo
    {
        [Key]
        public int id_dispositivo { get; set; }

        [Required] // Esto asegura que no mandes nombres vacíos
        public string nombre { get; set; }

        public string? tipo { get; set; }
        public string? marca { get; set; }
        public double? consumoWatts { get; set; }
        public string? ubicacion { get; set; }
        public string? estado { get; set; }

        // Le quitamos el '?' y le ponemos un valor por defecto 
        // para que C# nunca mande un nulo a SQL en la fecha
        public DateTime fechaRegistro { get; set; } = DateTime.Now;
    }
}