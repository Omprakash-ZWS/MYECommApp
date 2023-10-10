using Microsoft.AspNetCore.Identity;

namespace EcommerceApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional profile data for application users by adding properties to this class       
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public int MobileNo { get; set; }
        public string Address { get; set; }
      
    }
}
