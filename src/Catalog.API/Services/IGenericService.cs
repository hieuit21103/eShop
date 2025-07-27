namespace Catalog.API.Services;

public interface IGenericService<T> where T : class
{
    Task<PagedResult<T>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<T> GetByIdAsync(Guid id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
}