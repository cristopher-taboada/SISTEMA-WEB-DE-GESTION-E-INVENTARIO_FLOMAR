using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class CompraCreateViewModel
    {
        [Required(ErrorMessage = "El número de compra es obligatorio.")]
        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "El número de compra debe tener entre 5 y 20 caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9\-]+$",
            ErrorMessage = "El número de compra solo puede contener letras, números y guiones.")]
        public string numero_compra { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        public DateTime fecha_ingreso { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
        public int id_proveedor { get; set; }

        [StringLength(200, ErrorMessage = "Las observaciones no pueden superar los 200 caracteres.")]
        public string observaciones { get; set; } = string.Empty;
    }
}
