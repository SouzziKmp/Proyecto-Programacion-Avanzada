using System.Collections.Generic;
using Proyecto.Data;

namespace Proyecto.Repository
{
    // Coordina un unico DbContext por request y evita instanciar un Repository<T> por cada
    // consulta (RNF-05: las compras se procesan dentro de una transaccion/SaveChanges comun).
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<System.Type, object> _repositories = new Dictionary<System.Type, object>();
        private bool _disposed;

        public UnitOfWork() : this(new ApplicationDbContext())
        {
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
                _repositories[type] = new Repository<TEntity>(_context);

            return (IRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges() => _context.SaveChanges();

        public void Dispose()
        {
            if (_disposed) return;
            _context.Dispose();
            _disposed = true;
        }
    }
}
