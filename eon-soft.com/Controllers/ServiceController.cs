using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Service()
        {
            return View();
        }

        public IActionResult ViewAllServices()
        {
            return View();
        }

        public IActionResult AddService()
        {
            return View();
        }

        public IActionResult EditService()
        {
            return View();
        }

        public IActionResult RemoveService()
        {
            return View();
        }
    }
}
