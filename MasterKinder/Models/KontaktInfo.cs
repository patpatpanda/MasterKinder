using MasterKinder.Models;

namespace KinderReader.Models
{
    public class KontaktInfo
    {
        public int Id { get; set; }
        public string? Namn { get; set; }
        public string? Epost { get; set; }
        public string? Telefon { get; set; }
        public string? Roll { get; set; }
        public int ForskolanId { get; set; } // Foreign Key
        public Forskolan Forskolan { get; set; } // Navigation Property
    }
}
