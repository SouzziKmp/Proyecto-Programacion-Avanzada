using System;

namespace Proyecto.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
