using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models.ViewModel
{
    public class EditRoleDto
    {
        public EditRoleDto()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        [Required(ErrorMessage="Role Name is Required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
