namespace EcommerceApplication.Service.Interface
{
	public interface IUnitOfWork : IDisposable
	{		
		ICategoryRepository Category { get; }

		IProductRepository Product { get; }		

		int Save();
	}
}
