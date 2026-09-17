using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class Usuario
    {
        [Key]
        public int id_usuario { get; set; }

        public string nombre_usuario { get; set; }

        public string contrasena_hash { get; set; }

        public int id_rol { get; set; }
    }
}