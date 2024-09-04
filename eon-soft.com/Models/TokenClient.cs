using Microsoft.AspNetCore.Authentication;
using System.Text.Json.Nodes;

namespace eon_soft.com.Models
{
    public class TokenClient
    {
        public static string ACCESS_TOKEN = "access_token";
        public static string REFRESH_TOKEN = "refresh_token";
        public static string ID_TOKEN = "id_token";

        private readonly IConfiguration configuration;

        public TokenClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /*
         * Return the current access token for display
         */
        public async Task<string> GetAccessToken(HttpContext context)
        {
            string accessToken = context.Request.Cookies[".AspNetCore.Cookies"].ToString();
            if (!string.IsNullOrEmpty(accessToken)) { ACCESS_TOKEN = accessToken; }
            return await context.GetTokenAsync(ACCESS_TOKEN);

        }

        /*
         * Return the current id token for display
         */
        public async Task<string> GetIdToken(HttpContext context)
        {
            string idToken = context.Request.Cookies[".AspNetCore.Cookies"].ToString();
            if (!string.IsNullOrEmpty(idToken)) { ID_TOKEN = idToken; }
            return await context.GetTokenAsync(ID_TOKEN);
        }

        /*
         * Return the current refresh token for display
         */
        public async Task<string> GetRefreshToken(HttpContext context)
        {
            return await context.GetTokenAsync(REFRESH_TOKEN);
        }

        /*
         * Do the work of getting new tokens and updating cookies
         */
        public async Task RefreshAccessToken(HttpContext context)
        {
            var tokens = await this.RefreshTokens(context);
            await RewriteCookies(tokens, context);
        }

        /*
         * Send the refresh token grant message
         */
        private async Task<JsonNode> RefreshTokens(HttpContext context)
        {
            var tokenEndpoint = this.configuration.GetValue<string>("OpenIdConnect:TokenEndpoint");

            var clientId = this.configuration.GetValue<string>("OpenIdConnect:ClientId");
            var clientSecret = this.configuration.GetValue<string>("OpenIdConnect:ClientSecret");
            var refreshToken = await context.GetTokenAsync(REFRESH_TOKEN);

            var requestData = new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>(REFRESH_TOKEN, refreshToken),
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");

                var response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(requestData));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonNode.Parse(json).AsObject();
            }
        }

        /*
         * Write updated cookies with new tokens
         */
        private async Task RewriteCookies(JsonNode tokens, HttpContext context)
        {
            var accessToken = tokens[ACCESS_TOKEN]?.ToString();
            var refreshToken = tokens[REFRESH_TOKEN]?.ToString();
            var idToken = tokens[ID_TOKEN]?.ToString();

            // An access token is always returned
            var newTokens = new List<AuthenticationToken>();
            newTokens.Add(new AuthenticationToken { Name = ACCESS_TOKEN, Value = accessToken });

            // A refresh token will be returned when rotating refresh tokens are used, which is the default in the Curity Identity Server
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                refreshToken = await context.GetTokenAsync(REFRESH_TOKEN);
            }
            newTokens.Add(new AuthenticationToken { Name = REFRESH_TOKEN, Value = refreshToken });

            // A new ID token is optional
            if (string.IsNullOrWhiteSpace(idToken))
            {
                idToken = await context.GetTokenAsync(ID_TOKEN);
            }
            newTokens.Add(new AuthenticationToken { Name = ID_TOKEN, Value = idToken });

            // Rewrite cookies
            var properties = context.Features.Get<IAuthenticateResultFeature>().AuthenticateResult.Properties;
            properties.StoreTokens(newTokens);
            await context.SignInAsync(context.User, properties);
        }
    }
}
