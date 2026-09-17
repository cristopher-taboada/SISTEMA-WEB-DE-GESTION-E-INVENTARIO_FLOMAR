using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class Usuario
    {
        [Key] 
        public int id_usuario { get; set; } 

        public string nombre_usuario { get; set; } = string.Empty; 
        
        public string contrasena_hash { get; set; } = string.Empty; 

       
        public string nombre_completo { get; set; } = string.Empty;
        public int id_rol { get; set; }
        public int id_estado_usuario { get; set; }
    }
}