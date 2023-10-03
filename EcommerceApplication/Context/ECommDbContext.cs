using EcommerceApplication.Models;
using Microsoft.EntityFrameworkCore;
using EcommerceApplication.Models.ViewModel;

namespace EcommerceApplication.Context
{
	public class ECommDbContext : DbContext
	{
		public ECommDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Category> categories { get; set; }	
		public DbSet<Product> products { get; set; }	
	}
}
