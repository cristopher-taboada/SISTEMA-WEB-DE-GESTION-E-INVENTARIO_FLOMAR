        using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría")]
        [Column("id_categoria")]
        [Display(Name = "Categoría")]
        public int IdCategoria { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        public Categoria? Categoria { get; set; }

        [Range(0, 999999)]
        [Column("costo_adquisicion", TypeName = "decimal(10,2)")]
        [Display(Name = "Costo")]
        public decimal Costo { get; set; }

        [Range(0, 999999)]
        [Column("precio_venta", TypeName = "decimal(10,2)")]
        [Display(Name = "Precio de Venta")]
        public decimal PrecioVenta { get; set; }

        [Range(0, 999999)]
        [Column("stock_actual")]
        [Display(Name = "Stock Actual")]
        public int Stock { get; set; }

        [Range(0, 999999)]
        [Column("stock_minimo")]
        [Display(Name = "Stock Mínimo")]
        public int StockMinimo { get; set; }

        [Column("id_estado_repuesto")]
        public int IdEstadoRepuesto { get; set; } = 1;

        [ForeignKey(nameof(IdEstadoRepuesto))]
        public Estado_respuesto? EstadoRepuesto { get; set; }

        [NotMapped]
        public bool Activo
        {
            get => IdEstadoRepuesto == 1;
            set => IdEstadoRepuesto = value ? 1 : 2;
        }
    }
}
