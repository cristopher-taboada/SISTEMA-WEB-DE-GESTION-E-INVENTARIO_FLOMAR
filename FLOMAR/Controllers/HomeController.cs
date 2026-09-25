using FLOMAR.Data;
using FLOMAR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;

namespace FLOMAR.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlomarContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(FlomarContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (LoginController.RolActual == 0)
                return RedirectToAction("Index", "Login");

            var totalRepuestos = await _context.Repuestos
                .CountAsync(r => r.id_estado == 1);

            var totalUsuarios = await _context.Usuarios
                .CountAsync(u => u.id_estado_usuario == 1);

            var stockBajo = await _context.Repuestos
                .CountAsync(r => r.id_estado == 1 && r.stock_actual <= r.stock_minimo);

            var hoy = DateTime.Today;

            var ventasHoy = await _context.Ventas
                .Where(v => v.Fecha_hora.Date == hoy)
                .SumAsync(v => (decimal?)v.total_venta) ?? 0;

            var ultimosRepuestos = await _context.Repuestos
                .Where(r => r.id_estado == 1)
                .OrderByDescending(r => r.id_repuesto)
                .Take(5)
                .ToListAsync();

            var alertasStock = await _context.Repuestos
                .Where(r => r.id_estado == 1 && r.stock_actual <= r.stock_minimo)
                .OrderBy(r => r.stock_actual)
                .Take(5)
                .ToListAsync();

            var modelo = new DashboardViewModel
            {
                TotalRepuestos = totalRepuestos,
                TotalUsuarios = totalUsuarios,
                StockBajo = stockBajo,
                VentasHoy = ventasHoy,
                UltimosRepuestos = ultimosRepuestos,
                AlertasStock = alertasStock
            };

            return View(modelo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Admin()
        {
            if (LoginController.RolActual != 1)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public IActionResult Vendedor()
        {
            if (LoginController.RolActual != 2)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }






        // aqi empieza el codigo de reportes
        public async Task<IActionResult> Reportes()
        {
            if (LoginController.RolActual != 1) return RedirectToAction("Index", "Login");

            var api = _httpClientFactory.CreateClient("FlomarAPI");

            var ventas = await api.GetFromJsonAsync<List<Venta>>("api/ventas") ?? new List<Venta>();
            var usuarios = await api.GetFromJsonAsync<List<Usuario>>("api/usuarios") ?? new List<Usuario>();
            var compras = await api.GetFromJsonAsync<List<CompraReporteView>>("api/compras/reporte") ?? new List<CompraReporteView>();

            ViewBag.Ventas = ventas.Where(v => v.id_metodo_pago == 1 && v.Fecha_hora.Date == DateTime.Today)
                .Join(usuarios, v => v.Id_vendedor, u => u.id_usuario, (v, u) => new { u.nombre_completo, v.total_venta }).ToList();

            ViewBag.Compras = compras;

            return View();
        }
    }
}