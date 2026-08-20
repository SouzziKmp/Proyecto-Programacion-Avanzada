using System;
using System.Data.Entity;

namespace Proyecto.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        DbContextTransaction BeginTransaction();
        int SaveChanges();
    }
}
