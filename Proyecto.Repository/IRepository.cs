using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Proyecto.Repository
{
    // Patron Repository generico (RNF-11 / Requerimiento 1: la presentacion no accede
    // directamente al DbContext). Las capas Service consumen esta interfaz, nunca EF6 directo.
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}
