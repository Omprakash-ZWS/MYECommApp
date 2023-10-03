namespace EcommerceApplication.Service.Interface
{
	public interface IGenericRepository<T> where T : class
	{
		IEnumerable<T> GetAll();
		T GetById(int id);
		void Update(T entity);
		void Delete(T entity);
		void Add(T entity);

		void Save();
	}
}
