using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.Models.ViewModel
{
	public class CartDto
	{
		[Key]
		public int ProductId { get; set; }

		public string ProductName { get; set; }		
        public int Total { get; set; }
        public float Price { get; set; }
		public string Images { get; set; }
		public int proquantity { get; set; }
		public string SearchTerm { get; set; }
		public int CategoryId { get; set; }
		public int finaltotal { get; set; }
		//public Category category { get; set; }
		//public List<Category> categories { get; set; }
	}
}
