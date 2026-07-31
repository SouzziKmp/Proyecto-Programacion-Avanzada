using Proyecto.Data.Entities;
using Proyecto.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto.Service.Services
{
    public class ConciertoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConciertoService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<Concierto> ObtenerTodos()
        {
            return _unitOfWork.Repository<Concierto>()
                              .GetAll()
                              .ToList();
        }

        public Concierto ObtenerPorId(int id)
        {
            return _unitOfWork.Repository<Concierto>()
                              .GetById(id);
        }

        public void Agregar(Concierto concierto, byte[] contenido, string nombreArchivo, string tipoContenido)
        {
            _unitOfWork.Repository<Concierto>().Add(concierto);
            _unitOfWork.SaveChanges();

            if (contenido != null && contenido.Length > 0)
            {
                ConciertoImagen imagen = new ConciertoImagen
                {
                    ConciertoId = concierto.ConciertoId,
                    NombreArchivo = nombreArchivo,
                    TipoContenido = tipoContenido,
                    Contenido = contenido,
                    EsPrincipal = true,
                    Orden = 1,
                    FechaCarga = System.DateTime.Now
                };

                _unitOfWork.Repository<ConciertoImagen>().Add(imagen);
                _unitOfWork.SaveChanges();
            }
        }

        public void Actualizar(
            Concierto concierto,
            byte[] contenido,
            string nombreArchivo,
            string tipoContenido)
        {
            _unitOfWork.Repository<Concierto>().Update(concierto);
            _unitOfWork.SaveChanges();

            if (contenido != null && contenido.Length > 0)
            {
                var imagen = _unitOfWork
                    .Repository<ConciertoImagen>()
                    .Query()
                    .FirstOrDefault(i => i.ConciertoId == concierto.ConciertoId);

                if (imagen == null)
                {
                    imagen = new ConciertoImagen
                    {
                        ConciertoId = concierto.ConciertoId,
                        FechaCarga = System.DateTime.Now,
                        EsPrincipal = true,
                        Orden = 1
                    };

                    _unitOfWork.Repository<ConciertoImagen>().Add(imagen);
                }

                imagen.NombreArchivo = nombreArchivo;
                imagen.TipoContenido = tipoContenido;
                imagen.Contenido = contenido;

                _unitOfWork.SaveChanges();
            }
        }

        public void Eliminar(int id)
        {
            var concierto = ObtenerPorId(id);

            if (concierto != null)
            {
                _unitOfWork.Repository<Concierto>().Remove(concierto);
                _unitOfWork.SaveChanges();
            }
        }
    }
}