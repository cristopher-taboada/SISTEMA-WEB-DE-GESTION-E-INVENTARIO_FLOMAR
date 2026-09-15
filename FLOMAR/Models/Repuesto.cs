using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("REPUESTO")]
    public class Repuesto
    {
        [Key]
        [Column("id_repuesto")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(30)]
        [Column("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(120)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("id_categoria")]
        public int Id_categoria { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El costo no puede ser negativo")]
        [Column("costo_adquisicion")]
        public decimal costo_adquisicion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        [Column("precio_venta")]
        public decimal precio_venta { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        [Column("stock_actual")]
        public int stock_actual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        [Column("stock_minimo")]
        public int stock_minimo { get; set; }

        [Column("id_estado_repuesto")]
        public int id_estado_repuesto { get; set; } = 1;
    }
}