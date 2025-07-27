namespace Catalog.API.Services;

public class GenericService<T>(ApplicationDbContext context) : IGenericService<T> where T : class
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<PagedResult<T>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _dbSet.AsQueryable();
        var totalCount = await query.CountAsync();

        var result = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Items = result,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        context.SaveChanges();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return false;

        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
