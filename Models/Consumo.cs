using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergySaver.Models
{
    [Table("Consumo")]
    public class Consumo
    {
        [Key]
        [Column("id_consumo")]
        public int id_consumo { get; set; }

        [Column("id_usuario")]
        public int id_usuario { get; set; }

        [Column("id_dispositivo")]
        public int id_dispositivo { get; set; }

        [Column("fecha")]
        public DateTime fecha { get; set; }

        [Column("valor")]
        public double valor { get; set; }
    }
}