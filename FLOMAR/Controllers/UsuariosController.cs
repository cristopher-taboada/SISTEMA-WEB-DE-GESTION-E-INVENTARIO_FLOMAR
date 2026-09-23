using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    // El acceso a datos vive en la FlomarAPI.
    // Este controlador solo consume el endpoint api/usuarios.
    public class UsuariosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Usuarios  (listado de usuarios del administrador)
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var usuarios = await client
                .GetFromJsonAsync<List<Usuario>>("api/usuarios");

            return View(usuarios);
        }
    }
}
