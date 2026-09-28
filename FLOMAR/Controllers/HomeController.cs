using FLOMAR.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace FLOMAR.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // PANEL PRINCIPAL (administrador): todos los datos vienen de la API,
        // el MVC ya no se conecta a MySQL.
        public async Task<IActionResult> Index()
        {
            if (LoginController.RolActual == 0)
                return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.GetAsync("api/dashboard/resumen");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo cargar el resumen del panel.";
                return View(new DashboardViewModel());
            }

            var resumen = await response.Content
                .ReadFromJsonAsync<ResumenDashboardViewModel>();

            var modelo = new DashboardViewModel
            {
                TotalRepuestos = resumen?.total_repuestos ?? 0,
                TotalUsuarios = resumen?.total_usuarios ?? 0,
                StockBajo = resumen?.stock_bajo ?? 0,
                VentasHoy = resumen?.ventas_hoy ?? 0,
                UltimosRepuestos = resumen?.ultimos_repuestos ?? new List<Repuesto>(),
                AlertasStock = resumen?.alertas_stock ?? new List<Repuesto>()
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

        public async Task<IActionResult> Vendedor(string textoBusqueda)
        {
            // AQUÍ ESTÁ EL CAMBIO: Permite el acceso si el rol es 1 (Admin) o 2 (Vendedor)
            if (LoginController.RolActual != 1 && LoginController.RolActual != 2)
                return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var url = string.IsNullOrEmpty(textoBusqueda)
                ? "api/repuestos"
                : $"api/repuestos?textoBusqueda={Uri.EscapeDataString(textoBusqueda)}";

            var repuestos = await client.GetFromJsonAsync<List<Repuesto>>(url);

            return View(repuestos ?? new List<Repuesto>());
        }

        public async Task<IActionResult> Reportes()
        {
            if (LoginController.RolActual != 1)
                return RedirectToAction("Index", "Login");

            var api = _httpClientFactory.CreateClient("FlomarAPI");

            var ventas = await api.GetFromJsonAsync<List<Venta>>("api/ventas") ?? new List<Venta>();
            var usuarios = await api.GetFromJsonAsync<List<Usuario>>("api/usuarios") ?? new List<Usuario>();
            var compras = await api.GetFromJsonAsync<List<CompraReporteView>>("api/compras/reporte") ?? new List<CompraReporteView>();

            ViewBag.Ventas = ventas
                .Where(v => v.id_metodo_pago == 1 && v.Fecha_hora.Date == DateTime.Today)
                .Join(usuarios,
                      v => v.Id_vendedor,
                      u => u.id_usuario,
                      (v, u) => new { u.nombre_completo, v.total_venta })
                .ToList();

            ViewBag.Compras = compras;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}