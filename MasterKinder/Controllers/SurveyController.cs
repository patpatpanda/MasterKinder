using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly CsvService _csvService;
    private readonly ILogger<SurveyController> _logger;

    public SurveyController(CsvService csvService, ILogger<SurveyController> logger)
    {
        _csvService = csvService;
        _logger = logger;
    }

    [HttpGet("questions")]
    public async Task<IActionResult> GetQuestions()
    {
        try
        {
            var questions = await _csvService.GetQuestionsAsync();
            if (questions == null)
            {
                return NotFound();
            }
            return Ok(questions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching questions.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("forskoleverksamheter")]
    public async Task<IActionResult> GetForskoleverksamheter()
    {
        try
        {
            var forskoleverksamheter = await _csvService.GetForskoleverksamheterAsync();
            if (forskoleverksamheter == null)
            {
                return NotFound();
            }
            return Ok(forskoleverksamheter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching forskoleverksamheter.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("response-percentages")]
    public async Task<IActionResult> GetResponsePercentages([FromBody] SurveyRequest request)
    {
        try
        {
            var responsePercentages = await _csvService.CalculateResponsePercentagesAsync(request.SelectedQuestion, request.SelectedForskoleverksamhet);
            return Ok(responsePercentages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calculating response percentages.");
            return StatusCode(500, "Internal server error");
        }
    }
}

public class SurveyRequest
{
    public string SelectedQuestion { get; set; }
    public string SelectedForskoleverksamhet { get; set; }
}
