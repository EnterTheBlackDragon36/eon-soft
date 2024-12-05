using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    [Authorize]
    public class GoogleController : Controller
    {
        public IActionResult GoogleAIServices()
        {
            return View();
        }

        public IActionResult MachineLearning()
        {
            return View();
        }

        public IActionResult VertxAI()
        {
            return View();
        }

        public IActionResult TranslationAI()
        {
            return View();
        }


        public IActionResult SearchForRetailAI()
        {
            return View();
        }

        public IActionResult AgentBuilderAI()
        {
            return View();
        }

        public IActionResult SpeechAI()
        {
            return View();
        }

        public IActionResult VertexAIVision()
        {
            return View();
        }
    }
}
