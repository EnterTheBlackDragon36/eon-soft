using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    [Authorize]
    public class AmazonController : Controller
    {
        public IActionResult AmazonAIServices()
        {
            return View();
        }

        public IActionResult TranslateAI()
        {
            return View();
        }

        public IActionResult SageMakerAI()
        {
            return View();
        }

        public IActionResult ForecastAI()
        {
            return View();
        }

        public IActionResult LexAI()
        {
            return View();
        }

        public IActionResult DeepComposerAI()
        {
            return View();
        }

        public IActionResult DeepRacerAI()
        {
            return View();
        }

        public IActionResult KendraAI()
        {
            return View();
        }

        public IActionResult MachineLearning()
        {
            return View();
        }
    }
}
