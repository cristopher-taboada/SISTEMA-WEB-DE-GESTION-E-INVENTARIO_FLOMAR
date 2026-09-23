using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlomarAPI.Models
{
    [Table("REPUESTO")]
    public class Repuesto
    {
        [Key]
        [Column("id_repuesto")]
        public int id_repuesto { get; set; }

        // Propiedad publica PascalCase para enlazar en las vistas (asp-for="Id")
        [NotMapped]
        public int Id
        {
            get => id_repuesto;
            set => id_repuesto = value;
        }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(30)]
        [Column("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(120)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría")]
        [Column("id_categoria")]
        public int id_categoria { get; set; }

        [NotMapped]
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

        [Range(0, 999999, ErrorMessage = "El costo no puede ser negativo")]
        [Column("costo_adquisicion", TypeName = "decimal(10,2)")]
        public decimal costo_adquisicion { get; set; }

        // Propiedad publica PascalCase para enlazar en las vistas (asp-for="Costo")
        [NotMapped]
        public decimal Costo
        {
            get => costo_adquisicion;
            set => costo_adquisicion = value;
        }

        [Range(0, 999999, ErrorMessage = "El precio no puede ser negativo")]
        [Column("precio_venta", TypeName = "decimal(10,2)")]
        public decimal PrecioVenta { get; set; }

        [Range(0, 999999, ErrorMessage = "El stock no puede ser negativo")]
        [Column("stock_actual")]
        public int stock_actual { get; set; }

        // Propiedad publica PascalCase para enlazar en las vistas (asp-for="Stock")
        [NotMapped]
        public int Stock
        {
            get => stock_actual;
            set => stock_actual = value;
        }

        [Range(0, 999999, ErrorMessage = "El stock mínimo no puede ser negativo")]
        [Column("stock_minimo")]
        public int stock_minimo { get; set; }

        // Propiedad publica PascalCase para enlazar en las vistas (asp-for="StockMinimo")
        [NotMapped]
        public int StockMinimo
        {
            get => stock_minimo;
            set => stock_minimo = value;
        }

        [Column("id_estado")]
        public int id_estado { get; set; } = 1;
    }
}
