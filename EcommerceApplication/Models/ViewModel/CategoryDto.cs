using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApplication.Models.ViewModel
{
    public class CategoryDto    
    {
        public int CategoryId { get; set; }
        
        [Required]
        public string CategoryName { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        [Required]
        public string colour { get; set; }
        [Required]
        public string description { get; set; }
	}
  
}
