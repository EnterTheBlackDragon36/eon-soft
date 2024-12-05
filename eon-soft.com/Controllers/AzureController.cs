using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    [Authorize]
    public class AzureController : Controller
    {
        public IActionResult AzureAIServices()
        {
            return View();
        }


        public IActionResult MachineLearning()
        {
            return Ok();
        }

        
    }
}
