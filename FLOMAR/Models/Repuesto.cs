using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class Repuesto
    {
        // ID único del repuesto
        public int id_repuesto { get; set; }

        // Alias para vistas/controlador que usan "Id"
        public int Id => id_repuesto;

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

        // CATEGORÍA (int para BD, string para vistas)
        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int id_categoria { get; set; }

        // Alias "Categoria" como string que las vistas usan
        [Display(Name = "Categoría")]
        public string Categoria
        {
            get
            {
                return id_categoria switch
                {
                    1 => "Frenos",
                    2 => "Motor",
                    3 => "Encendido",
                    4 => "Suspensión",
                    5 => "Eléctrico",
                    _ => "Otra"
                };
            }
        }

        // PRECIO DE COMPRA
        [Range(0, 999999, ErrorMessage = "El costo no puede ser negativo")]
        [Display(Name = "Costo")]
        public decimal costo_adquisicion { get; set; }

        // Alias "Costo" que las vistas usan
        [Display(Name = "Costo")]
        public decimal Costo => costo_adquisicion;

        // PRECIO DE VENTA
        [Range(0, 999999, ErrorMessage = "El precio de venta no puede ser negativo")]
        [Display(Name = "Precio de Venta")]
        public decimal PrecioVenta { get; set; }

        // CANTIDAD DISPONIBLE
        [Range(0, 999999, ErrorMessage = "El stock no puede ser negativo")]
        [Display(Name = "Stock Actual")]
        public int stock_actual { get; set; }

        // Alias "Stock" que las vistas usan
        [Display(Name = "Stock Actual")]
        public int Stock => stock_actual;

        // CANTIDAD MÍNIMA PERMITIDA
        [Range(0, 999999, ErrorMessage = "El stock mínimo no puede ser negativo")]
        [Display(Name = "Stock Mínimo")]
        public int stock_minimo { get; set; }

        // Alias "StockMinimo" que las vistas usan
        [Display(Name = "Stock Mínimo")]
        public int StockMinimo => stock_minimo;

        // ESTADO DEL REPUESTO
        // 1 = Activo, otro = Desactivado
        public int id_estado { get; set; }

        // Alias "Activo" que las vistas/controlador usan
        public bool Activo => id_estado == 1;
    }
}
