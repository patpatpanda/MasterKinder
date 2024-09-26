using Microsoft.AspNetCore.Identity;

namespace MasterKinder.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int SchoolId { get; set; } // Koppla användare till en förskola
    }
}
