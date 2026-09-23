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
    }
}
