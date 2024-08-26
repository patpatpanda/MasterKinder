using System.ComponentModel.DataAnnotations;

public class PdfData
{
    [Key]
    public int Id { get; set; }

    public string? Namn { get; set; }
    public string? Helhetsomdome { get; set; }
    public int? Svarsfrekvens { get; set; }
    public int? AntalSvar { get; set; }
    public string? NormalizedNamn { get; set; }

   
}
