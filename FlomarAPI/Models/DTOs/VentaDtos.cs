namespace FlomarAPI.Models.DTOs
{
    // Linea que envia el punto de venta: que repuesto y cuantos
    public class VentaLineaRequest
    {
        public int id_repuesto { get; set; }
        public int cantidad { get; set; }
    }

    // POST api/ventas - lo que envia el carrito del vendedor.
    // El precio NO se recibe: la API lo toma de la base de datos
    // para que nadie pueda alterar precios desde el navegador.
    public class VentaCrearRequest
    {
        public int id_vendedor { get; set; }
        public int id_metodo_pago { get; set; } = 1;   // 1 = Efectivo
        public List<VentaLineaRequest> lineas { get; set; } = new();
    }

    // Cada fila del comprobante de venta
    public class VentaDetalleItemDto
    {
        public string codigo { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal subtotal { get; set; }
    }

    // Comprobante completo (GET api/ventas/{id} y respuesta del POST)
    public class VentaDetalleDto
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
        public List<VentaDetalleItemDto> detalles { get; set; } = new();
    }
}
