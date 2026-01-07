namespace Catalog.API.Domain.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}