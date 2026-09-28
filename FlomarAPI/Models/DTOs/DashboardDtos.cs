using FlomarAPI.Models;

namespace FlomarAPI.Models.DTOs
{
    // Resumen del panel principal de administracion.
    // Lo consume HomeController (Index) del MVC; antes ese panel se armaba
    // con EF Core directo en el MVC y ahora todo pasa por la API.
    public class ResumenDashboardDto
    {
        public int total_repuestos { get; set; }
        public int total_usuarios { get; set; }
        public int stock_bajo { get; set; }
        public decimal ventas_hoy { get; set; }

        public List<Repuesto> ultimos_repuestos { get; set; } = new();
        public List<Repuesto> alertas_stock { get; set; } = new();
    }
}
