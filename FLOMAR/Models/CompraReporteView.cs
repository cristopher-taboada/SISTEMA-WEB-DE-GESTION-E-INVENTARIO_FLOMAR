namespace FLOMAR.Models
{
    public class CompraReporteView
    {
        public DateTime fecha { get; set; }
        public string nro { get; set; } = string.Empty;
        public string proveedor { get; set; } = string.Empty;
        public string repuesto { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public decimal precio { get; set; }
    }
}