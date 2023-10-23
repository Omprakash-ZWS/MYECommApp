using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models.ViewModel
{
	public class CreateRoleDto
	{
        [Required]
        public String RoleName { get; set; }
    }
}
