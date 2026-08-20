using Proyecto.Data.Entities;
using Proyecto.Repository;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Proyecto.Service.Services
{
    public class ConciertoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConciertoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public void Agregar(
            Concierto concierto,
            List<ConciertoImagen> imagenes)
        {
            concierto.Codigo = "EVT-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            _unitOfWork.Repository<Concierto>().Add(concierto);
            _unitOfWork.SaveChanges();

            if (imagenes != null && imagenes.Count > 0)
            {
                int orden = 1;

                foreach (var imagen in imagenes)
                {
                    if (orden > 3)
                        break;

                    imagen.ConciertoId = concierto.ConciertoId;
                    imagen.Orden = orden;
                    imagen.EsPrincipal = orden == 1;
                    imagen.FechaCarga = DateTime.Now;

                    _unitOfWork.Repository<ConciertoImagen>().Add(imagen);

                    orden++;
                }

                _unitOfWork.SaveChanges();
            }
        }

        public void Actualizar(
            Concierto concierto,
            List<ConciertoImagen> imagenes)
        {
            _unitOfWork.Repository<Concierto>().Update(concierto);
            _unitOfWork.SaveChanges();

            // Si no se cargaron nuevas imágenes,
            // se conservan las imágenes existentes.
            if (imagenes == null || imagenes.Count == 0)
                return;

            var imagenesActuales = _unitOfWork
                .Repository<ConciertoImagen>()
                .Query()
                .Where(i => i.ConciertoId == concierto.ConciertoId)
                .ToList();

            foreach (var imagenActual in imagenesActuales)
            {
                _unitOfWork.Repository<ConciertoImagen>()
                    .Remove(imagenActual);
            }

            _unitOfWork.SaveChanges();

            int orden = 1;

            foreach (var imagen in imagenes)
            {
                if (orden > 3)
                    break;

                imagen.ConciertoId = concierto.ConciertoId;
                imagen.Orden = orden;
                imagen.EsPrincipal = orden == 1;
                imagen.FechaCarga = DateTime.Now;

                _unitOfWork.Repository<ConciertoImagen>().Add(imagen);

                orden++;
            }



            _unitOfWork.SaveChanges();

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