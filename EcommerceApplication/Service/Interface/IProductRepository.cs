using EcommerceApplication.Models;
using EcommerceApplication.Models.ViewModel;

namespace EcommerceApplication.Service.Interface
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		//IEnumerable<Product> Search(string SearchTerm);
	}
}
