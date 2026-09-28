using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using FLOMAR.Models;
using FLOMAR.Services;

namespace FLOMAR.Controllers
{
   
    public class MovimientoInventarioController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MovimientoInventarioController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
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
        public async Task<IActionResult> Index(int? meses)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var movimientos = await client
                .GetFromJsonAsync<List<MovimientoListaViewModel>>("api/movimientos");

            // Meses de la alerta: se pueden cambiar desde la vista (?meses=N)
            // y por defecto se toman de appsettings.json -> "MesesSinMovimiento"
            var mesesAlerta = meses.HasValue && meses.Value > 0
                ? meses.Value
                : _configuration.GetValue<int?>("MesesSinMovimiento") ?? 6;

            var sinMovimiento = new List<RepuestoSinMovimientoViewModel>();

            // La API calcula que repuestos no se movieron en ese periodo
            var respuestaAlerta = await client
                .GetAsync($"api/movimientos/sin-movimiento?meses={mesesAlerta}");

            if (respuestaAlerta.IsSuccessStatusCode)
            {
                sinMovimiento = await respuestaAlerta.Content
                    .ReadFromJsonAsync<List<RepuestoSinMovimientoViewModel>>() ?? new();
            }

            var modelo = new MovimientoIndexViewModel
            {
                Movimientos = movimientos ?? new(),
                SinMovimiento = sinMovimiento,
                MesesSinMovimiento = mesesAlerta
            };

            return View(modelo);
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
        public async Task<IActionResult> Create(int? id_repuesto)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            await CargarCombos(client);

            // Si se llega desde la alerta de inmovilizados (?id_repuesto=N),
            // el repuesto ya queda seleccionado en el formulario
            return View(new MovimientoCrearViewModel
            {
                id_repuesto = id_repuesto ?? 0,
                id_tipo_movimiento = 0
            });
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
