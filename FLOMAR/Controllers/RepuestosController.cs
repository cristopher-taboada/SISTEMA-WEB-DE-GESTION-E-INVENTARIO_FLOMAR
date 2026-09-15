using Microsoft.AspNetCore.Mvc;
using FLOMAR.Models;
using System.Collections.Generic;
using System.Linq;

namespace FLOMAR.Controllers
{
    public class RepuestosController : Controller
    {
        // LISTA TEMPORAL
        // Después será reemplazada por MySQL
        private static List<Repuesto> repuestos = new List<Repuesto>
        {
            new Repuesto
            {
                Id = 1,
                Codigo = "REP-001",
                Nombre = "Pastilla de freno",
                Activo = true
            },

            new Repuesto
            {
                Id = 2,
                Codigo = "REP-002",
                Nombre = "Filtro de aceite",
                Activo = true
            }
        };


        // LISTAR
        public IActionResult Index()
        {
            return View(repuestos);
        }


        // MOSTRAR CREAR
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // GUARDAR NUEVO REPUESTO
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

                // Todo repuesto nuevo empieza activo
                repuesto.Activo = true;

                repuestos.Add(repuesto);

                return RedirectToAction(nameof(Index));
            }

            return View(repuesto);
        }


        // MOSTRAR EDITAR
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


        // GUARDAR CAMBIOS
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


        // CAMBIAR ESTADO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstado(int id)
        {
            var repuesto = repuestos.FirstOrDefault(r => r.Id == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            // Si está activo → lo desactiva
            // Si está desactivado → lo activa
            repuesto.Activo = !repuesto.Activo;

            return RedirectToAction(nameof(Index));
        }
    }
}