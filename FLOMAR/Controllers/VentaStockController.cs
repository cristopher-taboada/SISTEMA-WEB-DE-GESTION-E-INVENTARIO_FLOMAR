using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FLOMAR.Data;
using FLOMAR.Models;

namespace FLOMAR.Controllers
{
    public class VentaStockController : Controller
    {
        private readonly FlomarContext _context;

        public VentaStockController(FlomarContext context)
        {
            _context = context;
        }

        // MOSTRAR REPUESTOS PARA VENDER

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var repuestos = await _context.Repuestos
                .Where(r => r.id_estado == 1)
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            return View(repuestos);
        }

        // MOSTRAR FORMULARIO DE VENTA
        [HttpGet]
        public async Task<IActionResult> Vender(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.id_repuesto == id);

            if (repuesto == null)
            {
                return NotFound();
            }

            if (repuesto.id_estado != 1)
            {
                TempData["Error"] = "El repuesto está inactivo.";
                return RedirectToAction(nameof(Index));
            }

            var modelo = new VentaStockViewModel
            {
                IdRepuesto = repuesto.id_repuesto,
                Codigo = repuesto.Codigo,
                Nombre = repuesto.Nombre,
                PrecioVenta = repuesto.PrecioVenta,
                StockActual = repuesto.stock_actual,
                Cantidad = 1
            };

            return View(modelo);
        }

        // CONFIRMAR Y DESCONTAR STOCK
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vender(VentaStockViewModel modelo)
        {
            if (modelo.Cantidad <= 0)
            {
                ModelState.AddModelError(
                    nameof(modelo.Cantidad),
                    "La cantidad debe ser mayor a 0."
                );
            }


            // Volvemos a consultar el repuesto desde MySQL.
            // No confiamos en el stock enviado por el formulario.
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(
                    r => r.id_repuesto == modelo.IdRepuesto
                );


            if (repuesto == null)
            {
                return NotFound();
            }


            // Volver a cargar los datos para mostrarlos si existe error
            modelo.Codigo = repuesto.Codigo;
            modelo.Nombre = repuesto.Nombre;
            modelo.PrecioVenta = repuesto.PrecioVenta;
            modelo.StockActual = repuesto.stock_actual;

            // VALIDAR QUE ESTÉ ACTIVO

            if (repuesto.id_estado != 1)
            {
                ModelState.AddModelError(
                    "",
                    "El repuesto se encuentra inactivo."
                );
            }


            // VALIDAR STOCK
            if (modelo.Cantidad > repuesto.stock_actual)
            {
                ModelState.AddModelError(
                    nameof(modelo.Cantidad),
                    $"Stock insuficiente. Disponible: {repuesto.stock_actual} unidades."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(modelo);
            }


            // DESCONTAR STOCK

            repuesto.stock_actual =
                repuesto.stock_actual - modelo.Cantidad;

            //GUARDAR EN MYSQL
           
            await _context.SaveChangesAsync();


            TempData["Exito"] =
                $"Venta realizada. Se descontaron {modelo.Cantidad} unidades de {repuesto.Nombre}.";


            // Si después del descuento llegó al mínimo
            if (repuesto.stock_actual <= repuesto.stock_minimo)
            {
                TempData["StockBajo"] =
                    $"Advertencia: {repuesto.Nombre} tiene stock bajo. Stock actual: {repuesto.stock_actual}.";
            }


            return RedirectToAction(nameof(Index));
        }
    }
}