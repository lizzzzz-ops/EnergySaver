using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

<<<<<<< HEAD
namespace Login_EnergySaver.Models
=======
namespace EnergySaver.Models
>>>>>>> 7065bafb3b2de88c4632b6748e8432d70a0a39bc
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