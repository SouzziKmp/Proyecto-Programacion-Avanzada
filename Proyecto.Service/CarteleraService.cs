using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Proyecto.Data.Entities;
using Proyecto.Repository;

namespace Proyecto.Service
{
    public class CarteleraService : ICarteleraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarteleraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Concierto> ObtenerCartelera()
        {
            var ahora = DateTime.Now;

            return _unitOfWork.Repository<Concierto>()
                .Query()
                .Include(c => c.Lugar)
                .Include(c => c.Categoria)
                .Include(c => c.Imagenes)
                .Include(c => c.TiposEntrada)
                .Where(c => c.Estado == 1 && c.FechaEvento >= ahora)
                .OrderBy(c => c.FechaEvento)
                .ToList();
        }

        public Concierto ObtenerDetalle(int conciertoId)
        {
            return _unitOfWork.Repository<Concierto>()
                .Query()
                .Include(c => c.Lugar)
                .Include(c => c.Categoria)
                .Include(c => c.Imagenes)
                .Include(c => c.TiposEntrada)
                .FirstOrDefault(c => c.ConciertoId == conciertoId && c.Estado == 1);
        }

        public ConciertoImagen ObtenerImagenPrincipal(int conciertoId)
        {
            return _unitOfWork.Repository<ConciertoImagen>()
                .Query()
                .Where(i => i.ConciertoId == conciertoId)
                .OrderByDescending(i => i.EsPrincipal)
                .ThenBy(i => i.Orden)
                .FirstOrDefault();
        }
    }
}