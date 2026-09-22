using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("REPUESTO")]
    public class Repuesto
    {
        // =========================
        // ID
        // =========================

        [Key]
        [Column("id_repuesto")]
        public int id_repuesto { get; set; }

        [NotMapped]
        public int Id => id_repuesto;


        // =========================
        // CÓDIGO
        // =========================

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(30, ErrorMessage = "El código no puede superar los 30 caracteres")]
        [Column("codigo")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;


        // =========================
        // NOMBRE
        // =========================

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres")]
        [Column("nombre")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;


        // =========================
        // CATEGORÍA
        // =========================

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría")]
        [Column("id_categoria")]
        [Display(Name = "Categoría")]
        public int id_categoria { get; set; }

        [NotMapped]
        [Display(Name = "Categoría")]
        public string Categoria
        {
            get
            {
                return id_categoria switch
                {
                    1 => "Frenos",
                    2 => "Suspensión",
                    3 => "Motor",
                    4 => "Eléctrico",
                    5 => "Aceites",
                    _ => "Otra"
                };
            }
        }


        // =========================
        // COSTO
        // =========================

        [Range(0, 999999, ErrorMessage = "El costo no puede ser negativo")]
        [Column("costo_adquisicion", TypeName = "decimal(10,2)")]
        [Display(Name = "Costo")]
        public decimal costo_adquisicion { get; set; }

        [NotMapped]
        [Display(Name = "Costo")]
        public decimal Costo => costo_adquisicion;


        // =========================
        // PRECIO DE VENTA
        // =========================

        [Range(0, 999999, ErrorMessage = "El precio de venta no puede ser negativo")]
        [Column("precio_venta", TypeName = "decimal(10,2)")]
        [Display(Name = "Precio de Venta")]
        public decimal PrecioVenta { get; set; }


        // =========================
        // STOCK ACTUAL
        // =========================

        [Range(0, 999999, ErrorMessage = "El stock no puede ser negativo")]
        [Column("stock_actual")]
        [Display(Name = "Stock Actual")]
        public int stock_actual { get; set; }

        [NotMapped]
        [Display(Name = "Stock Actual")]
        public int Stock => stock_actual;


        // =========================
        // STOCK MÍNIMO
        // =========================

        [Range(0, 999999, ErrorMessage = "El stock mínimo no puede ser negativo")]
        [Column("stock_minimo")]
        [Display(Name = "Stock Mínimo")]
        public int stock_minimo { get; set; }

        [NotMapped]
        [Display(Name = "Stock Mínimo")]
        public int StockMinimo => stock_minimo;


        // =========================
        // ESTADO
        // =========================

        [Column("id_estado")]
        public int id_estado { get; set; } = 1;

        [NotMapped]
        public bool Activo => id_estado == 1;
    }
}