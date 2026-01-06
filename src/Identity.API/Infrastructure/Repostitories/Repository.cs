using Ardalis.Specification.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Repositories;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(ApplicationDbContext context) : base(context)
    {
    }
}