using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Usuarios
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var usuarios = await client
                .GetFromJsonAsync<List<Usuario>>("api/usuarios");

            return View(usuarios ?? new List<Usuario>());
        }

        // POST: /Usuarios/CambiarEstado/5
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            try
            {
                // Invocamos directamente el endpoint dedicado de la API
                await client.PutAsync($"api/usuarios/cambiar-estado/{id}", null);
            }
            catch (Exception)
            {
                // Manejo de errores de conexión
            }

            return RedirectToAction("Index");
        }
    }
}