using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergySaver.Models
{
    [Table("Configuracion")]
    public class Configuracion
    {
       [Key]
        public int Id { get; set; }

        public decimal? TarifaCFE { get; set; }

        public decimal? Impuesto { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFin { get; set; }

        public double? LimiteConsumo { get; set; }
    }
}

