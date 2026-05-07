using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login_EnergySaver.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = "";

        [Column("correo")]
        public string Correo { get; set; } = "";

        [Column("contraseña")]
        public string Password { get; set; } = "";

        [Column("estado")]
        public string Rol { get; set; } = "";
    }
}