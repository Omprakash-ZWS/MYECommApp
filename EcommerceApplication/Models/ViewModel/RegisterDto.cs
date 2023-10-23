using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models.ViewModel
{
    public class RegisterDto
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Firstname is Required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        //[Required(ErrorMessage = "Middlename is Required")]
        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        [DisplayName("Address")]
        public string address { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password And Conform should be same")]
        [DisplayName("Conform Password")]
        public string ConformPassword { get; set; }
        [DisplayName("User Role")]
        public string UserRole { get; set; }
    }
}
