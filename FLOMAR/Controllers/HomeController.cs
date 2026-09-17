using Microsoft.AspNetCore.Mvc;

namespace FLOMAR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin()
        {
            if (LoginController.RolActual != 1) return RedirectToAction("Index", "Login");
            return View();
        }

        public IActionResult Vendedor()
        {
            if (LoginController.RolActual != 2) return RedirectToAction("Index", "Login");
            return View();
        }
    }
}