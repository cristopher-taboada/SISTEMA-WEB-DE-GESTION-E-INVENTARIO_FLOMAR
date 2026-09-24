namespace FLOMAR.Models
{
    // ====== Lo que el carrito (JavaScript) envia al MVC y el MVC a la API ======
    public class VentaLineaRequest
    {
        public int id_repuesto { get; set; }
        public int cantidad { get; set; }
    }

    public class VentaCrearRequest
    {
        public int id_vendedor { get; set; }
        public int id_metodo_pago { get; set; } = 1;   // 1 = Efectivo
        public List<VentaLineaRequest> lineas { get; set; } = new();
    }

    // ====== Comprobante que devuelve la API (coincide con VentaDetalleDto) ======
    public class VentaDetalleItemViewModel
    {
        public string codigo { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal subtotal { get; set; }
    }

    public class VentaDetalleViewModel
    {
        public int id_venta { get; set; }
        public string numero_transaccion { get; set; } = string.Empty;
        public DateTime fecha_hora { get; set; }
        public string vendedor { get; set; } = string.Empty;
        public string metodo_pago { get; set; } = string.Empty;
        public string tipo_comprobante { get; set; } = string.Empty;
        public decimal subtotal { get; set; }
        public decimal monto_impuesto { get; set; }
        public decimal total_venta { get; set; }
        public List<VentaDetalleItemViewModel> detalles { get; set; } = new();
    }
}
