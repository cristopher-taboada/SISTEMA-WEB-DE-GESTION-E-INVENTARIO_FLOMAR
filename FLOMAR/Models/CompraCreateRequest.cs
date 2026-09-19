namespace FLOMAR.Models
{
    public class CompraCreateRequest
    {
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public int id_proveedor { get; set; }
        public int id_usuario { get; set; }
        public string observaciones { get; set; } = string.Empty;
        public decimal monto_total { get; set; }
        public List<DetalleCompraItemRequest> Detalles { get; set; } = new List<DetalleCompraItemRequest>();
    }

    public class DetalleCompraItemRequest
    {
        public int id_repuesto { get; set; }
        public int cantidad { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal subtotal { get; set; }
    }
}
