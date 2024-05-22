using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class SurveyResponse
    {
        public int Id { get; set; }
        public string AvserAr { get; set; }
        public string ResultatkategoriKod { get; set; }
        public string ResultatkategoriNamn { get; set; }
        public string Stadsdelsnamnd { get; set; }
        public string Forskoleenhet { get; set; }
        public string Organisatoriskenhetskod { get; set; }
        public string Forskoleverksamhet { get; set; }
        public string RegiformNamn { get; set; }
        public int FragaomradeNr { get; set; }
        public string Fragaomradestext { get; set; }
        public int FragaNr { get; set; }
        public string Fragetext { get; set; }
        public string Kortfragetext { get; set; }
        public string SvarsalternativTyp { get; set; }
        public string Fragetyp { get; set; }
        public string Fragkategori { get; set; }
        public int AntalSvarsalternativ { get; set; }
        public int SvarsalternativNr { get; set; }
        public string SvarsalternativText { get; set; }
        public string GraderingSvarsalternativ { get; set; }
        public string Enkatroll { get; set; }
        public string Respondentroll { get; set; }
        public string Kon { get; set; }
        public int Utfall { get; set; }
        public int TotalVarde { get; set; }
        public int TotalVarde_ExklVetEj { get; set; }
    }

}
