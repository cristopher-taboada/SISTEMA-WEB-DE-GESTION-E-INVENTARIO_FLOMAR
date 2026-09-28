using FlomarAPI.Data;
using FlomarAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    // Datos del panel de administracion. Unica fuente de la informacion
    // de la base de datos para el MVC (nunca consulta MySQL directamente).
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly FlomarContext _context;

        public DashboardController(FlomarContext context)
        {
            _context = context;
        }

        // RESUMEN DEL PANEL - GET: api/dashboard/resumen
        [HttpGet("resumen")]
        public async Task<ActionResult<ResumenDashboardDto>> Resumen()
        {
            var repuestosActivos = _context.Repuestos.Where(r => r.id_estado == 1);
            var hoy = DateTime.Today;

            var resumen = new ResumenDashboardDto
            {
                total_repuestos = await repuestosActivos.CountAsync(),

                total_usuarios = await _context.Usuarios
                    .CountAsync(u => u.id_estado_usuario == 1),

                stock_bajo = await repuestosActivos
                    .CountAsync(r => r.stock_actual <= r.stock_minimo),

                // Ventas del dia (solo las confirmadas)
                ventas_hoy = await _context.Ventas
                    .Where(v => v.fecha_hora.Date == hoy && v.id_estado_venta == 1)
                    .SumAsync(v => (decimal?)v.total_venta) ?? 0,

                ultimos_repuestos = await repuestosActivos
                    .OrderByDescending(r => r.id_repuesto)
                    .Take(5)
                    .ToListAsync(),

                alertas_stock = await repuestosActivos
                    .Where(r => r.stock_actual <= r.stock_minimo)
                    .OrderBy(r => r.stock_actual)
                    .Take(5)
                    .ToListAsync()
            };

            return resumen;
        }
    }
}
