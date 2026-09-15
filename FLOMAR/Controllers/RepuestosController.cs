using Microsoft.AspNetCore.Mvc;
using FLOMAR.Models;
using System.Collections.Generic;
using System.Linq;

namespace FLOMAR.Controllers
{
    public class RepuestosController : Controller
    {
        // LISTA TEMPORAL
        // Después será reemplazada por la base de datos MySQL.
        private static List<Repuesto> repuestos = new List<Repuesto>
        {
            new Repuesto
            {
                Id = 1,
                Codigo = "FRE-001",
                Nombre = "Pastilla de freno",
                Categoria = "Frenos",
                Costo = 120,
                PrecioVenta = 180,
                Stock = 4,
                StockMinimo = 5,
                Activo = true
            },

            new Repuesto
            {
                Id = 2,
                Codigo = "MOT-001",
                Nombre = "Filtro de aceite",
                Categoria = "Motor",
                Costo = 45,
                PrecioVenta = 70,
                Stock = 15,
                StockMinimo = 5,
                Activo = true
            },

            new Repuesto
            {
                Id = 3,
                Codigo = "ENC-001",
                Nombre = "Bujía",
                Categoria = "Encendido",
                Costo = 25,
                PrecioVenta = 40,
                Stock = 3,
                StockMinimo = 4,
                Activo = true
            }
        };


        // =========================
        // LISTAR REPUESTOS
        // =========================
        public IActionResult Index()
        {
            return View(repuestos);
        }


        // =========================
        // MOSTRAR FORMULARIO CREAR
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // =========================
        // GUARDAR NUEVO REPUESTO
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Repuesto repuesto)
        {
            // Verificamos las validaciones de Repuesto.cs
            if (ModelState.IsValid)
            {
                // Generar ID automáticamente
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

                // Agregar a la lista
                repuestos.Add(repuesto);

                // Volver al listado
                return RedirectToAction(nameof(Index));
            }

            return View(repuesto);
        }


        // =========================
        // MOSTRAR FORMULARIO EDITAR
        // =========================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Buscar el repuesto por su ID
            var repuesto =
                repuestos.FirstOrDefault(r => r.Id == id);

            // Si no existe
            if (repuesto == null)
            {
                return NotFound();
            }

            // Mandarlo a Edit.cshtml
            return View(repuesto);
        }


        // =========================
        // GUARDAR CAMBIOS
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                // Buscar el repuesto original
                var repuestoExistente =
                    repuestos.FirstOrDefault(r => r.Id == repuesto.Id);

                if (repuestoExistente == null)
                {
                    return NotFound();
                }

                // Actualizar todos sus datos
                repuestoExistente.Codigo = repuesto.Codigo;
                repuestoExistente.Nombre = repuesto.Nombre;
                repuestoExistente.Categoria = repuesto.Categoria;
                repuestoExistente.Costo = repuesto.Costo;
                repuestoExistente.PrecioVenta = repuesto.PrecioVenta;
                repuestoExistente.Stock = repuesto.Stock;
                repuestoExistente.StockMinimo = repuesto.StockMinimo;

                // No modificamos Activo aquí.
                // Se mantiene como estaba.

                return RedirectToAction(nameof(Index));
            }

            return View(repuesto);
        }


        // =========================
        // ACTIVAR / DESACTIVAR
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstado(int id)
        {
            // Buscar el repuesto
            var repuesto =
                repuestos.FirstOrDefault(r => r.Id == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            // Cambiar al estado contrario
            repuesto.Activo = !repuesto.Activo;

            return RedirectToAction(nameof(Index));
        }
    }
}