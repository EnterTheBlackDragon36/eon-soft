using eon_soft.com.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        public static IConfiguration Configuration { get; set; }

        public SecurityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ActionResult Index()
        {
            return View("~/Views/Security/Protected.cshtml");
        }

        public ActionResult ReviewToken()
        {
            TokenClient tokenClient = new TokenClient(Configuration);
            var token = tokenClient.GetIdToken(this.HttpContext);
            return View("~/Views/Security/Tokens.cshtml");
        }
    }
}
