using System;
<<<<<<< HEAD
=======
using System.Data.Entity;
>>>>>>> e2f3b01558ed925e2221b1ceb0a64ba3b01104e0

namespace Proyecto.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
<<<<<<< HEAD
        int SaveChanges();
    }
}
=======
        DbContextTransaction BeginTransaction();
        int SaveChanges();
    }
}
>>>>>>> e2f3b01558ed925e2221b1ceb0a64ba3b01104e0
