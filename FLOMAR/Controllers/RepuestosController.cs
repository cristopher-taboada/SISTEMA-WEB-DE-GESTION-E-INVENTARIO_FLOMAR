using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FLOMAR.Data;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    public class RepuestosController : Controller
    {
        private readonly FlomarContext _context;

        public RepuestosController(FlomarContext context)
        {
            _context = context;
        }

        // LISTAR SOLO REPUESTOS ACTIVOS
        public async Task<IActionResult> Index()
        {
            var repuestos = await _context.Repuestos
                .Where(r => r.id_estado_repuesto == 1)
                .ToListAsync();

            return View(repuestos);
        }

        // MOSTRAR FORMULARIO CREAR
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // GUARDAR REPUESTO EN MYSQL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Repuesto repuesto)
        {
            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }

            // Estado 1 = Activo
            repuesto.id_estado_repuesto = 1;

            _context.Repuestos.Add(repuesto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // MOSTRAR REPUESTO PARA EDITAR
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.Id == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }

        // GUARDAR CAMBIOS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Repuesto repuesto)
        {
            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }

            var repuestoExistente = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.Id == repuesto.Id);

            if (repuestoExistente == null)
            {
                return NotFound();
            }

            repuestoExistente.Codigo = repuesto.Codigo;
            repuestoExistente.Nombre = repuesto.Nombre;
            repuestoExistente.Id_categoria = repuesto.Id_categoria;
            repuestoExistente.costo_adquisicion = repuesto.costo_adquisicion;
            repuestoExistente.precio_venta = repuesto.precio_venta;
            repuestoExistente.stock_actual = repuesto.stock_actual;
            repuestoExistente.stock_minimo = repuesto.stock_minimo;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DESACTIVAR SIN ELIMINAR DE MYSQL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.Id == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            // No lo eliminamos.
            // Solamente cambiamos su estado.
            repuesto.id_estado_repuesto = 2;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}