using FlomarAPI.Data;
using FlomarAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlomarAPI.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly FlomarContext _context;

        public CategoriasController(FlomarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Listar()
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.nombre_categoria)
                .ToListAsync();

            return Ok(categorias);
        }
    }
}