using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Programacion_Avanzada.Filters
{
    // Filtro global de excepciones para MVC (Unidad 6): registra el error en
    // App_Data/errores.log y evita mostrarle al usuario detalles internos
    // (stack trace) o dejar una solicitud AJAX colgada sin respuesta JSON.
    public class ManejadorExcepcionesAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            RegistrarError(filterContext.Exception);

            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { mensaje = "Ocurrió un error inesperado. Intente de nuevo." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new ViewResult { ViewName = "Error" };
            }

            filterContext.ExceptionHandled = true;
        }

        private static void RegistrarError(Exception ex)
        {
            try
            {
                var ruta = HttpContext.Current.Server.MapPath("~/App_Data/errores.log");
                var carpeta = Path.GetDirectoryName(ruta);

                if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var linea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | MVC | {ex.GetType().Name} | {ex.Message}{Environment.NewLine}";

                if (ex is DbEntityValidationException validationEx)
                {
                    foreach (var error in validationEx.EntityValidationErrors.SelectMany(r => r.ValidationErrors))
                    {
                        linea += $"    - {error.PropertyName}: {error.ErrorMessage}{Environment.NewLine}";
                    }
                }

                File.AppendAllText(ruta, linea);
            }
            catch
            {
                // Si no se puede escribir el log, no debe tumbar la aplicación.
            }
        }
    }
}
