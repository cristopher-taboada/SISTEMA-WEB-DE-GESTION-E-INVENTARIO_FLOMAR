using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class VentaStockViewModel
    {
        [Required]
        public int IdRepuesto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public decimal PrecioVenta { get; set; }

        public int StockActual { get; set; }
    }
}