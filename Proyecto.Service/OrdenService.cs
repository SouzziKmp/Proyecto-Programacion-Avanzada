using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Proyecto.Data.Entities;
using Proyecto.Repository;

namespace Proyecto.Service
{
    public class OrdenService : IOrdenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Orden> ObtenerHistorial(string usuarioId)
        {
            return _unitOfWork.Repository<Orden>()
                .Query()
                .Include(o => o.Detalles.Select(d => d.TipoEntrada.Concierto))
                .Where(o => o.UsuarioId == usuarioId)
                .OrderByDescending(o => o.FechaOrden)
                .ToList();
        }

        public Orden ObtenerDetalle(string usuarioId, int ordenId)
        {
            return _unitOfWork.Repository<Orden>()
                .Query()
                .Include(o => o.Detalles.Select(d => d.TipoEntrada.Concierto))
                .Include(o => o.Detalles.Select(d => d.Tickets))
                .FirstOrDefault(o => o.UsuarioId == usuarioId && o.OrdenId == ordenId);
        }
    }
}
