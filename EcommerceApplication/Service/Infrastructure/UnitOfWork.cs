using EcommerceApplication.Context;
using EcommerceApplication.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace EcommerceApplication.Service.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ECommDbContext _context;

		public UnitOfWork(ECommDbContext context)
		{
			_context = context;			
			Product = new ProductRepository(_context);			
			Category = new CategoryRepository(_context);
		}		
		public ICategoryRepository Category { get; private set; }
		public IProductRepository Product { get; private set; }

		public int Save()
		{
			return _context.SaveChanges();
		}
		public void Dispose()
		{
			_context.Dispose();
		}

	}
}


