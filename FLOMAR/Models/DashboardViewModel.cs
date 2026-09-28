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

    // Respuesta de GET api/dashboard/resumen (la API hace las consultas a MySQL)
    public class ResumenDashboardViewModel
    {
        public int total_repuestos { get; set; }
        public int total_usuarios { get; set; }
        public int stock_bajo { get; set; }
        public decimal ventas_hoy { get; set; }

        public List<Repuesto> ultimos_repuestos { get; set; } = new();
        public List<Repuesto> alertas_stock { get; set; } = new();
    }

}