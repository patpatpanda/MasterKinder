using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class SurveyResponses_2022 : ISurveyResponse
    {
       public int Id { get; set; }

        [MaxLength(4)]
        public string? AvserAr { get; set; }

        [MaxLength(255)]
        public string? ResultatkategoriKod { get; set; }

        [MaxLength(255)]
        public string? ResultatkategoriNamn { get; set; }

        [MaxLength(255)]
        public string? Stadsdelsnamnd { get; set; }

        [MaxLength(255)]
        public string? Forskoleenhet { get; set; }

        [MaxLength(255)]
        public string? Organisatoriskenhetskod { get; set; }

        [MaxLength(255)]
        public string? Forskoleverksamhet { get; set; }

        [MaxLength(200)]
        public string? RegiformNamn { get; set; }

        [MaxLength(500)]
        public string? FrageomradeNr { get; set; }

        [MaxLength(200)]
        public string? Frageomradestext { get; set; }

        [MaxLength(255)]
        public string? FrageNr { get; set; }

        [MaxLength(500)]
        public string? Fragetext { get; set; }

        [MaxLength(255)]
        public string? Kortfragetext { get; set; }

        [MaxLength(255)]
        public string? SvarsalternativTyp { get; set; }

        [MaxLength(255)]
        public string? Fragetyp { get; set; }

        [MaxLength(255)]
        public string? Fragekategori { get; set; }

        [MaxLength(255)]
        public string? AntalSvarsalternativ { get; set; }

        [MaxLength(255)]
        public string? SvarsalternativNr { get; set; }

        [MaxLength(200)]
        public string? SvarsalternativText { get; set; }

        [MaxLength(255)]
        public string? GraderingSvarsalternativ { get; set; }

        [MaxLength(255)]
        public string? Enkatroll { get; set; }

        [MaxLength(255)]
        public string? Respondentroll { get; set; }

        [MaxLength(255)]
        public string? Kon { get; set; }

        [MaxLength(255)]
        public string? Utfall { get; set; }

        [MaxLength(255)]
        public string? TotalVarde { get; set; }

        [MaxLength(255)]
        public string? TotalVarde_ExklVetEj { get; set; }
    }
}
