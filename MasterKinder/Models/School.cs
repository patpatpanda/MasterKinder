using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MasterKinder.Models
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }

        [Required]
        public string SchoolName { get; set; }

        public int TotalResponses { get; set; }
        public double SatisfactionPercentage { get; set; }

        [JsonIgnore]
        public ICollection<Response>? Responses { get; set; } = new List<Response>();
    }
}
