using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergySaver.Models
{
    [Table("Dispositivos")]
    public class Dispositivo
    {
        [Key]
        [Column("id_dispositivo")]
        public int id_dispositivo { get; set; }

        [Column("nombre")]
        public string nombre { get; set; } = "";

        [Column("tipo")]
        public string? tipo { get; set; }

        [Column("marca")]
        public string? marca { get; set; }

        [Column("consumoWatts")]
        public double? consumoWatts { get; set; }

        [Column("ubicacion")]
        public string? ubicacion { get; set; }

        [Column("estado")]
        public string? estado { get; set; }

        [Column("fechaRegistro")]
        public DateTime fechaRegistro { get; set; } = DateTime.Now;
    }
}