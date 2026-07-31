using Proyecto.Data.Entities;
using Proyecto.Repository;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    public class ImagenController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Concierto(int id)
        {
            var imagen = _unitOfWork
                .Repository<ConciertoImagen>()
                .Query()
                .Where(i => i.ConciertoId == id)
                .OrderBy(i => i.Orden)
                .FirstOrDefault();

            if (imagen == null || imagen.Contenido == null)
            {
                return HttpNotFound();
            }

            return File(imagen.Contenido, imagen.TipoContenido);
        }
    }
}