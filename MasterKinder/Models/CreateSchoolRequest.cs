namespace MasterKinder.Models
{
    public class CreateSchoolRequest
    {
         public string SchoolName { get; set; }
        public int TotalResponses { get; set; }
        public double ResponseRatePercentage { get; set; } // Ändrad här
        public int NumberOfChildren { get; set; } // Lagt till
        public string Address { get; set; } // Lagt till
        public string Description { get; set; } // Lagt till

        public ContactInfo Contact { get; set; } // Lagt till

        public string TypeOfService { get; set; } // Lagt till
        public string OperatingArea { get; set; } // Lagt till
        public string OrganizationForm { get; set; } // Lagt till
        public int ChildrenPerEmployee { get; set; } // Lagt till
        public int PercentageOfLicensedTeachers { get; set; } // Lagt till
        public string Accessibility { get; set; } // Lagt till
        public string OrientationAndProfile { get; set; } // Lagt till
        public string IndoorDescription { get; set; } // Lagt till
        public string OutdoorDescription { get; set; } // Lagt till
        public string FoodAndMealsDescription { get; set; } // Lagt till
        public string GoalsAndVisionDescription { get; set; } // Lagt till

        public List<CreateResponseRequest> Responses { get; set; }
    }
    public class ContactInfo
    {
        public string Principal { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}
