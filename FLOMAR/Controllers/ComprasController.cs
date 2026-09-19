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

            var detalles = await _context.Detalle_compras
                .Where(dc => dc.id_compra == id)
                .ToListAsync();

            var modelo = new CompraDetalleViewModel
            {
                Id_compra = compra.Id_compra,
                numero_compra = compra.numero_compra,
                fecha_ingreso = compra.fecha_ingreso,
                proveedor = compra.ProveedorRel?.nombre ?? "",
                registrado_por = compra.UsuarioRel?.nombre_completo ?? "",
                monto_total = compra.monto_total,
                observaciones = compra.observaciones,
                Detalles = detalles.Select(dc => new DetalleCompraItem
                {
                    repuesto = _context.Repuestos
                        .Where(r => r.id_repuesto == dc.id_repuesto)
                        .Select(r => r.Nombre)
                        .FirstOrDefault() ?? "",
                    cantidad = dc.cantidad,
                    costo_unitario = dc.costo_unitario,
                    subtotal = dc.subtotal
                }).ToList()
            };

            return View(modelo);
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
        // GUARDAR NUEVA COMPRA
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraCreateViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var compra = new Compra
                {
                    numero_compra = modelo.numero_compra,
                    fecha_ingreso = modelo.fecha_ingreso,
                    id_proveedor = modelo.id_proveedor,
                    id_usuario = 1,
                    monto_total = 0,
                    observaciones = modelo.observaciones
                };

                _context.Add(compra);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(modelo);
        }


        // =========================
        // MOSTRAR FORMULARIO EDITAR
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var compra = await _context.Compras.FirstOrDefaultAsync(m => m.Id_compra == id);

            if (compra == null) return NotFound();

            var modelo = new AdministrarInventario
            {
                Id_compra = compra.Id_compra,
                numero_compra = compra.numero_compra,
                fecha_ingreso = compra.fecha_ingreso,
                monto_total = compra.monto_total,
                observaciones = compra.observaciones
            };

            return View(modelo);
        }


        // =========================
        // GUARDAR CAMBIOS
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdministrarInventario compra)
        {
            if (id != compra.Id_compra) return NotFound();

            if (ModelState.IsValid)
            {
                var existente = await _context.Compras.FindAsync(id);
                if (existente == null) return NotFound();

                existente.numero_compra = compra.numero_compra;
                existente.fecha_ingreso = compra.fecha_ingreso;
                existente.monto_total = compra.monto_total;
                existente.observaciones = compra.observaciones;

                _context.Update(existente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(compra);
        }


        // =========================
        // ACTIVAR / DESACTIVAR
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null) return NotFound();

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
