using Microsoft.AspNetCore.Mvc;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    public class RepuestosController : Controller
    {
        public IActionResult Index()
        {
            var repuestos = new List<Repuesto>
            {
                new Repuesto
                {
                    Id = 1,
                    Codigo = "REP-001",
                    Nombre = "Pastilla de freno"
                },

                new Repuesto
                {
                    Id = 2,
                    Codigo = "REP-002",
                    Nombre = "Filtro de aceite"
                }
            };

            return View(repuestos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction(nameof(Index));
            }

            return View(repuesto);
        }
    }
}