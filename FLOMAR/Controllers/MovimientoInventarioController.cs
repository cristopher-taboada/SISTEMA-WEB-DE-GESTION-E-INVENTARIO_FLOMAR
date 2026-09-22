using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FLOMAR.Models;
using FLOMAR.Data;

namespace FLOMAR.Controllers
{
    public class MovimientoInventarioController : Controller
    {
        private readonly FlomarContext _context;

        public MovimientoInventarioController(FlomarContext context)
        {
            _context = context;
        }


        // =========================
        // LISTAR MOVIMIENTOS
        // =========================
        public async Task<IActionResult> Index()
        {
            var movimientos = await (
                from m in _context.Movimientos
                join r in _context.Repuestos on m.id_repuesto equals r.id_repuesto
                join tm in _context.TiposMovimiento on m.id_tipo_movimiento equals tm.id_tipo_movimiento
                join u in _context.Usuarios on m.id_usuario equals u.id_usuario
                orderby m.fecha_movimiento descending
                select new MovimientoListaViewModel
                {
                    id_movimiento = m.id_movimiento,
                    id_repuesto = m.id_repuesto,
                    fecha_movimiento = m.fecha_movimiento,
                    codigo_repuesto = r.Codigo,
                    nombre_repuesto = r.Nombre,
                    tipo_movimiento = tm.nombre_tipo,
                    signo = tm.signo,
                    cantidad = m.cantidad,
                    motivo = m.motivo,
                    observaciones = m.observaciones,
                    usuario = u.nombre_completo,
                    stock_resultante = 0 // se calcula abajo
                }
            ).ToListAsync();

            // STOCK RESULTANTE REAL de cada movimiento:
            // stock despues del movimiento = stock_actual_hoy - suma de los
            // movimientos posteriores del mismo repuesto (la lista ya viene
            // ordenada de mas reciente a mas antiguo)
            var stockHoy = await _context.Repuestos
                .ToDictionaryAsync(r => r.id_repuesto, r => r.stock_actual);

            var acumuladoPosterior = new Dictionary<int, int>();

            foreach (var m in movimientos)
            {
                acumuladoPosterior.TryGetValue(m.id_repuesto, out int posteriores);

                if (stockHoy.TryGetValue(m.id_repuesto, out int actual))
                    m.stock_resultante = actual - posteriores;

                acumuladoPosterior[m.id_repuesto] = posteriores + (m.signo * m.cantidad);
            }

            return View(movimientos);
        }


        // =========================
        // VER DETALLE
        // =========================
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movimiento = await (
                from m in _context.Movimientos
                join r in _context.Repuestos on m.id_repuesto equals r.id_repuesto
                join tm in _context.TiposMovimiento on m.id_tipo_movimiento equals tm.id_tipo_movimiento
                join u in _context.Usuarios on m.id_usuario equals u.id_usuario
                where m.id_movimiento == id
                select new MovimientoDetalleViewModel
                {
                    id_movimiento = m.id_movimiento,
                    fecha_movimiento = m.fecha_movimiento,
                    codigo_repuesto = r.Codigo,
                    nombre_repuesto = r.Nombre,
                    stock_actual = r.stock_actual,
                    stock_minimo = r.stock_minimo,
                    tipo_movimiento = tm.nombre_tipo,
                    signo = tm.signo,
                    cantidad = m.cantidad,
                    motivo = m.motivo,
                    observaciones = m.observaciones,
                    usuario = u.nombre_completo
                }
            ).FirstOrDefaultAsync();

            if (movimiento == null) return NotFound();

            return View(movimiento);
        }


        // =========================
        // FORMULARIO CREAR (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Repuestos = await _context.Repuestos
                .Where(r => r.id_estado == 1)
                .Select(r => new { r.id_repuesto, r.Codigo, r.Nombre })
                .ToListAsync();

            ViewBag.TiposMovimiento = await _context.TiposMovimiento
                .Select(t => new { t.id_tipo_movimiento, t.nombre_tipo })
                .ToListAsync();

            return View();
        }


        // =========================
        // GUARDAR MOVIMIENTO (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovimientoCrearViewModel modelo)
        {
            ViewBag.Repuestos = await _context.Repuestos
                .Where(r => r.id_estado == 1)
                .Select(r => new { r.id_repuesto, r.Codigo, r.Nombre })
                .ToListAsync();

            ViewBag.TiposMovimiento = await _context.TiposMovimiento
                .Select(t => new { t.id_tipo_movimiento, t.nombre_tipo })
                .ToListAsync();

            if (!ModelState.IsValid)
                return View(modelo);

            var tipo = await _context.TiposMovimiento.FindAsync(modelo.id_tipo_movimiento);
            if (tipo == null) return NotFound();

            var repuesto = await _context.Repuestos.FindAsync(modelo.id_repuesto);
            if (repuesto == null) return NotFound();

            // El signo debe ser valido: 1 (ingreso) o -1 (salida/merma)
            if (tipo.signo != 1 && tipo.signo != -1)
            {
                ModelState.AddModelError("",
                    $"El tipo de movimiento '{tipo.nombre_tipo}' tiene un signo invalido en la base de datos.");
                return View(modelo);
            }

            // Una salida NUNCA puede dejar el stock en negativo
            if (tipo.signo == -1 && repuesto.stock_actual < modelo.cantidad)
            {
                ModelState.AddModelError("cantidad",
                    $"Stock insuficiente. Solo hay {repuesto.stock_actual} unidades disponibles de '{repuesto.Nombre}'.");
                return View(modelo);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Insertar movimiento
                var movimiento = new MovimientoInventario
                {
                    fecha_movimiento = DateTime.Now,
                    id_repuesto = modelo.id_repuesto,
                    cantidad = modelo.cantidad,
                    id_tipo_movimiento = modelo.id_tipo_movimiento,
                    id_usuario = 1,
                    motivo = modelo.motivo,
                    observaciones = modelo.observaciones
                };

                _context.Movimientos.Add(movimiento);

                // 2. Actualizar stock segun signo
                if (tipo.signo == 1)
                    repuesto.stock_actual += modelo.cantidad;
                else // signo == -1 (ya validado arriba)
                    repuesto.stock_actual -= modelo.cantidad;

                _context.Update(repuesto);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Exito"] = "Movimiento registrado y stock actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Error al guardar: " + ex.Message;
                return RedirectToAction(nameof(Create));
            }
        }


        // =========================
        // ELIMINAR CON REVERSION
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null) return NotFound();

            var tipo = await _context.TiposMovimiento.FindAsync(movimiento.id_tipo_movimiento);
            var repuesto = await _context.Repuestos.FindAsync(movimiento.id_repuesto);

            if (tipo == null || repuesto == null) return NotFound();

            // Al revertir un INGRESO se resta stock: verificar que no quede negativo
            if (tipo.signo == 1 && repuesto.stock_actual < movimiento.cantidad)
            {
                TempData["Error"] =
                    $"No se puede eliminar: el stock actual de '{repuesto.Nombre}' es {repuesto.stock_actual} " +
                    $"y revertir este ingreso requiere descontar {movimiento.cantidad} unidades.";
                return RedirectToAction(nameof(Index));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Revertir stock (operacion inversa)
                if (tipo.signo == 1)
                    repuesto.stock_actual -= movimiento.cantidad;
                else
                    repuesto.stock_actual += movimiento.cantidad;

                _context.Update(repuesto);
                _context.Movimientos.Remove(movimiento);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Error al eliminar: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
