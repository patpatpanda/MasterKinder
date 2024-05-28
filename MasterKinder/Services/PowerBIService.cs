using MasterKinder.Services;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using System;
using System.Threading.Tasks;

namespace MasterKinder.Services
{
    public class PowerBIService
    {
        private static Guid workspaceId = new Guid("d8caeb85-b1c6-489c-bdf5-5d6ea740d027"); // Ersätt med ditt arbetsyta-ID
        private static Guid reportId = new Guid("96bb9060-289c-4616-bac5-4110c962a548"); // Ersätt med ditt rapport-ID


        public static async Task<EmbedConfig> GetEmbedConfig()
        {
            string accessToken = await AuthService.GetAccessToken();

            TokenCredentials tokenCredentials = new TokenCredentials(accessToken, "Bearer");
            using (var client = new PowerBIClient(new Uri("https://api.powerbi.com"), tokenCredentials))
            {
                var report = await client.Reports.GetReportInGroupAsync(workspaceId, reportId);

                return new EmbedConfig
                {
                    EmbedUrl = report.EmbedUrl,
                    AccessToken = accessToken
                };
            }
        }
    }

    public class EmbedConfig
    {
        public string EmbedUrl { get; set; }
        public string AccessToken { get; set; }
    }
}
