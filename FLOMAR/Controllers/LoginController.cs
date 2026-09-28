using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public static int RolActual = 0;

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

            // La API valida las credenciales y el estado contra la base de datos
            var response = await client.PostAsJsonAsync("api/auth/login", new
            {
                usuario = usuario_input,
                password = password_input
            });

            if (response.IsSuccessStatusCode)
            {
                var sesion = await response.Content.ReadFromJsonAsync<LoginResponse>();

                RolActual = sesion!.id_rol;

                if (RolActual == 1) return RedirectToAction("Admin", "Home");
                else return RedirectToAction("Vendedor", "Home");
            }

            // Capturamos el mensaje exacto que envía la API (cuenta inactiva o datos incorrectos)
            try
            {
                var errorObj = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (errorObj != null && errorObj.ContainsKey("mensaje"))
                {
                    ViewBag.Error = errorObj["mensaje"];
                }
                else
                {
                    ViewBag.Error = "Datos incorrectos";
                }
            }
            catch
            {
                ViewBag.Error = "Datos incorrectos";
            }

            return View("Index");
        }
    }
}