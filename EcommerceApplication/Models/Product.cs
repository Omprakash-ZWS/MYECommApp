using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        public string Images { get; set; }
       
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        [Required]
        public float Price { get; set; }
        public int proquantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

       
    }
}
