using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult Application()
        {
            return View();
        }

        public IActionResult ViewAllApplication()
        {
            return View();
        }

        public IActionResult EditApplication()
        {
            return View();
        }

        public IActionResult RemoveApplication()
        {
            return View();
        }

        public IActionResult ConfigureApplication()
        {
            return View();
        }
    }
}
