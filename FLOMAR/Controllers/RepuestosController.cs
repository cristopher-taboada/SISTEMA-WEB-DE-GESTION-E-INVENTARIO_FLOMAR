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


        // LISTAR
        public async Task<IActionResult> Index()
        {
            var repuestos = await _context.Repuestos
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            return View(repuestos);
        }


        // MOSTRAR CREAR
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // CREAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Repuesto repuesto)
        {
            bool codigoExiste = await _context.Repuestos
                .AnyAsync(r => r.Codigo == repuesto.Codigo);

            if (codigoExiste)
            {
                ModelState.AddModelError(
                    nameof(repuesto.Codigo),
                    "Ya existe un repuesto con este código."
                );
            }


            // Validación del precio
            if (repuesto.PrecioVenta < repuesto.costo_adquisicion)
            {
                ModelState.AddModelError(
                    nameof(repuesto.PrecioVenta),
                    "El precio de venta no puede ser menor al costo."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }


            repuesto.id_estado = 1;

            _context.Repuestos.Add(repuesto);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // MOSTRAR EDITAR
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(
                    r => r.id_repuesto == id
                );

            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }


        // GUARDAR EDICIÓN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Repuesto repuesto)
        {
            // Validar código repetido
            bool codigoExiste = await _context.Repuestos
                .AnyAsync(r =>
                    r.Codigo == repuesto.Codigo &&
                    r.id_repuesto != repuesto.id_repuesto
                );

            if (codigoExiste)
            {
                ModelState.AddModelError(
                    nameof(repuesto.Codigo),
                    "Ya existe otro repuesto con este código."
                );
            }


            // Validar precio
            if (repuesto.PrecioVenta < repuesto.costo_adquisicion)
            {
                ModelState.AddModelError(
                    nameof(repuesto.PrecioVenta),
                    "El precio de venta no puede ser menor al costo."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }


            // Buscar el repuesto original
            var actual = await _context.Repuestos
                .FirstOrDefaultAsync(
                    r => r.id_repuesto == repuesto.id_repuesto
                );


            if (actual == null)
            {
                return NotFound();
            }


            // Actualizar datos
            actual.Codigo = repuesto.Codigo;
            actual.Nombre = repuesto.Nombre;
            actual.id_categoria = repuesto.id_categoria;
            actual.costo_adquisicion = repuesto.costo_adquisicion;
            actual.PrecioVenta = repuesto.PrecioVenta;
            actual.stock_actual = repuesto.stock_actual;
            actual.stock_minimo = repuesto.stock_minimo;


            // Guardar en MySQL
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }


        // ACTIVAR / DESACTIVAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(
                    r => r.id_repuesto == id
                );


            if (repuesto == null)
            {
                return NotFound();
            }


            repuesto.id_estado =
                repuesto.id_estado == 1 ? 2 : 1;


            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}