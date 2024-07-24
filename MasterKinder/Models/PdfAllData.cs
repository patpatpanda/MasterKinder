using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterKinder.Models
{
    public class PdfAllData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Namn { get; set; }
        public string? Helhetsomdome { get; set; }
        public int? Svarsfrekvens { get; set; }
        public int? AntalSvar { get; set; }
        public string? NormalizedNamn { get; set; }

        // New fields based on the PDF content
        public double? UtvecklingOchLarande { get; set; }
        public double? SprakligUtveckling { get; set; }
        public double? FårStod { get; set; }
        public double? HållbarUtveckling { get; set; }
        public double? InformationOmUtvecklingOchLarande { get; set; }
        public double? Trygghet { get; set; }
        public double? SocialaFormagor { get; set; }
        public double? PersonalensOmsorg { get; set; }
        public double? PositivBildAvSigSjälv { get; set; }
        public double? LikaMojligheter { get; set; }
        public double? LedningTillganglighet { get; set; }
        public double? PersonalensBemotande { get; set; }
        public double? MatInformation { get; set; }
        public double? FysiskaAktiviteter { get; set; }
        public double? SamverkanMedHemmet { get; set; }

        // Additional fields for each percentage
        public double? TrygghetAndelInstammer { get; set; }
        public double? SprakligUtvecklingAndelInstammer { get; set; }
        public double? FårStodAndelInstammer { get; set; }
        public double? HållbarUtvecklingAndelInstammer { get; set; }
        public double? InformationOmUtvecklingOchLarandeAndelInstammer { get; set; }
        public double? SocialaFormagorAndelInstammer { get; set; }
        public double? PersonalensOmsorgAndelInstammer { get; set; }
        public double? PositivBildAvSigSjälvAndelInstammer { get; set; }
        public double? LikaMojligheterAndelInstammer { get; set; }
        public double? LedningTillganglighetAndelInstammer { get; set; }
        public double? PersonalensBemotandeAndelInstammer { get; set; }
        public double? MatInformationAndelInstammer { get; set; }
        public double? FysiskaAktiviteterAndelInstammer { get; set; }
        public double? SamverkanMedHemmetAndelInstammer { get; set; }
    }
}
