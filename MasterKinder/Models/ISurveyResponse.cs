namespace MasterKinder.Models
{
    public interface ISurveyResponse
    {
        int Id { get; set; }
        string Forskoleverksamhet { get; set; }
        string Fragetext { get; set; }
        string GraderingSvarsalternativ { get; set; }
        string Utfall { get; set; }
        string SvarsalternativText { get; set; }
       string FrageNr { get; set; }
    }
   
}
