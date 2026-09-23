using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FLOMAR.Data;

namespace FLOMAR.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly FlomarContext _context;

        public UsuariosController(FlomarContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .OrderBy(u => u.id_usuario)
                .ToListAsync();

            return View(usuarios);
        }
    }
}