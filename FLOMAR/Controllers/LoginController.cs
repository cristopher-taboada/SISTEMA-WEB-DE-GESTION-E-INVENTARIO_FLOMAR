using Microsoft.AspNetCore.Mvc;
using FLOMAR.Data;
using System.Linq;

namespace FLOMAR.Controllers
{
    public class LoginController : Controller
    {
        private readonly FlomarContext _context;
        public static int RolActual = 0;

        public LoginController(FlomarContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(string usuario_input, string password_input)
        {
           
            var usuario = _context.usuario.FirstOrDefault(u => u.nombre_usuario == usuario_input && u.contrasena_hash == password_input);

            if (usuario != null)
            {
                RolActual = usuario.id_rol;

                if (RolActual == 1) return RedirectToAction("Admin", "Home");
                else return RedirectToAction("Vendedor", "Home");
            }

            ViewBag.Error = "Datos incorrectos";
            return View("Index");
        }
    }
}