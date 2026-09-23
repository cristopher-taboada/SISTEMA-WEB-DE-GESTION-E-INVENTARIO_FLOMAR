using FlomarAPI.Data;
using FlomarAPI.Models;
using FlomarAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/compras")]
    public class ComprasController : ControllerBase
    {
        private readonly FlomarContext _context;

        public ComprasController(FlomarContext context)
        {
            _context = context;
        }

        // LISTAR COMPRAS - GET: api/compras
        [HttpGet]
        public async Task<ActionResult<List<CompraListaDto>>> Listar()
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
                .Select(x => new CompraListaDto
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

            return compras;
        }

        // VER DETALLE - GET: api/compras/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompraDetalleDto>> Obtener(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.ProveedorRel)
                .Include(c => c.UsuarioRel)
                .FirstOrDefaultAsync(m => m.Id_compra == id);

            if (compra == null)
                return NotFound(new RespuestaApi { mensaje = "Compra no encontrada." });

            var detalles = await (
                from dc in _context.Detalle_compras
                join r in _context.Repuestos on dc.id_repuesto equals r.id_repuesto into gj
                from r in gj.DefaultIfEmpty()
                where dc.id_compra == id
                select new DetalleCompraItemDto
                {
                    codigo = r != null ? r.Codigo : "",
                    repuesto = r != null ? r.Nombre : "(repuesto eliminado)",
                    cantidad = dc.cantidad,
                    costo_unitario = dc.costo_unitario,
                    subtotal = dc.subtotal
                }
            ).ToListAsync();

            return new CompraDetalleDto
            {
                Id_compra = compra.Id_compra,
                numero_compra = compra.numero_compra,
                fecha_ingreso = compra.fecha_ingreso,
                id_proveedor = compra.id_proveedor,
                proveedor = compra.ProveedorRel?.nombre ?? "",
                registrado_por = compra.UsuarioRel?.nombre_completo ?? "",
                observaciones = compra.observaciones,
                monto_total = compra.monto_total,
                productos = detalles.Count,
                Detalles = detalles
            };
        }

        // NUMERO CORRELATIVO - GET: api/compras/siguiente-numero
        [HttpGet("siguiente-numero")]
        public async Task<IActionResult> SiguienteNumero()
        {
            int total = await _context.Compras.CountAsync();
            string siguiente = $"COMP-{(total + 1):D5}";

            return Ok(new { numero = siguiente });
        }

        // CREAR COMPRA (transaccion + stock) - POST: api/compras
        [HttpPost]
        public async Task<IActionResult> Crear(CompraCrearRequest request)
        {
            // Validar numero de compra unico
            bool numeroExiste = await _context.Compras
                .AnyAsync(c => c.numero_compra == request.numero_compra);

            if (numeroExiste)
            {
                return Conflict(new RespuestaApi
                {
                    mensaje = "Ya existe una compra registrada con este numero.",
                    campo = "numero_compra"
                });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Insertar cabecera
                var compra = new Compra
                {
                    numero_compra = request.numero_compra,
                    fecha_ingreso = request.fecha_ingreso,
                    id_proveedor = request.id_proveedor,
                    id_usuario = 1,
                    observaciones = request.observaciones,
                    monto_total = 0
                };

                _context.Compras.Add(compra);
                await _context.SaveChangesAsync(); // genera compra.Id_compra

                decimal montoTotal = 0;

                // Solo las filas con cantidad mayor a 0 se guardan
                var productosValidos = request.Productos
                    .Where(p => p.cantidad > 0)
                    .ToList();

                foreach (var prod in productosValidos)
                {
                    decimal subtotal = prod.cantidad * prod.costo_unitario;

                    var detalle = new Detalle_compra
                    {
                        id_compra = compra.Id_compra,
                        id_repuesto = prod.id_repuesto,
                        cantidad = prod.cantidad,
                        costo_unitario = prod.costo_unitario,
                        subtotal = subtotal
                    };
                    _context.Detalle_compras.Add(detalle);

                    // Sumar stock al repuesto
                    var repuesto = await _context.Repuestos
                        .FirstOrDefaultAsync(r => r.id_repuesto == prod.id_repuesto);
                    if (repuesto != null)
                    {
                        repuesto.stock_actual += prod.cantidad;
                        _context.Update(repuesto);
                    }

                    montoTotal += subtotal;
                }

                // Actualizar monto total calculado
                compra.monto_total = montoTotal;
                _context.Update(compra);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new RespuestaApi { mensaje = $"Compra {compra.numero_compra} registrada correctamente." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new RespuestaApi { mensaje = "Error al guardar la compra: " + ex.Message });
            }
        }

        // EDITAR CABECERA - PUT: api/compras/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(int id, CompraEditarRequest request)
        {
            var existente = await _context.Compras.FindAsync(id);
            if (existente == null)
                return NotFound(new RespuestaApi { mensaje = "Compra no encontrada." });

            existente.numero_compra = request.numero_compra;
            existente.fecha_ingreso = request.fecha_ingreso;
            existente.id_proveedor = request.id_proveedor;
            existente.monto_total = request.monto_total;
            existente.observaciones = request.observaciones ?? "";

            _context.Update(existente);
            await _context.SaveChangesAsync();

            return Ok(new RespuestaApi { mensaje = "Compra actualizada correctamente." });
        }

        // ELIMINAR (con reversion de stock) - DELETE: api/compras/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound(new RespuestaApi { mensaje = "Compra no encontrada." });

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

                return Ok(new RespuestaApi { mensaje = $"Compra {compra.numero_compra} eliminada y stock revertido correctamente." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new RespuestaApi { mensaje = "Error al eliminar la compra: " + ex.Message });
            }
        }
    }
}
