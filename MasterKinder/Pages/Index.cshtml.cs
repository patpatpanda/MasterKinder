using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace MasterKinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CsvService _csvService;

        public IndexModel(CsvService csvService)
        {
            _csvService = csvService;
        }

        [BindProperty]
        public string SelectedQuestion { get; set; }
        public List<string> Questions { get; set; }
        public Dictionary<string, double> ResponsePercentages { get; set; }
        public double SpecificResponsePercentage { get; set; }
        public double OverallSatisfactionPercentage { get; set; }

        public void OnGet()
        {
            Questions = _csvService.GetAllQuestions();
        }

        public void OnPost()
        {
            Questions = _csvService.GetAllQuestions();
            ResponsePercentages = _csvService.CalculateResponsePercentages(SelectedQuestion);
            SpecificResponsePercentage = _csvService.CalculateSpecificResponsePercentage(SelectedQuestion, "Vet ej");
            OverallSatisfactionPercentage = _csvService.CalculateOverallSatisfactionPercentage(SelectedQuestion);
        }

        
    }
}
