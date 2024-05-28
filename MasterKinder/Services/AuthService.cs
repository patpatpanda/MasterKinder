using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace MasterKinder.Services
{
    public class AuthService
    {
        private static string clientId = "ca91c67a-7695-4c84-a97a-9b7eb18181dc";
        private static string tenantId = "451b0ef9-87ef-467b-ab71-572a777e4d7e";
        private static string clientSecret = "1am8Q~3ZU5qg~EBWs4YxC5A-Ac3DRA2rHXgNWbS~";
        private static string[] scopes = new string[] { "https://analysis.windows.net/powerbi/api/.default" };
        private static string authority = $"https://login.microsoftonline.com/{tenantId}";

        public static async Task<string> GetAccessToken()
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }
    }
}
