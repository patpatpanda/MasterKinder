using KinderReader.Models;

public class Forskolan
{
    public int Id { get; set; }
    public string? Namn { get; set; }
    public string? Adress { get; set; }
    public string? Beskrivning { get; set; }
    public string? TypAvService { get; set; }
    public string? VerksamI { get; set; }
    public string? Organisationsform { get; set; }
    public int AntalBarn { get; set; }
    public double? AntalBarnPerArsarbetare { get; set; }
    public double? AndelLegitimeradeForskollarare { get; set; }
    public string? Webbplats { get; set; }
    public string? InriktningOchProfil { get; set; }
    public string? InneOchUtemiljo { get; set; }
    public string? KostOchMaltider { get; set; }
    public string? MalOchVision { get; set; }
    public string? MerOmOss { get; set; }
    public ICollection<KontaktInfo>? Kontakter { get; set; }
    public double Latitude { get; set; } // Nytt fält
    public double Longitude { get; set; } // Nytt fält
    public string? BildUrl { get; set; } // Nytt fält för bild
}
