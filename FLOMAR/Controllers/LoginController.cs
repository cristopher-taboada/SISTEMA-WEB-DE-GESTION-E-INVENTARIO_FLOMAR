using Microsoft.AspNetCore.Mvc;

namespace FLOMAR.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}