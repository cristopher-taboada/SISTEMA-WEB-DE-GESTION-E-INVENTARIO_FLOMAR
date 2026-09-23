using FlomarAPI.Data;
using FlomarAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/tiposmovimiento")]
    public class TiposMovimientoController : ControllerBase
    {
        private readonly FlomarContext _context;

        public TiposMovimientoController(FlomarContext context)
        {
            _context = context;
        }

        // GET: api/tiposmovimiento  (para el combo de movimientos)
        [HttpGet]
        public async Task<ActionResult<List<TipoMovimiento>>> Listar()
        {
            return await _context.TiposMovimiento
                .OrderBy(t => t.id_tipo_movimiento)
                .ToListAsync();
        }
    }
}
