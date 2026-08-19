using Proyecto.Data.Entities;
using Proyecto.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto.Service.Services
{
    public class CategoriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<Categoria> ObtenerTodas()
        {
            return _unitOfWork
                .Repository<Categoria>()
                .GetAll()
                .OrderBy(c => c.Nombre)
                .ToList();
        }
    }
}