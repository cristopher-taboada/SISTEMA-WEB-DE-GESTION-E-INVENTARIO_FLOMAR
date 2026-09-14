using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    public class Repuesto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Range(0, 9999999)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Costo { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, 9999999)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, 999999)]
        public int Stock { get; set; }

        [Required]
        [Range(0, 999999)]
        [Display(Name = "Stock mínimo")]
        public int StockMinimo { get; set; }

        public bool Activo { get; set; } = true;
    }
}