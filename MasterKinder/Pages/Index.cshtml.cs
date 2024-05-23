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

        public async Task<IActionResult> OnGetAsync()
        {
            Questions = _csvService.GetQuestions();
            Forskoleverksamheter = _csvService.GetForskoleverksamheter();

            if (Questions != null && Questions.Any())
            {
                SelectedQuestion = Questions.FirstOrDefault();
                ResponsePercentages = _csvService.CalculateResponsePercentages(SelectedQuestion, SelectedForskoleverksamhet);
            }

            return Page();
        }

        public IActionResult OnPostSelectQuestion()
        {
            Questions = _csvService.GetQuestions();
            Forskoleverksamheter = _csvService.GetForskoleverksamheter();
            ResponsePercentages = _csvService.CalculateResponsePercentages(SelectedQuestion, SelectedForskoleverksamhet);

            _logger.LogInformation($"SelectedQuestion: {SelectedQuestion}, SelectedForskoleverksamhet: {SelectedForskoleverksamhet}");
            _logger.LogInformation($"ResponsePercentages count: {ResponsePercentages.Count}");

            return Page();
        }
    }
}
