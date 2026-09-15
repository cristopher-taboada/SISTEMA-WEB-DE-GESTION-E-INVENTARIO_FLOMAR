using Microsoft.AspNetCore.Mvc;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    public class RepuestosController : Controller
    {
        private static List<Repuesto> repuestos = new List<Repuesto>
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


        public IActionResult Index()
        {
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

                if (repuestos.Count > 0)
                {
                    repuesto.Id = repuestos.Max(r => r.Id) + 1;
                }
                else
                {
                    repuesto.Id = 1;
                }

                repuestos.Add(repuesto);

                return RedirectToAction(nameof(Index));
            }

            return View(repuesto);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var repuesto = repuestos.FirstOrDefault(r => r.Id == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                var repuestoExistente =
                    repuestos.FirstOrDefault(r => r.Id == repuesto.Id);

                if (repuestoExistente == null)
                {
                    return NotFound();
                }

                repuestoExistente.Codigo = repuesto.Codigo;
                repuestoExistente.Nombre = repuesto.Nombre;

                return RedirectToAction(nameof(Index));
            }

            return View(repuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Desactivar(int id)
        {
            var repuesto = repuestos.FirstOrDefault(r => r.Id == id);

            if (repuesto != null)
            {
                repuestos.Remove(repuesto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}