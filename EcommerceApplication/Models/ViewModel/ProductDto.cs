using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models.ViewModel
{
    public class ProductDto
    {
        //public IEnumerable<CartDto> cartDtos { get; set; }
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        public string Images { get; set; }
        [Required]
        public float Price { get; set; }
        public int proquantity { get; set; }
        public int CategoryId { get; set; }
		public List<Product> products { get; set; }

	}
}
