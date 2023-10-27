using EcommerceApplication.Context;
using EcommerceApplication.Models;
using EcommerceApplication.Models.ViewModel;
using EcommerceApplication.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication.Service.Infrastructure
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		public ProductRepository(ECommDbContext context) : base(context)
		{
			
		}

		//public ECommDbContext Context { get; }

		//public IEnumerable<CartDto> IProductRepository.Search(string earchTerm)
		//{
		//	if (string.IsNullOrEmpty(SearchTerm))
		//	{
		//		return _ProductList;
		//	}
		//	return _ProductList.Where(e => e.ProductName.Contains(SearchTerm));
		//}

		//	IEnumerable<CartDto> IProductRepository.Search(string searchTerm)
		//	{
		//		throw new NotImplementedException();
		//	}
		//}
	}
}
