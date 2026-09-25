using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlomarAPI.Data; // Asegúrate de que esta sea la ruta correcta de tu contexto
using FlomarAPI.Models; // Para que reconozca el modelo Venta

namespace FlomarAPI.Controllers
{
    [Route("api/ventas")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly FlomarContext _context;

        public VentasController(FlomarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVentas()
        {
            // Devuelve toda la tabla de ventas para el arqueo de caja
            return Ok(await _context.Ventas.ToListAsync());
        }
    }
}