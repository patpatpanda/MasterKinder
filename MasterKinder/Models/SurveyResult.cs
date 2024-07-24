using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterKinder.Models
{
    public class SurveyResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? AvserAr { get; set; }
        public string ResultatkategoriKod { get; set; }
        public string ResultatkategoriNamn { get; set; }
        public string Stadsdelsnamnd { get; set; }
        public string Forskolenhet { get; set; }
        public int? Organisatoriskenhetskod { get; set; }
        public string Forskoleverksamhet { get; set; }
        public string RegiformNamn { get; set; }
        public int? FragaOmradeNr { get; set; }
        public string FragaOmradeText { get; set; }
        public int? FragaNr { get; set; }
        public string FragaText { get; set; }
        public string KortFragaText { get; set; }
        public string SvarsalternativTyp { get; set; }
        public string FragaTyp { get; set; }
        public string FragaKategori { get; set; }
        public int? AntalSvarsalternativ { get; set; }
        public int? SvarsalternativNr { get; set; }
        public string SvarsalternativText { get; set; }
        public string GraderingSvarsalternativ { get; set; }
        public string EnkatRoll { get; set; }
        public string RespondentRoll { get; set; }
        public string Kon { get; set; }
        public double? Utfall { get; set; }
        public double? TotalVarde { get; set; }
        public double? TotalVardeExklVetEj { get; set; }

        // Add fields for extracted percentages
        public double? FragaNr1 { get; set; }
        public double? FragaNr2 { get; set; }
        public double? FragaNr3 { get; set; }
        public double? FragaNr4 { get; set; }
        public double? FragaNr5 { get; set; }
        public double? FragaNr6 { get; set; }
        public double? FragaNr7 { get; set; }
        public double? FragaNr8 { get; set; }
        public double? FragaNr9 { get; set; }
        public double? FragaNr10 { get; set; }
        public double? FragaNr11 { get; set; }
        public double? FragaNr12 { get; set; }
        public double? FragaNr13 { get; set; }
        public double? FragaNr14 { get; set; }
    }
}
