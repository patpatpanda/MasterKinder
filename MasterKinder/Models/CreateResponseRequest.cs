namespace MasterKinder.Models
{
    public class CreateResponseRequest
    {
        public string Question { get; set; }
        public double Percentage { get; set; }
        public string Gender { get; set; }
        public string Year { get; set; }
        public string Category { get; set; } // Lägg till detta fält
    }
}
