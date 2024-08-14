using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class Survey
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string AvserAr { get; set; }

        [MaxLength(50)]
        public string ResultatkategoriNamn { get; set; }

        [MaxLength(50)]
        public string Stadsdelsnamnd { get; set; }

        [MaxLength(100)]
        public string Forskoleenhet { get; set; }

        [MaxLength(50)]
        public string Forskoleverksamhet { get; set; }

       

       

        [MaxLength(100)]
        public string Fragaomradestext { get; set; }

        [MaxLength(255)]
        public string FragaNr { get; set; }

        [MaxLength(255)]
        public string Fragetext { get; set; }

        [MaxLength(255)]
        public string Kortfragetext { get; set; }

        [MaxLength(255)]
        public string SvarsalternativTyp { get; set; }

        [MaxLength(255)]
        public string Fragetyp { get; set; }

        [MaxLength(255)]
        public string Fragkategori { get; set; }

        [MaxLength(255)]
        public string SvarsalternativNr { get; set; }

        [MaxLength(255)]
        public string SvarsalternativText { get; set; }
    }
}
