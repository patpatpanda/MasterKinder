using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class PdfData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Namn { get; set; }
        public string Helhetsomdome { get; set; }
        public int Svarsfrekvens { get; set; }
        public int AntalSvar { get; set; }
    }
}
