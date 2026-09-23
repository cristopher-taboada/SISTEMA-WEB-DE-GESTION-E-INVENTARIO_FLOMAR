using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using FLOMAR.Models;
using FLOMAR.Services;

namespace FLOMAR.Controllers
{
    // La logica de stock, validaciones y el calculo del stock resultante
    // viven en la FlomarAPI. Este controlador solo orquesta llamadas HTTP.
    public class MovimientoInventarioController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MovimientoInventarioController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Carga los combos del formulario (repuestos y tipos de movimiento)
        private async Task CargarCombos(HttpClient client)
        {
            ViewBag.Repuestos = await client
                .GetFromJsonAsync<List<Repuesto>>("api/repuestos");

            ViewBag.TiposMovimiento = await client
                .GetFromJsonAsync<List<TipoMovimiento>>("api/tiposmovimiento");
        }

        // =========================
        // LISTAR MOVIMIENTOS (kardex)
        // =========================
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var movimientos = await client
                .GetFromJsonAsync<List<MovimientoListaViewModel>>("api/movimientos");

            return View(movimientos);
        }

        // =========================
        // VER DETALLE
        // =========================
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.GetAsync($"api/movimientos/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            var modelo = await response.Content
                .ReadFromJsonAsync<MovimientoDetalleViewModel>();

            return View(modelo);
        }

        // =========================
        // FORMULARIO CREAR (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            await CargarCombos(client);

            return View(new MovimientoCrearViewModel());
        }

        // =========================
        // GUARDAR MOVIMIENTO (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovimientoCrearViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            // La API valida: repuesto/tipo existen, signo valido y
            // que una salida no deje el stock en negativo
            var response = await client.PostAsJsonAsync("api/movimientos", modelo);

            var (mensaje, _) = await ApiHelper.LeerRespuestaAsync(response);

            if (response.IsSuccessStatusCode)
            {
                TempData["Exito"] = mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = mensaje;
            return RedirectToAction(nameof(Create));
        }

        // =========================
        // ELIMINAR (la API revierte el stock)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.DeleteAsync($"api/movimientos/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            var (mensaje, _) = await ApiHelper.LeerRespuestaAsync(response);

            if (response.IsSuccessStatusCode)
                TempData["Exito"] = mensaje;
            else
                TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Index));
        }
    }
}
