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
                id_repuesto = 1,
                Codigo = "FRE-001",
                Nombre = "Pastilla de freno",
                id_categoria = 1,
                costo_adquisicion = 120,
                PrecioVenta = 180,
                stock_actual = 4,
                stock_minimo = 5,
                id_estado = 1
            },

            new Repuesto
            {
                id_repuesto = 2,
                Codigo = "MOT-001",
                Nombre = "Filtro de aceite",
                id_categoria = 2,
                costo_adquisicion = 45,
                PrecioVenta = 70,
                stock_actual = 15,
                stock_minimo = 5,
                id_estado = 1
            },

            new Repuesto
            {
                id_repuesto = 3,
                Codigo = "ENC-001",
                Nombre = "Bujía",
                id_categoria = 3,
                costo_adquisicion = 25,
                PrecioVenta = 40,
                stock_actual = 3,
                stock_minimo = 4,
                id_estado = 1
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
                    repuesto.id_repuesto = repuestos.Max(r => r.id_repuesto) + 1;
                }
                else
                {
                    repuesto.id_repuesto = 1;
                }

                // Todo repuesto nuevo empieza activo
                repuesto.id_estado = 1;

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
                repuestoExistente.id_categoria = repuesto.id_categoria;
                repuestoExistente.costo_adquisicion = repuesto.costo_adquisicion;
                repuestoExistente.PrecioVenta = repuesto.PrecioVenta;
                repuestoExistente.stock_actual = repuesto.stock_actual;
                repuestoExistente.stock_minimo = repuesto.stock_minimo;
                repuestoExistente.id_estado = repuesto.id_estado;

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
                repuestos.FirstOrDefault(r => r.id_repuesto == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            // Cambiar al estado contrario
            repuesto.id_estado = repuesto.id_estado == 1 ? 0 : 1;

            return RedirectToAction(nameof(Index));
        }
    }
}