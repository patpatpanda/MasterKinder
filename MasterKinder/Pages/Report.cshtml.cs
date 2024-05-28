using MasterKinder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterKinder.Pages
{
    public class ReportModel : PageModel
    {
        public EmbedConfig EmbedConfig { get; private set; }

        public async Task OnGetAsync()
        {
            EmbedConfig = await PowerBIService.GetEmbedConfig();
        }
    }
}
