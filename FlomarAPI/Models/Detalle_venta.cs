namespace FlomarAPI.Models
{
    // Linea de una venta: un repuesto, su cantidad y precio (tabla DETALLE_VENTA)
    public class Detalle_venta
    {
        public int id_detalle_venta { get; set; }
        public int id_venta { get; set; }
        public int id_repuesto { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal subtotal { get; set; }
    }
}
