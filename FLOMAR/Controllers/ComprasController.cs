using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FLOMAR.Models;
using FLOMAR.Data;

namespace FLOMAR.Controllers
{
    public class ComprasController : Controller
    {
        private readonly FlomarContext _context;

        public ComprasController(FlomarContext context)
        {
            _context = context;
        }


        // =========================
        // LISTAR COMPRAS
        // =========================
        public async Task<IActionResult> Index()
        {
            var compras = await _context.Compras
                .Include(c => c.ProveedorRel)
                .Include(c => c.UsuarioRel)
                .GroupJoin(
                    _context.Detalle_compras,
                    c => c.Id_compra,
                    dc => dc.id_compra,
                    (c, dcs) => new { Compra = c, Detalles = dcs }
                )
                .Select(x => new AdministrarInventario
                {
                    Id_compra = x.Compra.Id_compra,
                    numero_compra = x.Compra.numero_compra,
                    fecha_ingreso = x.Compra.fecha_ingreso,
                    proveedor = x.Compra.ProveedorRel!.nombre,
                    productos = x.Detalles.Count(),
                    monto_total = x.Compra.monto_total,
                    registrado_por = x.Compra.UsuarioRel!.nombre_completo
                })
                .OrderByDescending(x => x.fecha_ingreso)
                .ToListAsync();

            return View(compras);
        }


        // =========================
        // VER DETALLE
        // =========================
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var compra = await _context.Compras
                .Include(c => c.ProveedorRel)
                .Include(c => c.UsuarioRel)
                .FirstOrDefaultAsync(m => m.Id_compra == id);

            if (compra == null) return NotFound();

            var detalles = await (
                from dc in _context.Detalle_compras
                join r in _context.Repuestos on dc.id_repuesto equals r.id_repuesto into gj
                from r in gj.DefaultIfEmpty()
                where dc.id_compra == id
                select new DetalleCompraItem
                {
                    codigo = r != null ? r.Codigo : "",
                    repuesto = r != null ? r.Nombre : "(repuesto eliminado)",
                    cantidad = dc.cantidad,
                    costo_unitario = dc.costo_unitario,
                    subtotal = dc.subtotal
                }
            ).ToListAsync();

            var modelo = new CompraDetalleViewModel
            {
                Id_compra = compra.Id_compra,
                numero_compra = compra.numero_compra,
                fecha_ingreso = compra.fecha_ingreso,
                proveedor = compra.ProveedorRel?.nombre ?? "",
                registrado_por = compra.UsuarioRel?.nombre_completo ?? "",
                monto_total = compra.monto_total,
                observaciones = compra.observaciones,
                Detalles = detalles
            };

            return View(modelo);
        }


        // =========================
        // FORMULARIO CREAR (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarProveedores();

            // Correlativo sugerido segun las compras del anio actual
            var anio = DateTime.Now.Year;
            var correlativo = await _context.Compras
                .CountAsync(c => c.fecha_ingreso.Year == anio) + 1;

            var modelo = new CompraCreateFormViewModel
            {
                numero_compra = $"CMP-{anio}-{correlativo:D3}",
                fecha_ingreso = DateTime.Now,
                Productos = await ObtenerProductosConCantidades(null)
            };

            return View(modelo);
        }


        // =========================
        // GUARDAR COMPRA (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraCreateFormViewModel modelo)
        {
            var productosConCantidad = modelo.Productos
                .Where(p => p.cantidad > 0)
                .ToList();

            // 1. Validacion del modelo (nro compra, fecha, proveedor, etc.)
            if (!ModelState.IsValid)
            {
                await CargarProveedores();
                modelo.Productos = await ObtenerProductosConCantidades(modelo.Productos);
                return View(modelo);
            }

            // 2. Debe haber al menos un producto con cantidad
            if (productosConCantidad.Count == 0)
            {
                await CargarProveedores();
                modelo.Productos = await ObtenerProductosConCantidades(modelo.Productos);
                ModelState.AddModelError("", "Debe ingresar cantidad en al menos un producto.");
                return View(modelo);
            }

            // 3. Numero de compra duplicado
            bool numeroExiste = await _context.Compras
                .AnyAsync(c => c.numero_compra == modelo.numero_compra);

            if (numeroExiste)
            {
                await CargarProveedores();
                modelo.Productos = await ObtenerProductosConCantidades(modelo.Productos);
                ModelState.AddModelError("numero_compra", "Ya existe una compra registrada con ese numero.");
                return View(modelo);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 4. Verificar repuestos y calcular el monto con los costos REALES de la BD
                //    (no se confia en los costos ocultos que vienen del formulario)
                var itemsValidados = new List<(ProductoFilaViewModel fila, Repuesto repuesto)>();
                decimal montoTotal = 0;

                foreach (var item in productosConCantidad)
                {
                    var repuesto = await _context.Repuestos.FindAsync(item.id_repuesto);

                    if (repuesto == null)
                    {
                        await transaction.RollbackAsync();
                        await CargarProveedores();
                        modelo.Productos = await ObtenerProductosConCantidades(modelo.Productos);
                        ModelState.AddModelError("", $"El repuesto con ID {item.id_repuesto} ya no existe.");
                        return View(modelo);
                    }

                    itemsValidados.Add((item, repuesto));
                    montoTotal += item.cantidad * repuesto.costo_adquisicion;
                }

                // 5. Crear la compra
                var compra = new Compra
                {
                    numero_compra = modelo.numero_compra,
                    fecha_ingreso = modelo.fecha_ingreso,
                    id_proveedor = modelo.id_proveedor,
                    id_usuario = 1,
                    monto_total = montoTotal,
                    observaciones = modelo.observaciones ?? ""
                };

                _context.Compras.Add(compra);
                await _context.SaveChangesAsync();

                // 6. Crear detalles y aumentar stock
                foreach (var (fila, repuesto) in itemsValidados)
                {
                    _context.Detalle_compras.Add(new Detalle_compra
                    {
                        id_compra = compra.Id_compra,
                        id_repuesto = fila.id_repuesto,
                        cantidad = fila.cantidad,
                        costo_unitario = repuesto.costo_adquisicion,
                        subtotal = fila.cantidad * repuesto.costo_adquisicion
                    });

                    repuesto.stock_actual += fila.cantidad;
                    _context.Update(repuesto);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Exito"] = $"Compra {compra.numero_compra} registrada correctamente.";
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
        // HELPERS PRIVADOS
        // =========================
        private async Task CargarProveedores()
        {
            ViewBag.Proveedores = await _context.Proveedores
                .Select(p => new { p.Id_proveedor, p.nombre })
                .ToListAsync();
        }

        // Recarga la lista de repuestos desde la BD (codigo, nombre y costo reales)
        // conservando las cantidades que el usuario escribio en el formulario
        private async Task<List<ProductoFilaViewModel>> ObtenerProductosConCantidades(
            List<ProductoFilaViewModel>? posteados)
        {
            var repuestos = await _context.Repuestos
                .Where(r => r.id_estado == 1)
                .Select(r => new ProductoFilaViewModel
                {
                    id_repuesto = r.id_repuesto,
                    codigo = r.Codigo,
                    nombre = r.Nombre,
                    costo_unitario = r.costo_adquisicion,
                    cantidad = 0
                })
                .ToListAsync();

            if (posteados != null)
            {
                foreach (var fila in repuestos)
                {
                    var posteada = posteados
                        .FirstOrDefault(p => p.id_repuesto == fila.id_repuesto);

                    if (posteada != null)
                        fila.cantidad = posteada.cantidad;
                }
            }

            return repuestos;
        }


        // =========================
        // EDITAR (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var compra = await _context.Compras
                .Include(c => c.ProveedorRel)
                .Include(c => c.UsuarioRel)
                .FirstOrDefaultAsync(m => m.Id_compra == id);

            if (compra == null) return NotFound();

            var modelo = new CompraEditViewModel
            {
                Id_compra = compra.Id_compra,
                numero_compra = compra.numero_compra,
                fecha_ingreso = compra.fecha_ingreso,
                id_proveedor = compra.id_proveedor,
                monto_total = compra.monto_total,
                observaciones = compra.observaciones,
                registrado_por = compra.UsuarioRel?.nombre_completo ?? "",
                productos = await _context.Detalle_compras
                    .CountAsync(dc => dc.id_compra == id)
            };

            ViewBag.Proveedores = await _context.Proveedores
                .Select(p => new { p.Id_proveedor, p.nombre })
                .ToListAsync();

            return View(modelo);
        }


        // =========================
        // EDITAR (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompraEditViewModel modelo)
        {
            if (id != modelo.Id_compra) return NotFound();

            if (!ModelState.IsValid)
            {
                // Recargar datos de solo lectura y la lista de proveedores
                var compraActual = await _context.Compras
                    .Include(c => c.UsuarioRel)
                    .FirstOrDefaultAsync(c => c.Id_compra == id);

                if (compraActual == null) return NotFound();

                modelo.registrado_por = compraActual.UsuarioRel?.nombre_completo ?? "";
                modelo.productos = await _context.Detalle_compras
                    .CountAsync(dc => dc.id_compra == id);

                ViewBag.Proveedores = await _context.Proveedores
                    .Select(p => new { p.Id_proveedor, p.nombre })
                    .ToListAsync();

                return View(modelo);
            }

            var existente = await _context.Compras.FindAsync(id);
            if (existente == null) return NotFound();

            existente.numero_compra = modelo.numero_compra;
            existente.fecha_ingreso = modelo.fecha_ingreso;
            existente.id_proveedor = modelo.id_proveedor;
            existente.monto_total = modelo.monto_total;
            existente.observaciones = modelo.observaciones ?? "";

            _context.Update(existente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // ELIMINAR (con reversion de stock)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var detalles = await _context.Detalle_compras
                    .Where(dc => dc.id_compra == id)
                    .ToListAsync();

                // 1. Revertir el stock que ingreso con esta compra
                foreach (var detalle in detalles)
                {
                    var repuesto = await _context.Repuestos.FindAsync(detalle.id_repuesto);
                    if (repuesto != null)
                    {
                        repuesto.stock_actual -= detalle.cantidad;

                        // El stock nunca puede quedar negativo
                        if (repuesto.stock_actual < 0)
                            repuesto.stock_actual = 0;

                        _context.Update(repuesto);
                    }
                }

                // 2. Eliminar primero los detalles (FK) y luego la compra
                _context.Detalle_compras.RemoveRange(detalles);
                _context.Compras.Remove(compra);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Exito"] = $"Compra {compra.numero_compra} eliminada y stock revertido correctamente.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Error al eliminar la compra: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
