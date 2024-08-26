public class Malibu
{
    public int Id { get; set; }
    public string? Namn { get; set; }
    public string? Helhetsomdome { get; set; }
    public int? Svarsfrekvens { get; set; }
    public int? AntalSvar { get; set; }
    public string? NormalizedNamn { get; set; }
    public List<Question> Questions { get; set; }
}

public class Question
{
    public int Id { get; set; }
    public string? FrageText { get; set; }
    public int? AndelInstammer { get; set; }
    public int? Year { get; set; }
    public int MalibuId { get; set; }
    public Malibu Malibu { get; set; }
}
