namespace MasterKinder.Models
{
     public interface ISurveyResponse
    {
        int Id { get; set; }
        string? Forskoleverksamhet { get; set; }
         string? AvserAr { get; set; }
        string? Fragetext { get; set; }
        string? GraderingSvarsalternativ { get; set; }
        int Utfall { get; set; }  // Ändrat från string? till int
        string? SvarsalternativText { get; set; }
        string? FrageNr { get; set; }
        int SvarsalternativNr { get; set; }  // Ny egenskap
        int TotalVarde { get; set; }         // Ny egenskap
        int TotalVarde_ExklVetEj { get; set; }  // Ny egenskap
    }
}
