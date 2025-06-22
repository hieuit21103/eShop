using Microsoft.EntityFrameworkCore;
using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Models.DTOs;
using Identity.API.Services.Interfaces;


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
            return _dbSet.ToList() ?? throw new InvalidOperationException("No entities found.");
        }

        public virtual T GetById(object id)
        {
            return _dbSet.Find(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
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