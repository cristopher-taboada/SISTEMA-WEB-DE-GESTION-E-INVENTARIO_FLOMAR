namespace FLOMAR.Models
{
    public class CompraDetalleViewModel
    {
        // Datos de la compra (cabecera)
        public int Id_compra { get; set; }
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public string proveedor { get; set; } = string.Empty;
        public string registrado_por { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
        public decimal monto_total { get; set; }

        // Líneas de la compra (detalle)
        public List<DetalleCompraItem> Detalles { get; set; } = new List<DetalleCompraItem>();
    }

    public class DetalleCompraItem
    {
        public string repuesto { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal subtotal { get; set; }
    }
}
