<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿
using FLOMAR.Models;
using Microsoft.AspNetCore.Mvc;
>>>>>>> Stashed changes

namespace FLOMAR.Controllers
{
    public class LoginController : Controller
    {
<<<<<<< Updated upstream
=======
        [HttpGet]
>>>>>>> Stashed changes
        public IActionResult Index()
        {
            return View();
        }
<<<<<<< Updated upstream
    }
}
=======

        [HttpPost]
        public IActionResult Index(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

>>>>>>> Stashed changes
