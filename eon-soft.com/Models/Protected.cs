﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace eon_soft.com.Models
{
    [Authorize]
    public class ProtectedModel : PageModel
    {
        public string? Username { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        private readonly TokenClient tokenClient;

        public ProtectedModel(TokenClient tokenClient)
        {
            this.tokenClient = tokenClient;
        }

        public async Task OnGet()
        {
            ClaimsPrincipal user = this.User;
            var givenName = user.FindFirstValue("given_name");
            var familyName = user.FindFirstValue("family_name");
            this.Username = $"{givenName} {familyName}";

            this.AccessToken = await this.tokenClient.GetAccessToken(this.HttpContext);
            this.RefreshToken = await this.tokenClient.GetRefreshToken(this.HttpContext);
        }

        public ClaimsPrincipal OnGetClaims()
        {
            ClaimsPrincipal user = this.User;
            var givenName = user.FindFirstValue("given_name");
            var familyName = user.FindFirstValue("family_name");
            this.Username = $"{givenName} {familyName}";

            this.AccessToken = this.tokenClient.GetAccessToken(this.HttpContext).ToString();
            this.RefreshToken = this.tokenClient.GetRefreshToken(this.HttpContext).ToString();

            return user;
        }

        public async Task<IActionResult> OnPostRefreshToken()
        {
            await this.tokenClient.RefreshAccessToken(this.HttpContext);
            this.AccessToken = await this.tokenClient.GetAccessToken(this.HttpContext);
            this.RefreshToken = await this.tokenClient.GetRefreshToken(this.HttpContext);
            return Page();
        }

        public async Task OnPostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
