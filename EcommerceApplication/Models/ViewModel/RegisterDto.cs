using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models.ViewModel
{
    public class RegisterDto
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Firstname is Required")]
        public string FirstName { get; set; }
        //[Required(ErrorMessage = "Middlename is Required")]
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Phone no. is Required")]
      
        public string address { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password And Conform should be same")]
        public string ConformPassword { get; set; }
    }
}
