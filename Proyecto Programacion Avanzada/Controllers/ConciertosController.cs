using Proyecto.Data.Entities;
using Proyecto.Service.Services;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    public class ConciertosController : Controller
    {
        private readonly ConciertoService _service;
        private readonly CategoriaService _categoriaService;
        private readonly LugarService _lugarService;

        public ConciertosController(ConciertoService service,
                                    CategoriaService categoriaService,
                                    LugarService lugarService)
        {
                                    _service = service;
                                    _categoriaService = categoriaService;
                                    _lugarService = lugarService;
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
            IEnumerable<HttpPostedFileBase> archivos)
        {
            // El codigo lo genera el servicio, no viene en el formulario.
            ModelState.Remove(nameof(Concierto.Codigo));

            if (ModelState.IsValid)
            {
                var listaImagenes = new List<ConciertoImagen>();

                if (archivos != null)
                {
                    foreach (var imagen in archivos)
                    {
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            using (var br = new BinaryReader(imagen.InputStream))
                            {
                                listaImagenes.Add(new ConciertoImagen
                                {
                                    NombreArchivo = Path.GetFileName(imagen.FileName),
                                    TipoContenido = imagen.ContentType,
                                    Contenido = br.ReadBytes(imagen.ContentLength)
                                });
                            }
                        }
                    }
                }

                if (listaImagenes.Count > 3)
                {
                    ModelState.AddModelError(
                        "",
                        "Solo se permiten un máximo de 3 imágenes."
                    );
                }
                else
                {
                    _service.Agregar(concierto, listaImagenes);

                    return RedirectToAction("Index");
                }



            



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
            IEnumerable<HttpPostedFileBase> archivos)
        {
            if (ModelState.IsValid)
            {
                var listaImagenes = new List<ConciertoImagen>();

                if (archivos != null)
                {
                    foreach (var imagen in archivos)
                    {
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            using (var br = new BinaryReader(imagen.InputStream))
                            {
                                listaImagenes.Add(new ConciertoImagen
                                {
                                    NombreArchivo = Path.GetFileName(imagen.FileName),
                                    TipoContenido = imagen.ContentType,
                                    Contenido = br.ReadBytes(imagen.ContentLength)
                                });
                            }
                        }
                    }
                }

                if (listaImagenes.Count > 3)
                {
                    ModelState.AddModelError(
                        "",
                        "Solo se permiten un máximo de 3 imágenes."
                    );
                }
                else
                {
                    _service.Actualizar(concierto, listaImagenes);

                    return RedirectToAction("Index");
                }




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