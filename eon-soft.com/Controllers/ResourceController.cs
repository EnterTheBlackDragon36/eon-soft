using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    public class ResourceController : Controller
    {
        public IActionResult Resource()
        {
            return View();
        }

        public IActionResult ViewAllResources()
        {
            return View();
        }

        public IActionResult AddResource()
        {
            return View();
        }

        public IActionResult EditResource()
        {
            return View();
        }

        public IActionResult RemoveResource()
        {
            return View();
        }
    }
}
