using System.Web.Mvc;
using Proyecto.Service;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    public class CarteleraController : Controller
    {
        private readonly ICarteleraService _carteleraService;

        public CarteleraController(ICarteleraService carteleraService)
        {
            _carteleraService = carteleraService;
        }

        public ActionResult Index()
        {
            var conciertos = _carteleraService.ObtenerCartelera();
            return View(conciertos);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var concierto = _carteleraService.ObtenerDetalle(id.Value);

            if (concierto == null)
            {
                return HttpNotFound();
            }

            return View(concierto);
        }

        public ActionResult Imagen(int id)
        {
            var imagen = _carteleraService.ObtenerImagenPrincipal(id);

            if (imagen == null)
            {
                return HttpNotFound();
            }

            return File(imagen.Contenido, imagen.TipoContenido);
        }
    }
}