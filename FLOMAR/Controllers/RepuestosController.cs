using Microsoft.AspNetCore.Mvc;

namespace FLOMAR.Controllers
{
    public class RepuestosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}