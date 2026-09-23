using FlomarAPI.Data;
using FlomarAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/proveedores")]
    public class ProveedoresController : ControllerBase
    {
        private readonly FlomarContext _context;

        public ProveedoresController(FlomarContext context)
        {
            _context = context;
        }

        // GET: api/proveedores  (para el combo de compras)
        [HttpGet]
        public async Task<ActionResult<List<Proveedor>>> Listar()
        {
            return await _context.Proveedores
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }
    }
}
