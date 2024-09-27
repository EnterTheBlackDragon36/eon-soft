using eon_soft.com.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eon_soft.com.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        public static IConfiguration Configuration { get; set; }
        public readonly HttpContextAccessor _httpContextAccessor;

        public SecurityController(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public IActionResult Protected()
        {
            return View();
        }

        public IActionResult Tokens()
        {
            TokenClient tokenClient = new TokenClient(Configuration);

            var idToken = "";
            //HttpCookie
            //System.Web.HttpCookie cookie = Request.Cookies["AspNetCore.Cookies"];
            //var token = tokenClient.GetToken();
            //var cookie = this.HttpContext.Request.Cookies["AspNetCore.Cookies"];

            var cookieName = ".AspNetCore.Cookies";

            var cookies = Request.Cookies;
            foreach (var cookie in cookies)
            {
                string name = cookie.Key;
                string value = cookie.Value;
                if (name == cookieName) { idToken = value; }
            }
         
            //var token = this.HttpContext.Response.Cookies["AspNetCore.Cookies"].Value;
            var token1 = tokenClient.GetAzureToken();
            //var accessToken = tokenClient.GetAccessToken(this.HttpContext);
            //var idToken = tokenClient.GetIdToken(this.HttpContext);
            //var refreshToken = tokenClient.GetRefreshToken(this.HttpContext);
            //ViewBag.AccessToken = accessToken;
            //ViewBag.RefreshToken = refreshToken;
            ViewBag.IdToken = idToken;
            return View();
        }

        //public ActionResult Index()
        //{
        //    return View("~/Views/Security/Protected.cshtml");
        //}

        //public ActionResult ReviewToken()
        //{
        //    TokenClient tokenClient = new TokenClient(Configuration);
        //    var token = tokenClient.GetIdToken(this.HttpContext);
        //    return View("~/Views/Security/Tokens.cshtml");
        //}
    }
}
