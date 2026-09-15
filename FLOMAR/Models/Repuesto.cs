using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class Repuesto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50, ErrorMessage = "El código no puede tener más de 50 caracteres")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;   

        public int Id_categoria { get; set; }
        public decimal costo_adquisicion { get; set; }
        public decimal precio_venta { get; set; }
        public int stock_actual { get; set; }
        public int stock_minimo { get; set; }
        public int id_estado_respuesto { get; set; }
    }
}
