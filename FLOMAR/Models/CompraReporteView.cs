namespace FLOMAR.Models
{
    public class CompraReporteView
    {
        public int Id_compra { get; set; }
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public decimal monto_total { get; set; }
        public string proveedor { get; set; } = string.Empty;
    }
}
