using EcommerceApplication.Context;
using EcommerceApplication.Models;
using EcommerceApplication.Service.Interface;

namespace EcommerceApplication.Service.Infrastructure
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(ECommDbContext context) : base(context)
		{
		}
	}
}
