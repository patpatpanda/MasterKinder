using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class PdfDataAll
    {
        [Key]
        public int Id { get; set; }

        public string Namn { get; set; }
        public string Helhetsomdome { get; set; }
        public int Svarsfrekvens { get; set; }
        public int AntalSvar { get; set; }
        public string? NormalizedNamn { get; set; }

        // Nya egenskaper för frågorna
        public string FrageText { get; set; }
        public int InstammerHelt { get; set; }
        public int Instammer { get; set; }
    }
}
