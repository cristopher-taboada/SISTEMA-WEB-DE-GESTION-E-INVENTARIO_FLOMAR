namespace FLOMAR.Models
{
    public class Venta
    {
        public int id_venta { get; set; }
        public string numero_transaccion { get; set; } = string.Empty;
        public DateTime Fecha_hora { get; set; }
        public int id_cliente { get; set; }
        public int Id_vendedor { get; set; }
        public int id_metodo_pago { get; set; }
        public int id_impuesto { get; set; }
        public int id_estado_venta { get; set; }
        public decimal sub_total { get; set; }
        public decimal monto_impuesto { get; set; }
        public decimal total_venta { get; set; }
        public string tipo_comprobante { get; set; } = string.Empty;
    }
}
