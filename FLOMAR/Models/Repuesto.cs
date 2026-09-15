using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class Repuesto
    {
        // ID único del repuesto
        public int Id { get; set; }


        // CÓDIGO DEL REPUESTO
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50, ErrorMessage = "El código no puede superar los 50 caracteres")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;


        // NOMBRE DEL REPUESTO
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;


        // CATEGORÍA
        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = string.Empty;


        // PRECIO DE COMPRA
        [Range(0, 999999, ErrorMessage = "El costo no puede ser negativo")]
        [Display(Name = "Costo")]
        public decimal Costo { get; set; }


        // PRECIO DE VENTA
        [Range(0, 999999, ErrorMessage = "El precio de venta no puede ser negativo")]
        [Display(Name = "Precio de Venta")]
        public decimal PrecioVenta { get; set; }


        // CANTIDAD DISPONIBLE
        [Range(0, 999999, ErrorMessage = "El stock no puede ser negativo")]
        [Display(Name = "Stock Actual")]
        public int Stock { get; set; }


        // CANTIDAD MÍNIMA PERMITIDA
        [Range(0, 999999, ErrorMessage = "El stock mínimo no puede ser negativo")]
        [Display(Name = "Stock Mínimo")]
        public int StockMinimo { get; set; }


        // ESTADO DEL REPUESTO
        // true  = Activo
        // false = Desactivado
        public bool Activo { get; set; } = true;
    }
}