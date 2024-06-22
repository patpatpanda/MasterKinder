namespace MasterKinder.Models
{
    public class CreateSchoolRequest
    {
        public string SchoolName { get; set; }
        public int TotalResponses { get; set; }
        public double SatisfactionPercentage { get; set; }
        public List<CreateResponseRequest> Responses { get; set; }
    }
}
