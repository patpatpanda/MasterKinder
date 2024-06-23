using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public int NumberOfChildren { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        // Contact Information
        public string Principal { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }

        // Additional Information
        public string TypeOfService { get; set; }
        public string OperatingArea { get; set; }
        public string OrganizationForm { get; set; }
        public int ChildrenPerEmployee { get; set; }
        public int PercentageOfLicensedTeachers { get; set; }
        public string Accessibility { get; set; }
        public string OrientationAndProfile { get; set; }

        public string IndoorDescription { get; set; }
        public string OutdoorDescription { get; set; }
        public string FoodAndMealsDescription { get; set; }
        public string GoalsAndVisionDescription { get; set; }

        // Navigation property
        public ICollection<Response>? Responses { get; set; } = new List<Response>();
    }
}
