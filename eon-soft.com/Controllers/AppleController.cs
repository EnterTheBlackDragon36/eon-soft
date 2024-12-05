using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    [Authorize]
    public class AppleController : Controller
    {
        public IActionResult AppleAIServices()
        {
            return View();
        }

        public IActionResult NaturalLanguageAI()
        {
            return View();
        }

        public IActionResult SpeechAI()
        {
            return View();
        }

        public IActionResult SoundAnalysisAI()
        {
            return View();
        }

        public IActionResult TranslationAI()
        {
            return View();
        }

        public IActionResult MachineLearning()
        {
            return View();
        }
    }
}
