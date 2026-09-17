using FLOMAR.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FLOMAR.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult Vendedor()
        {
            
            if (LoginController.RolActual != 2) return RedirectToAction("Index", "Login");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}