using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using FLOMAR.Models;

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

        public IActionResult Vendedor(string textoBusqueda)
        {
            if (LoginController.RolActual != 2) return RedirectToAction("Index", "Login");

            var repuestosTemp = new List<Repuesto>
            {
                new Repuesto { Nombre = "Aro Enkei", Codigo = "AR-001", PrecioVenta = 850 },
                new Repuesto { Nombre = "Disco de Freno", Codigo = "FR-002", PrecioVenta = 320 },
                new Repuesto { Nombre = "Filtro de Aceite", Codigo = "FI-003", PrecioVenta = 75 },
                new Repuesto { Nombre = "Pastillas de Freno", Codigo = "FR-004", PrecioVenta = 180 },
                new Repuesto { Nombre = "Aro Deportivo", Codigo = "AR-006", PrecioVenta = 720 }
            };

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                repuestosTemp = repuestosTemp.Where(r =>
                    r.Nombre.ToLower().Contains(textoBusqueda.ToLower()) ||
                    r.Codigo.ToLower().Contains(textoBusqueda.ToLower())).ToList();
            }

            return View(repuestosTemp);
        }
    }
}