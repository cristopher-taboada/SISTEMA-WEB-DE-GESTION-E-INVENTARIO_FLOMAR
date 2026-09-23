using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FLOMAR.Data;
using FLOMAR.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace FLOMAR.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlomarContext _context;

        public HomeController(FlomarContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin()
        {
            if (LoginController.RolActual != 1) return RedirectToAction("Index", "Login");
            return View();
        }

        public async Task<IActionResult> Vendedor(string textoBusqueda)
        {
            if (LoginController.RolActual != 2) return RedirectToAction("Index", "Login");

            var query = _context.Repuestos.AsQueryable();

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                query = query.Where(r => r.Nombre.Contains(textoBusqueda) || r.Codigo.Contains(textoBusqueda));
            }

            var listaRepuestos = await query.ToListAsync();

            return View(listaRepuestos);
        }
    }
}