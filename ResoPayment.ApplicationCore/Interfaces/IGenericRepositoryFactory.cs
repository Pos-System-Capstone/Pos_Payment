namespace ResoPayment.ApplicationCore.Interfaces;

public interface IGenericRepositoryFactory
{
	IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}