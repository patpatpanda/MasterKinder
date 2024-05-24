using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterKinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CsvService _csvService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(CsvService csvService, ILogger<IndexModel> logger)
        {
            _csvService = csvService;
            _logger = logger;
        }

        [BindProperty]
        public List<string> Questions { get; set; }

        [BindProperty]
        public List<string> Forskoleverksamheter { get; set; }

        [BindProperty]
        public string SelectedQuestion { get; set; }

        [BindProperty]
        public string SelectedForskoleverksamhet { get; set; }

        public Dictionary<string, double> ResponsePercentages { get; set; }

        public int TotalResponses { get; set; }

        public bool SearchPerformed { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Questions = _csvService.GetQuestions();
            Forskoleverksamheter = _csvService.GetForskoleverksamheter();
            // Ensure initial load does not trigger search result message
            SelectedQuestion = null;
            SelectedForskoleverksamhet = null;
            ResponsePercentages = null;
            TotalResponses = 0;
            SearchPerformed = false; // No search performed initially
            return Page();
        }

        public IActionResult OnPostSelectQuestion()
        {
            Questions = _csvService.GetQuestions();
            Forskoleverksamheter = _csvService.GetForskoleverksamheter();
            ResponsePercentages = _csvService.CalculateResponsePercentages(SelectedQuestion, SelectedForskoleverksamhet);
            TotalResponses = _csvService.CountTotalResponses(SelectedQuestion, SelectedForskoleverksamhet);
            SearchPerformed = true; // Search has been performed

            _logger.LogInformation($"SelectedQuestion: {SelectedQuestion}, SelectedForskoleverksamhet: {SelectedForskoleverksamhet}");
            _logger.LogInformation($"ResponsePercentages count: {ResponsePercentages.Count}");
            _logger.LogInformation($"TotalResponses: {TotalResponses}");

            return Page();
        }
    }
}
