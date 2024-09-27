using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Globalization;
using System.Text.Json.Nodes;

namespace eon_soft.com.Models
{
    public class TokenClient
    {
        public static string ACCESS_TOKEN = "access_token";
        public static string REFRESH_TOKEN = "refresh_token";
        public static string ID_TOKEN = "id_token";

        private readonly IConfiguration _configuration;

        public TokenClient(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /*
         * Return the current access token for display
         */
        public async Task<string> GetAccessToken(HttpContext context)
        {
            var accessToken = await context.GetTokenAsync(ACCESS_TOKEN); 
            if (!string.IsNullOrEmpty(accessToken)) { ACCESS_TOKEN = accessToken; }
            return accessToken;
        }

        /*
         * Return the current id token for display
         */
        public async Task<string> GetIdToken(HttpContext context)
        {

            var idToken = context.Request.Cookies[".AspNetCore.Cookies"];
            if (!string.IsNullOrEmpty(idToken)) { ID_TOKEN = idToken; }
            return idToken;
        }

        /*
         * Return the current refresh token for display
         */
        public async Task<string> GetRefreshToken(HttpContext context)
        {
            return await context.GetTokenAsync(REFRESH_TOKEN);
        }

        public async Task<string> GetAzureToken()
        {
            var tokenUri = _configuration.GetSection("AzureAd:TokenEndpoint").Value;
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(tokenUri);

            return accessToken;
        }

        public string GetToken()
        {

            // TODO: Install-Package Microsoft.IdentityModel.Clients.ActiveDirectory -Version 2.21.301221612
            // and add using Microsoft.IdentityModel.Clients.ActiveDirectory

            //The client id that Azure AD created when you registered your client app.
            string clientID = _configuration.GetSection("AzureAd:ClientId").ToString();
            string clientSecret = _configuration.GetSection("AzureAd:ClientSecret").ToString();
            string AuthEndPoint = _configuration.GetSection("AzureAd:TokenEndpoint").ToString();
            string TenantId = _configuration.GetSection("AzureAd:TenantId").ToString();

            //RedirectUri you used when you register your app.
            //For a client app, a redirect uri gives Azure AD more details on the application that it will authenticate.
            // You can use this redirect uri for your client app
            string redirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

            //Resource Uri for Power BI API
            string resourceUri = "https://analysis.windows.net/powerbi/api";

            //Get access token:
            // To call a Power BI REST operation, create an instance of AuthenticationContext and call AcquireToken
            // AuthenticationContext is part of the Active Directory Authentication Library NuGet package
            // To install the Active Directory Authentication Library NuGet package in Visual Studio,
            //  run "Install-Package Microsoft.IdentityModel.Clients.ActiveDirectory" from the nuget Package Manager Console.


            ClientCredential cc = new ClientCredential(clientID, clientSecret);


            // AcquireToken will acquire an Azure access token
            // Call AcquireToken to get an Azure token from Azure Active Directory token issuance endpoint
            string authority = string.Format(CultureInfo.InvariantCulture, AuthEndPoint, TenantId);
            AuthenticationContext authContext = new AuthenticationContext(authority);
            string token = authContext.AcquireTokenAsync("https://management.azure.com/", cc).Result.AccessToken;
            //Console.WriteLine(token);
            Console.ReadLine();
            return token;
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
            var tokenEndpoint = this._configuration.GetValue<string>("OpenIdConnect:TokenEndpoint");

            var clientId = this._configuration.GetValue<string>("OpenIdConnect:ClientId");
            var clientSecret = this._configuration.GetValue<string>("OpenIdConnect:ClientSecret");
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
