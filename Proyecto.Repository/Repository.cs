using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Proyecto.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query() => _dbSet;

        public TEntity GetById(object id) => _dbSet.Find(id);

        public IEnumerable<TEntity> GetAll() => _dbSet.ToList();

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate).ToList();

        public void Add(TEntity entity) => _dbSet.Add(entity);

        public void Remove(TEntity entity) => _dbSet.Remove(entity);

        public void Update(TEntity entity) => _context.Entry(entity).State = EntityState.Modified;
    }
}
