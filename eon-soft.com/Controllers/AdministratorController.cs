using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    public class AdministratorController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult ManageFeatures()
        {
            return View();
        }

        public IActionResult ManageServices()
        {
            return View();
        }

        public IActionResult ManageUsers()
        {
            return View();
        }

        public IActionResult ManageApplications()
        {
            string gitUrl = string.Empty;
            string gitProject = string.Empty;
            string gitApplication = string.Empty;
            return View();
        }
    }
}
