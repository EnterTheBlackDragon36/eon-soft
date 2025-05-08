using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    public class AppsController : Controller
    {
        public IActionResult Apps()
        {
            return View();
        }
    }
}
