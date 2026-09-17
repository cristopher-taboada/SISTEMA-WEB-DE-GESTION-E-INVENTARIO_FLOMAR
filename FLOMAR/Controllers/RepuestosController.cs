        using Microsoft.AspNetCore.Mvc;
        using Microsoft.AspNetCore.Mvc.Rendering;
        using Microsoft.EntityFrameworkCore;
        using FLOMAR.Data;
        using FLOMAR.Models;
        using System.Threading.Tasks;
        using System.Linq;

        namespace FLOMAR.Controllers
        {
            public class RepuestosController : Controller
            {
                private readonly FlomarContext _context;

                public RepuestosController(FlomarContext context)
                {
                    _context = context;
                }

                // MOSTRAR FORMULARIO CREAR
                [HttpGet]
                public async Task<IActionResult> Create()
                {
                    await CargarCategorias();

                    return View();
                }

                // (POST Create)
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create(Repuesto repuesto)
                {
                    if (repuesto == null)
                    {
                        return NotFound();
                    }

                    bool categoriaExiste = await _context.Categorias
                        .AnyAsync(c => c.Id == repuesto.IdCategoria);

                    if (!categoriaExiste)
                    {
                        ModelState.AddModelError(
                            nameof(repuesto.IdCategoria),
                            "Seleccione una categoría válida."
                        );
                    }

                    if (!ModelState.IsValid)
                    {
                        await CargarCategorias(repuesto.IdCategoria);
                        return View(repuesto);
                    }

                    _context.Repuestos.Add(repuesto);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                // ==========================================
                // EDITAR - GET
                // ==========================================
                [HttpGet]
                public async Task<IActionResult> Edit(int id)
                {
                    var repuesto = await _context.Repuestos
                        .FirstOrDefaultAsync(r => r.Id == id);

                    if (repuesto == null)
                    {
                        return NotFound();
                    }

                    await CargarCategorias(repuesto.IdCategoria);

                    return View(repuesto);
                }

                // ==========================================
                // EDITAR - POST
                // ==========================================
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(Repuesto repuesto)
                {
                    bool codigoExiste = await _context.Repuestos
                        .AnyAsync(r =>
                            r.Codigo == repuesto.Codigo &&
                            r.Id != repuesto.Id);

                    if (codigoExiste)
                    {
                        ModelState.AddModelError(
                            nameof(repuesto.Codigo),
                            "Ya existe otro repuesto con este código."
                        );
                    }

                    bool categoriaExiste = await _context.Categorias
                        .AnyAsync(c => c.Id == repuesto.IdCategoria);

                    if (!categoriaExiste)
                    {
                        ModelState.AddModelError(
                            nameof(repuesto.IdCategoria),
                            "Seleccione una categoría válida."
                        );
                    }

                    if (!ModelState.IsValid)
                    {
                        await CargarCategorias(repuesto.IdCategoria);
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

                    // IMPORTANTE:
                    // Ahora guardamos el ID, no un texto.
                    repuestoExistente.IdCategoria = repuesto.IdCategoria;

                    repuestoExistente.Costo = repuesto.Costo;
                    repuestoExistente.PrecioVenta = repuesto.PrecioVenta;
                    repuestoExistente.Stock = repuesto.Stock;
                    repuestoExistente.StockMinimo = repuesto.StockMinimo;

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                // ==========================================
                // ACTIVAR / DESACTIVAR
                // ==========================================
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> CambiarEstado(int id)
                {
                    var repuesto = await _context.Repuestos
                        .FirstOrDefaultAsync(r => r.Id == id);

                    if (repuesto == null)
                    {
                        return NotFound();
                    }

                    if (repuesto.IdEstadoRepuesto == 1)
                    {
                        // Inactivo
                        repuesto.IdEstadoRepuesto = 2;
                    }
                    else
                    {
                        // Activo
                        repuesto.IdEstadoRepuesto = 1;
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                // ==========================================
                // CARGAR CATEGORÍAS
                // ==========================================
                private async Task CargarCategorias(
                    int? categoriaSeleccionada = null)
                {
                    var categorias = await _context.Categorias
                        .OrderBy(c => c.NombreCategoria)
                        .ToListAsync();

                    ViewBag.Categorias = new SelectList(
                        categorias,
                        "Id",
                        "NombreCategoria",
                        categoriaSeleccionada
                    );
                }
            }
        }
