using FlomarAPI.Data;
using FlomarAPI.Models;
using FlomarAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/repuestos")]
    public class RepuestosController : ControllerBase
    {
        private readonly FlomarContext _context;

        public RepuestosController(FlomarContext context)
        {
            _context = context;
        }

        // GET: api/repuestos?textoBusqueda=filtro
        [HttpGet]
        public async Task<ActionResult<List<Repuesto>>> Listar([FromQuery] string? textoBusqueda)
        {
            var query = _context.Repuestos.AsQueryable();

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                query = query.Where(r => r.Nombre.Contains(textoBusqueda) || r.Codigo.Contains(textoBusqueda));
            }

            return await query.OrderBy(r => r.Nombre).ToListAsync();
        }

        // GET: api/repuestos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Repuesto>> Obtener(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.id_repuesto == id);

            if (repuesto == null)
                return NotFound(new RespuestaApi { mensaje = "Repuesto no encontrado." });

            return repuesto;
        }

        // POST: api/repuestos
        [HttpPost]
        public async Task<ActionResult<Repuesto>> Crear(Repuesto repuesto)
        {
            bool codigoExiste = await _context.Repuestos
                .AnyAsync(r => r.Codigo == repuesto.Codigo);

            if (codigoExiste)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = "Ya existe un repuesto con este código.",
                    campo = "Codigo"
                });
            }

            if (repuesto.PrecioVenta < repuesto.costo_adquisicion)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = "El precio de venta no puede ser menor al costo.",
                    campo = "PrecioVenta"
                });
            }

            repuesto.id_repuesto = 0;
            repuesto.id_estado = 1;

            _context.Repuestos.Add(repuesto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Obtener), new { id = repuesto.id_repuesto }, repuesto);
        }

        // PUT: api/repuestos/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Repuesto>> Editar(int id, Repuesto repuesto)
        {
            var actual = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.id_repuesto == id);

            if (actual == null)
                return NotFound(new RespuestaApi { mensaje = "Repuesto no encontrado." });

            bool codigoExiste = await _context.Repuestos
                .AnyAsync(r => r.Codigo == repuesto.Codigo && r.id_repuesto != id);

            if (codigoExiste)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = "Ya existe otro repuesto con este código.",
                    campo = "Codigo"
                });
            }

            if (repuesto.PrecioVenta < repuesto.costo_adquisicion)
            {
                return BadRequest(new RespuestaApi
                {
                    mensaje = "El precio de venta no puede ser menor al costo.",
                    campo = "PrecioVenta"
                });
            }

            actual.Codigo = repuesto.Codigo;
            actual.Nombre = repuesto.Nombre;
            actual.id_categoria = repuesto.id_categoria;
            actual.costo_adquisicion = repuesto.costo_adquisicion;
            actual.PrecioVenta = repuesto.PrecioVenta;
            actual.stock_actual = repuesto.stock_actual;
            actual.stock_minimo = repuesto.stock_minimo;

            await _context.SaveChangesAsync();

            return Ok(actual);
        }

        // POST: api/repuestos/5/cambiar-estado
        [HttpPost("{id:int}/cambiar-estado")]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var repuesto = await _context.Repuestos
                .FirstOrDefaultAsync(r => r.id_repuesto == id);

            if (repuesto == null)
                return NotFound(new RespuestaApi { mensaje = "Repuesto no encontrado." });

            repuesto.id_estado = repuesto.id_estado == 1 ? 2 : 1;

            await _context.SaveChangesAsync();

            return Ok(new RespuestaApi { mensaje = "Estado del repuesto actualizado." });
        }
    }
}
