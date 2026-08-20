using System.Web.Mvc;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Moderacion()
        {
            return View();
        }
    }
}