using FLOMAR.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Diagnostics;

namespace FLOMAR.Controllers
{
    public class HomeController : Controller
    {
        // El MVC ya no usa FlomarContext: los datos se piden a la FlomarAPI por HTTP
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Admin()
        {
            if (LoginController.RolActual != 1) return RedirectToAction("Index", "Login");

            return View();
        }

        public async Task<IActionResult> Vendedor(string textoBusqueda)
        {
            if (LoginController.RolActual != 2) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            // La busqueda por nombre o codigo la hace la API
            var url = string.IsNullOrEmpty(textoBusqueda)
                ? "api/repuestos"
                : $"api/repuestos?textoBusqueda={Uri.EscapeDataString(textoBusqueda)}";

            var listaRepuestos = await client.GetFromJsonAsync<List<Repuesto>>(url);

            return View(listaRepuestos);
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

    public class CompraReporteView
    {
        public DateTime fecha { get; set; }
        public string nro { get; set; } = "";
        public string proveedor { get; set; } = "";
        public string repuesto { get; set; } = "";
        public int cantidad { get; set; }
        public decimal precio { get; set; }
    }
}