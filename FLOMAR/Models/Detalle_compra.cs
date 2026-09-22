namespace FLOMAR.Models
{
    public class Detalle_compra
    {
        public int Id_detalle_compra { get; set; }
        public int id_compra { get; set; }
        public int id_repuesto { get; set; }
        public int cantidad { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal subtotal { get; set; }
    }
}
