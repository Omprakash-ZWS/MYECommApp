using EcommerceApplication.Context;
using EcommerceApplication.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication.Service.Infrastructure
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ECommDbContext _context;		
		private readonly DbSet<T> _dbSet;
		public GenericRepository(ECommDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}
		public void Add(T entity)
		{
			_dbSet.Add(entity);
		}

		public void Delete(T entity)
		{
			this._dbSet.Remove(entity);
		}



		public IEnumerable<T> GetAll()
		{
			return this._dbSet.ToList();
		}

		public T GetById(int id)
		{
			return this._dbSet.Find(id);
		}

		public void Save()
		{
			_context.SaveChangesAsync();
		}	

		public void Update(T entity)
		{
			_dbSet.Entry(entity).State = EntityState.Modified;
		}

	}
}