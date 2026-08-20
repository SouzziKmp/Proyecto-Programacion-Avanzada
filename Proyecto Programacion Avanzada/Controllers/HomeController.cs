using System.Linq;
using System.Web.Mvc;
using Proyecto.Service;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarteleraService _carteleraService;

        public HomeController(ICarteleraService carteleraService)
        {
            _carteleraService = carteleraService;
        }

        public ActionResult Index()
        {
            var proximosConciertos = _carteleraService.ObtenerCartelera()
                .OrderBy(c => c.FechaEvento)
                .Take(3);

            return View(proximosConciertos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}