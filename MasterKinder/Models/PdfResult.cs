using MasterKinder.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class PdfResult
    {
        [Key]
        public int Id { get; set; }

        public string? Helhetsomdome { get; set; }
        public string? AntalSvar { get; set; }
        public string? Svarsfrekvens { get; set; }

        public int ForskolanId { get; set; }
        [ForeignKey("ForskolanId")]
        public Forskolan Forskolan { get; set; }
    }
}