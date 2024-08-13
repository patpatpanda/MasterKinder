using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class SurveyResponse
    {
        public int Id { get; set; }
        public string AvserAr { get; set; }
      
        public string ResultatkategoriNamn { get; set; }
        public string Stadsdelsnamnd { get; set; }
        public string Forskoleenhet { get; set; }
      
        public string Forskoleverksamhet { get; set; }
        public string RegiformNamn { get; set; }
        public string FragaomradeNr { get; set; }
        public string Fragaomradestext { get; set; }
        public string FragaNr { get; set; }
        public string Fragetext { get; set; }
        public string Kortfragetext { get; set; }
        public string SvarsalternativTyp { get; set; }
        public string Fragetyp { get; set; }
        public string Fragkategori { get; set; }
       
        public string SvarsalternativNr { get; set; }
        public string SvarsalternativText { get; set; }
        
     
        
    }

}