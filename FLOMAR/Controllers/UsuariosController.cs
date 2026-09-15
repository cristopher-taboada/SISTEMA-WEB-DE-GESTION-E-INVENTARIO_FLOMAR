using Microsoft.AspNetCore.Mvc;

namespace FLOMAR.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}