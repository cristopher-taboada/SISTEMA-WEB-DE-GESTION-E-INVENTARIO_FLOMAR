namespace FlomarAPI.Models
{
    // Cabecera de una venta del punto de venta (tabla VENTA)
    public class Venta
    {
        public int id_venta { get; set; }
        public string numero_transaccion { get; set; } = string.Empty;
        public DateTime fecha_hora { get; set; }
        public int? id_cliente { get; set; }          // NULL = cliente mostrador
        public int id_vendedor { get; set; }
        public int id_metodo_pago { get; set; }
        public int id_impuesto { get; set; }
        public int id_estado_venta { get; set; }
        public decimal subtotal { get; set; }
        public decimal monto_impuesto { get; set; }
        public decimal total_venta { get; set; }
        public string tipo_comprobante { get; set; } = "Recibo Interno";
    }
}
