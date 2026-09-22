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


        // =========================================
        // LISTAR REPUESTOS DESDE MYSQL
        // =========================================

        public async Task<IActionResult> Index()
        {
            var repuestos = await _context.Repuestos
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            return View(repuestos);
        }


        // =========================================
        // MOSTRAR FORMULARIO CREAR
        // =========================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // =========================================
        // GUARDAR REPUESTO EN MYSQL
        // =========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Repuesto repuesto)
        {
            // Comprobar código repetido
            bool codigoExiste = await _context.Repuestos
                .AnyAsync(r => r.Codigo == repuesto.Codigo);

            if (codigoExiste)
            {
                ModelState.AddModelError(
                    nameof(repuesto.Codigo),
                    "Ya existe un repuesto con este código."
                );
            }


            // El precio de venta no puede ser menor al costo
            if (repuesto.PrecioVenta < repuesto.costo_adquisicion)
            {
                ModelState.AddModelError(
                    nameof(repuesto.PrecioVenta),
                    "El precio de venta no puede ser menor al costo de adquisición."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }


            // 1 = Activo
            repuesto.id_estado = 1;


            // INSERT
            _context.Repuestos.Add(repuesto);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }


        // =========================================
        // MOSTRAR FORMULARIO EDITAR
        // =========================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.id_repuesto == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }


        // =========================================
        // GUARDAR EDICIÓN EN MYSQL
        // =========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Repuesto repuesto)
        {
            bool codigoExiste = await _context.Repuestos
                .AnyAsync(r =>
                    r.Codigo == repuesto.Codigo &&
                    r.id_repuesto != repuesto.id_repuesto);

            if (codigoExiste)
            {
                ModelState.AddModelError(
                    nameof(repuesto.Codigo),
                    "Ya existe otro repuesto con este código."
                );
            }


            if (repuesto.PrecioVenta < repuesto.costo_adquisicion)
            {
                ModelState.AddModelError(
                    nameof(repuesto.PrecioVenta),
                    "El precio de venta no puede ser menor al costo de adquisición."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }


            var repuestoExistente = await _context.Repuestos
                .FirstOrDefaultAsync(
                    r => r.id_repuesto == repuesto.id_repuesto);

            if (repuestoExistente == null)
            {
                return NotFound();
            }


            repuestoExistente.Codigo =
                repuesto.Codigo;

            repuestoExistente.Nombre =
                repuesto.Nombre;

            repuestoExistente.id_categoria =
                repuesto.id_categoria;

            repuestoExistente.costo_adquisicion =
                repuesto.costo_adquisicion;

            repuestoExistente.PrecioVenta =
                repuesto.PrecioVenta;

            repuestoExistente.stock_actual =
                repuesto.stock_actual;

            repuestoExistente.stock_minimo =
                repuesto.stock_minimo;


            // UPDATE
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }


        // =========================================
        // ACTIVAR / DESACTIVAR
        // =========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(
                    r => r.id_repuesto == id);

            if (repuesto == null)
            {
                return NotFound();
            }


            // ESTADO_GENERAL
            // 1 = Activo
            // 2 = Inactivo

            if (repuesto.id_estado == 1)
            {
                repuesto.id_estado = 2;
            }
            else
            {
                repuesto.id_estado = 1;
            }


            // UPDATE EN MYSQL
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}