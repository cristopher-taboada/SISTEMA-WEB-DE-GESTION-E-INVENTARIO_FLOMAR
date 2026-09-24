using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public static int RolActual = 0;
        public static int IdUsuarioActual = 0;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Entrar(string usuario_input, string password_input)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            // La API valida las credenciales contra la base de datos
            var response = await client.PostAsJsonAsync("api/auth/login", new
            {
                usuario = usuario_input,
                password = password_input
            });

            if (response.IsSuccessStatusCode)
            {
                var sesion = await response.Content.ReadFromJsonAsync<LoginResponse>();

                RolActual = sesion!.id_rol;
                IdUsuarioActual = sesion!.id_usuario;

                if (RolActual == 1) return RedirectToAction("Admin", "Home");
                else return RedirectToAction("Vendedor", "Home");
            }

            ViewBag.Error = "Datos incorrectos";
            return View("Index");
        }
    }
}
