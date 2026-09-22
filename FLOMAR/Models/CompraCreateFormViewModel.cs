using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class CompraCreateFormViewModel
    {
        [Required(ErrorMessage = "El numero de compra es obligatorio.")]
        [StringLength(20, MinimumLength = 5)]
        public string numero_compra { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        public DateTime fecha_ingreso { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor.")]
        public int id_proveedor { get; set; }

        [StringLength(200)]
        public string observaciones { get; set; } = string.Empty;

        public List<ProductoFilaViewModel> Productos { get; set; } = new List<ProductoFilaViewModel>();
    }

    public class ProductoFilaViewModel
    {
        public int id_repuesto { get; set; }
        public string codigo { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public decimal costo_unitario { get; set; }

        [Range(0, int.MaxValue)]
        public int cantidad { get; set; }
    }
}
