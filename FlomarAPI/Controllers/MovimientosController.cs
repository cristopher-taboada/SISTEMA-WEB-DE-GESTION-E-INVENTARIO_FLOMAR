using FlomarAPI.Data;
using FlomarAPI.Models;
using FlomarAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/movimientos")]
    public class MovimientosController : ControllerBase
    {
        private readonly FlomarContext _context;

        public MovimientosController(FlomarContext context)
        {
            _context = context;
        }

        // LISTAR MOVIMIENTOS (kardex) - GET: api/movimientos
        [HttpGet]
        public async Task<ActionResult<List<MovimientoListaDto>>> Listar()
        {
            var movimientos = await (
                from m in _context.Movimientos
                join r in _context.Repuestos on m.id_repuesto equals r.id_repuesto
                join tm in _context.TiposMovimiento on m.id_tipo_movimiento equals tm.id_tipo_movimiento
                join u in _context.Usuarios on m.id_usuario equals u.id_usuario
                orderby m.fecha_movimiento descending
                select new MovimientoListaDto
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

            return movimientos;
        }

        // VER DETALLE - GET: api/movimientos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovimientoDetalleDto>> Obtener(int id)
        {
            var movimiento = await (
                from m in _context.Movimientos
                join r in _context.Repuestos on m.id_repuesto equals r.id_repuesto
                join tm in _context.TiposMovimiento on m.id_tipo_movimiento equals tm.id_tipo_movimiento
                join u in _context.Usuarios on m.id_usuario equals u.id_usuario
                where m.id_movimiento == id
                select new MovimientoDetalleDto
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

            if (movimiento == null)
                return NotFound(new RespuestaApi { mensaje = "Movimiento no encontrado." });

            return movimiento;
        }

        // CREAR MOVIMIENTO (transaccion + stock) - POST: api/movimientos
        [HttpPost]
        public async Task<IActionResult> Crear(MovimientoCrearRequest request)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.id_repuesto == request.id_repuesto);

            if (repuesto == null)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = "El repuesto seleccionado no existe.",
                    campo = "id_repuesto"
                });
            }

            var tipo = await _context.TiposMovimiento
                .FirstOrDefaultAsync(t => t.id_tipo_movimiento == request.id_tipo_movimiento);

            if (tipo == null)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = "El tipo de movimiento seleccionado no existe.",
                    campo = "id_tipo_movimiento"
                });
            }

            // El signo solo puede ser 1 (suma) o -1 (resta)
            if (tipo.signo != 1 && tipo.signo != -1)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = "El tipo de movimiento tiene un signo invalido en la base de datos (debe ser 1 o -1).",
                    campo = "id_tipo_movimiento"
                });
            }

            // Un movimiento de salida NO puede dejar el stock en negativo
            if (tipo.signo == -1 && repuesto.stock_actual < request.cantidad)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = $"Stock insuficiente: '{repuesto.Nombre}' tiene {repuesto.stock_actual} unidades y se intentan sacar {request.cantidad}.",
                    campo = "cantidad"
                });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Insertar movimiento
                var movimiento = new MovimientoInventario
                {
                    fecha_movimiento = DateTime.Now,
                    id_repuesto = request.id_repuesto,
                    cantidad = request.cantidad,
                    id_tipo_movimiento = request.id_tipo_movimiento,
                    id_usuario = 1,
                    motivo = request.motivo,
                    observaciones = request.observaciones
                };

                _context.Movimientos.Add(movimiento);

                // 2. Actualizar stock segun signo
                if (tipo.signo == 1)
                    repuesto.stock_actual += request.cantidad;
                else // signo == -1 (ya validado arriba)
                    repuesto.stock_actual -= request.cantidad;

                _context.Update(repuesto);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new RespuestaApi { mensaje = "Movimiento registrado y stock actualizado correctamente." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new RespuestaApi { mensaje = "Error al guardar: " + ex.Message });
            }
        }

        // ELIMINAR CON REVERSION - DELETE: api/movimientos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
                return NotFound(new RespuestaApi { mensaje = "Movimiento no encontrado." });

            var tipo = await _context.TiposMovimiento.FindAsync(movimiento.id_tipo_movimiento);
            var repuesto = await _context.Repuestos.FindAsync(movimiento.id_repuesto);

            if (tipo == null || repuesto == null)
                return NotFound(new RespuestaApi { mensaje = "Datos relacionados no encontrados." });

            // Al revertir un INGRESO se resta stock: verificar que no quede negativo
            if (tipo.signo == 1 && repuesto.stock_actual < movimiento.cantidad)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = $"No se puede eliminar: el stock actual de '{repuesto.Nombre}' es {repuesto.stock_actual} " +
                              $"y revertir este ingreso requiere descontar {movimiento.cantidad} unidades."
                });
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

                return Ok(new RespuestaApi { mensaje = "Movimiento eliminado y stock revertido correctamente." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new RespuestaApi { mensaje = "Error al eliminar: " + ex.Message });
            }
        }
    }
}
