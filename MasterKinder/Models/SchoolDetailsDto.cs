namespace MasterKinder.Models
{
    public class SchoolDetailsDto
    {
        public int SchoolId { get; set; } // Add this line
        public string SchoolName { get; set; }
        public int TotalResponses { get; set; }
        public double Helhetsomdome { get; set; }
        public double Svarsfrekvens { get; set; }
        public int AntalBarn { get; set; }
        public string Accessibility { get; set; }
        public string Address { get; set; }
        public int ChildrenPerEmployee { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string FoodAndMealsDescription { get; set; }
        public string GoalsAndVisionDescription { get; set; }
        public string IndoorDescription { get; set; }
        public string OperatingArea { get; set; }
        public string OrganizationForm { get; set; }
        public string OrientationAndProfile { get; set; }
        public string OutdoorDescription { get; set; }
        public double PercentageOfLicensedTeachers { get; set; }
        public string Phone { get; set; }
        public string Principal { get; set; }
        public string TypeOfService { get; set; }
        public string Website { get; set; }
    }
}
