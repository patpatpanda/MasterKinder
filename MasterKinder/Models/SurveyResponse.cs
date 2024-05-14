using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class SurveyResponse
    {
        [Key]
        public int Id { get; set; }

        
        public string AvserAr { get; set; } // Anta år alltid är 4 tecken

        
        public string ResultatkategoriKod { get; set; }

        
        public string ResultatkategoriNamn { get; set; }

        
        public string Stadsdelsnamnd { get; set; }

        
        public string Forskoleenhet { get; set; }

       
        public string Organisatoriskenhetskod { get; set; }

        
        public string Forskoleverksamhet { get; set; }

        
        public string RegiformNamn { get; set; }

        
        public string FragaomradeNr { get; set; }

       
        public string Fragaomradestext { get; set; }

       
        public string FragaNr { get; set; }

        
        public string Fragetext { get; set; }

        
        public string Kortfragetext { get; set; }

        
        public string SvarsalternativTyp { get; set; }

        
        public string Fragetype { get; set; }


        public string Fragcategory { get; set; }


        public string AntalSvarsalternativ { get; set; }


        public string SvarsalternativNr { get; set; }

        public string SvarsalternativText { get; set; }


        public string GraderingSvarsalternativ { get; set; }


        public string Enkatroll { get; set; }


        public string Respondentroll { get; set; }


        public string Kon { get; set; }

      
        public string Utfall { get; set; }

 
        public string TotalVarde { get; set; }

    
        public string TotalVarde_ExklVetEj { get; set; }
    }
}
