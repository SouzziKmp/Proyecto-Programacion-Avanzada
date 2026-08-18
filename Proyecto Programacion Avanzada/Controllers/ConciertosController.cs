using Proyecto.Data.Entities;
using Proyecto.Service.Services;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    public class ConciertosController : Controller
    {
        private readonly ConciertoService _service;
        private readonly CategoriaService _categoriaService;
        private readonly LugarService _lugarService;

        public ConciertosController()
        {
            _service = new ConciertoService();
            _categoriaService = new CategoriaService();
            _lugarService = new LugarService();
        }


        // INDEX


        [AllowAnonymous]
        public ActionResult Index()
        {
            var lista = _service.ObtenerTodos();
            return View(lista);
        }


        // DETAILS

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var concierto = _service.ObtenerPorId(id);

            if (concierto == null)
                return HttpNotFound();

            return View(concierto);
        }


        // CREATE GET


        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Categorias = new SelectList(
                _categoriaService.ObtenerTodas(),
                "CategoriaId",
                "Nombre"
            );

            ViewBag.Lugares = new SelectList(
                _lugarService.ObtenerTodos(),
                "LugarId",
                "Nombre"
            );

            return View();
        }


        // CREATE POST


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create(
            Concierto concierto,
            HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                byte[] contenido = null;
                string nombreArchivo = null;
                string tipoContenido = null;

                if (imagen != null &&
                    imagen.ContentLength > 0)
                {
                    using (var br =
                        new System.IO.BinaryReader(
                            imagen.InputStream))
                    {
                        contenido =
                            br.ReadBytes(
                                imagen.ContentLength
                            );
                    }

                    nombreArchivo =
                        System.IO.Path.GetFileName(
                            imagen.FileName
                        );

                    tipoContenido =
                        imagen.ContentType;
                }

                _service.Agregar(
                    concierto,
                    contenido,
                    nombreArchivo,
                    tipoContenido
                );

                return RedirectToAction("Index");
            }

            ViewBag.Categorias = new SelectList(
                _categoriaService.ObtenerTodas(),
                "CategoriaId",
                "Nombre",
                concierto.CategoriaId
            );

            ViewBag.Lugares = new SelectList(
                _lugarService.ObtenerTodos(),
                "LugarId",
                "Nombre",
                concierto.LugarId
            );

            return View(concierto);
        }

        // EDIT GET


        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            var concierto = _service.ObtenerPorId(id);

            if (concierto == null)
                return HttpNotFound();

            ViewBag.Categorias = new SelectList(
                _categoriaService.ObtenerTodas(),
                "CategoriaId",
                "Nombre",
                concierto.CategoriaId
            );

            ViewBag.Lugares = new SelectList(
                _lugarService.ObtenerTodos(),
                "LugarId",
                "Nombre",
                concierto.LugarId
            );

            return View(concierto);
        }


        // EDIT POST


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(
    Concierto concierto,
    HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                byte[] contenido = null;
                string nombreArchivo = null;
                string tipoContenido = null;

                if (imagen != null &&
                    imagen.ContentLength > 0)
                {
                    using (var br =
                        new System.IO.BinaryReader(
                            imagen.InputStream))
                    {
                        contenido =
                            br.ReadBytes(
                                imagen.ContentLength
                            );
                    }

                    nombreArchivo =
                        System.IO.Path.GetFileName(
                            imagen.FileName
                        );

                    tipoContenido =
                        imagen.ContentType;
                }

                _service.Actualizar(
                    concierto,
                    contenido,
                    nombreArchivo,
                    tipoContenido
                );

                return RedirectToAction("Index");
            }

            ViewBag.Categorias = new SelectList(
                _categoriaService.ObtenerTodas(),
                "CategoriaId",
                "Nombre",
                concierto.CategoriaId
            );

            ViewBag.Lugares = new SelectList(
                _lugarService.ObtenerTodos(),
                "LugarId",
                "Nombre",
                concierto.LugarId
            );

            return View(concierto);
        }


        // DELETE GET


        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            var concierto = _service.ObtenerPorId(id);

            if (concierto == null)
                return HttpNotFound();

            return View(concierto);
        }


        // DELETE POST


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            _service.Eliminar(id);

            return RedirectToAction("Index");
        }
    }
}