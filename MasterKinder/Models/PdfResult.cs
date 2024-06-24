using MasterKinder.Models;

namespace KinderReader.Models
{
    public class PdfResult
    {
        public int Id { get; set; }
        public string Helhetsomdome { get; set; }
        public string AntalSvar { get; set; }
        public string Svarsfrekvens { get; set; }
        public int ForskolanId { get; set; } // Foreign Key to Forskolan
        public Forskolan Forskolan { get; set; } // Navigation Property
    }
}
