namespace Identity.API.Services
{
    public abstract class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;


        public GenericService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            var list = _dbSet.ToList();
            if (list == null || !list.Any())
                throw new KeyNotFoundException("No entities found.");
            return list;
        }

        public virtual T GetById(object id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            return entity;
        }

        public virtual T GetByUserId(object id)
        {
            var entity = _dbSet.FirstOrDefault(e => EF.Property<object>(e, "UserId").Equals(id));
            if (entity == null)
                throw new KeyNotFoundException($"Entity with UserId {id} not found.");
            return entity;
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
        public virtual void Delete(object id)
        {
            var entity = GetById(id);
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}