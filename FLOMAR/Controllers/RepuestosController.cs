using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using FLOMAR.Models;
using FLOMAR.Services;

namespace FLOMAR.Controllers
{
    public class RepuestosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RepuestosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        // LISTAR
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var repuestos = await client
                .GetFromJsonAsync<List<Repuesto>>("api/repuestos");

            return View(repuestos);
        }


        // MOSTRAR CREAR
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // CREAR (la validacion de negocio la hace la API)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Repuesto repuesto)
        {
            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.PostAsJsonAsync("api/repuestos", repuesto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            // La API devuelve el mensaje y el campo que fallo
            var (mensaje, campo) = await ApiHelper.LeerRespuestaAsync(response);
            ModelState.AddModelError(campo, mensaje);

            return View(repuesto);
        }


        // MOSTRAR EDITAR
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.GetAsync($"api/repuestos/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            var repuesto = await response.Content.ReadFromJsonAsync<Repuesto>();

            return View(repuesto);
        }


        // GUARDAR EDICION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Repuesto repuesto)
        {
            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.PutAsJsonAsync(
                $"api/repuestos/{repuesto.id_repuesto}", repuesto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            var (mensaje, campo) = await ApiHelper.LeerRespuestaAsync(response);
            ModelState.AddModelError(campo, mensaje);

            return View(repuesto);
        }


        // ACTIVAR / DESACTIVAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.PostAsync(
                $"api/repuestos/{id}/cambiar-estado", null);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
