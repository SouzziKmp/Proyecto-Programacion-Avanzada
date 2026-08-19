using Proyecto.Data.Entities;
using Proyecto.Repository;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    public class ImagenController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImagenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Concierto(int id, int orden = 1)
        {
            var imagen = _unitOfWork
                .Repository<ConciertoImagen>()
                .Query()
                .Where(i => i.ConciertoId == id && i.Orden == orden)
                .FirstOrDefault();

            if (imagen == null || imagen.Contenido == null)
            {
                return HttpNotFound();
            }

            return File(imagen.Contenido, imagen.TipoContenido);
        }
    }
}