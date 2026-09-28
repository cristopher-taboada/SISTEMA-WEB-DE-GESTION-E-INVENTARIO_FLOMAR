namespace FLOMAR.Models
{
    public class DashboardViewModel
    {
        public int TotalRepuestos { get; set; }
        public int TotalUsuarios { get; set; }
        public int StockBajo { get; set; }
        public decimal VentasHoy { get; set; }

        public List<Repuesto> UltimosRepuestos { get; set; } = new List<Repuesto>();
        public List<Repuesto> AlertasStock { get; set; } = new List<Repuesto>();
    }
}