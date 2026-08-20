using Proyecto.Data.Entities;
using Proyecto.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto.Service.Services
{
    public class LugarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LugarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<Lugar> ObtenerTodos()
        {
            return _unitOfWork
                .Repository<Lugar>()
                .GetAll()
                .OrderBy(l => l.Nombre)
                .ToList();
        }
    }
}