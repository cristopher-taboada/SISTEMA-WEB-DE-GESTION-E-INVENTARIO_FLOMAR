using FlomarAPI.Data;
using FlomarAPI.Models;
using FlomarAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/ventas")]
    public class VentasController : ControllerBase
    {
        private readonly FlomarContext _context;

        public VentasController(FlomarContext context)
        {
            _context = context;
        }

        // Arma el comprobante completo de una venta (usado por GET y POST)
        private async Task<VentaDetalleDto> ConstruirDetalle(Venta venta)
        {
            var vendedor = await _context.Usuarios
                .Where(u => u.id_usuario == venta.id_vendedor)
                .Select(u => u.nombre_completo)
                .FirstOrDefaultAsync() ?? string.Empty;

            var metodoPago = await _context.MetodosPago
                .Where(m => m.id_metodo_pago == venta.id_metodo_pago)
                .Select(m => m.nombre_metodo)
                .FirstOrDefaultAsync() ?? string.Empty;

            var detalles = await (
                from dv in _context.Detalle_ventas
                join r in _context.Repuestos on dv.id_repuesto equals r.id_repuesto into gj
                from r in gj.DefaultIfEmpty()
                where dv.id_venta == venta.id_venta
                select new VentaDetalleItemDto
                {
                    codigo = r != null ? r.Codigo : string.Empty,
                    nombre = r != null ? r.Nombre : "(repuesto eliminado)",
                    cantidad = dv.cantidad,
                    precio_unitario = dv.precio_unitario,
                    subtotal = dv.subtotal
                }
            ).ToListAsync();

            return new VentaDetalleDto
            {
                id_venta = venta.id_venta,
                numero_transaccion = venta.numero_transaccion,
                fecha_hora = venta.fecha_hora,
                vendedor = vendedor,
                metodo_pago = metodoPago,
                tipo_comprobante = venta.tipo_comprobante,
                subtotal = venta.subtotal,
                monto_impuesto = venta.monto_impuesto,
                total_venta = venta.total_venta,
                detalles = detalles
            };
        }

        // VER DETALLE / COMPROBANTE - GET: api/ventas/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentaDetalleDto>> Obtener(int id)
        {
            var venta = await _context.Ventas
                .FirstOrDefaultAsync(v => v.id_venta == id);

            if (venta == null)
                return NotFound(new RespuestaApi { mensaje = "Venta no encontrada." });

            return await ConstruirDetalle(venta);
        }

        // REGISTRAR VENTA - POST: api/ventas
        // En una sola transaccion: valida stock, guarda cabecera y detalles,
        // y descuenta el stock. Si algo falla, no se guarda nada.
        [HttpPost]
        public async Task<ActionResult<VentaDetalleDto>> Crear(VentaCrearRequest request)
        {
            if (request.lineas == null || request.lineas.Count == 0)
                return BadRequest(new RespuestaApi { mensaje = "El carrito esta vacio.", campo = "lineas" });

            // Si el carrito trae el mismo repuesto dos veces, se agrupa
            // (DETALLE_VENTA tiene UNIQUE(id_venta, id_repuesto))
            var lineas = request.lineas
                .GroupBy(l => l.id_repuesto)
                .Select(g => new VentaLineaRequest
                {
                    id_repuesto = g.Key,
                    cantidad = g.Sum(x => x.cantidad)
                })
                .ToList();

            if (lineas.Any(l => l.cantidad <= 0))
                return BadRequest(new RespuestaApi { mensaje = "Las cantidades deben ser mayores a cero.", campo = "lineas" });

            var ids = lineas.Select(l => l.id_repuesto).ToList();
            var repuestos = await _context.Repuestos
                .Where(r => ids.Contains(r.id_repuesto))
                .ToListAsync();

            // Validar existencia y stock ANTES de guardar nada
            foreach (var linea in lineas)
            {
                var rep = repuestos.FirstOrDefault(r => r.id_repuesto == linea.id_repuesto);

                if (rep == null)
                    return BadRequest(new RespuestaApi { mensaje = $"El repuesto con id {linea.id_repuesto} no existe.", campo = "lineas" });

                if (rep.stock_actual < linea.cantidad)
                    return Conflict(new RespuestaApi
                    {
                        mensaje = $"Stock insuficiente para \"{rep.Nombre}\". Disponible: {rep.stock_actual}, solicitado: {linea.cantidad}.",
                        campo = "lineas"
                    });
            }

            // El IVA vigente (13%) se lee del catalogo IMPUESTO
            var impuesto = await _context.Impuestos.FirstOrDefaultAsync(i => i.vigente);
            var porcentaje = impuesto?.porcentaje ?? 0m;

            // Los precios se toman de la base de datos (no del navegador)
            decimal subtotal = lineas.Sum(l =>
                repuestos.First(r => r.id_repuesto == l.id_repuesto).PrecioVenta * l.cantidad);
            decimal montoImpuesto = Math.Round(subtotal * porcentaje / 100m, 2);
            decimal total = subtotal + montoImpuesto;

            // Numero de transaccion correlativo
            var correlativo = await _context.Ventas.CountAsync() + 1;
            var numero = $"VTA-{correlativo:D5}";
            while (await _context.Ventas.AnyAsync(v => v.numero_transaccion == numero))
            {
                correlativo++;
                numero = $"VTA-{correlativo:D5}";
            }

            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var venta = new Venta
                {
                    numero_transaccion = numero,
                    fecha_hora = DateTime.Now,
                    id_cliente = null, // cliente mostrador
                    id_vendedor = request.id_vendedor > 0 ? request.id_vendedor : 1,
                    id_metodo_pago = request.id_metodo_pago > 0 ? request.id_metodo_pago : 1,
                    id_impuesto = impuesto?.id_impuesto ?? 1,
                    id_estado_venta = 1, // Confirmada
                    subtotal = subtotal,
                    monto_impuesto = montoImpuesto,
                    total_venta = total,
                    tipo_comprobante = "Recibo Interno"
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync(); // genera venta.id_venta

                foreach (var linea in lineas)
                {
                    var rep = repuestos.First(r => r.id_repuesto == linea.id_repuesto);

                    _context.Detalle_ventas.Add(new Detalle_venta
                    {
                        id_venta = venta.id_venta,
                        id_repuesto = rep.id_repuesto,
                        cantidad = linea.cantidad,
                        precio_unitario = rep.PrecioVenta,
                        subtotal = rep.PrecioVenta * linea.cantidad
                    });

                    // La venta descuenta el stock
                    rep.stock_actual -= linea.cantidad;
                }

                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();

                return CreatedAtAction(
                    nameof(Obtener),
                    new { id = venta.id_venta },
                    await ConstruirDetalle(venta));
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }
    }
}
