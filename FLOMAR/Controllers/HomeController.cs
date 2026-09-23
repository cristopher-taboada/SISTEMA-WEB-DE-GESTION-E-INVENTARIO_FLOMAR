using FLOMAR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <-- Importante para usar ToListAsync y Where
using FLOMAR.Data; // <-- Importante para conectar con tu FlomarContext
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace FLOMAR.Controllers
{
    public class HomeController : Controller
    {
        // ----------------------------------------------------
        // AGREGUE ESTO: BRAYAN FLORES (Inyección del Contexto de la BD)
        // ----------------------------------------------------
        // Explicación para estudiar: Declaramos la variable privada '_context' 
        // y modificamos el constructor de la clase para recibir la conexión a la base de datos 
        // mediante inyección de dependencias, permitiendo consultar las tablas de MySQL.
        private readonly FlomarContext _context;

        public HomeController(FlomarContext context)
        {
            _context = context;
        }
        // HASTA AQUI: BRAYAN FLORES (Inyección de dependencias)

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
       




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}