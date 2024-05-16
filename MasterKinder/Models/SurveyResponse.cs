using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
      public class SurveyResponse
    {
        [Key]
        public int Id { get; set; }

        public string AvserAr { get; set; }
       
        public string Stadsdelsnamnd { get; set; }
        public string Forskoleenhet { get; set; }
        public string Fragaomradestext { get; set; }
        public string Fragetext { get; set; }
        public string SvarsalternativText { get; set; }
       
    }
}
