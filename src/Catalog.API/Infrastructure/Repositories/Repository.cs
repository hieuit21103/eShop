namespace Catalog.API.Infrastructure.Repositories;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
