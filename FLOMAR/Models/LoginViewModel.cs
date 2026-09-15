
using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;
    }
}
