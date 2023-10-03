using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApplication.Models
{
	public class Category
	{
        
		
		public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "CategoryName is required")]
        public string CategoryName { get; set; }
       
        public DateTime? CreatedAt { get; set; } 
       
        public DateTime? UpdatedAt { get; set; } 

        public DateTime? DeletedAt { get; set; }
        [Required]
        public string colour { get; set; }

        [Required(ErrorMessage = "Description is requiredd")]
		public string description { get; set; }
	}
}
