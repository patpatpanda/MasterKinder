using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterKinder.Models
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }

        [ForeignKey("School")]
        public int SchoolId { get; set; }

        public string Category { get; set; }
        public string Question { get; set; }
        public double Percentage { get; set; }
        public string Gender { get; set; }
        public string Year { get; set; }

        public School School { get; set; }
    }
}
