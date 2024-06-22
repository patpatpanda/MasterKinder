using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MasterKinder.Models
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }
        public string Question { get; set; }
        public double Percentage { get; set; }
        public string Gender { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
        public int SchoolId { get; set; }

        [JsonIgnore]
        public School School { get; set; }
    }
}
