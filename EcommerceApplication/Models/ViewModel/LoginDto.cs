using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models.ViewModel
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
    }
}
