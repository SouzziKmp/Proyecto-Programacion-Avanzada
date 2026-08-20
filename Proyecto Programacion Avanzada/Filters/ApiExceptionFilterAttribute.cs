using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Proyecto_Programacion_Avanzada.Filters
{
    // Filtro global de excepciones para la Web API (Unidad 6): asegura que cualquier
    // error no controlado devuelva JSON consistente en vez de HTML/stack trace, y
    // deja registro en App_Data/errores.log.
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            RegistrarError(context.Exception);

            context.Response = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                new { mensaje = "Ocurrió un error inesperado al procesar la solicitud." });
        }

        private static void RegistrarError(Exception ex)
        {
            try
            {
                var ruta = HttpContext.Current.Server.MapPath("~/App_Data/errores.log");
                var carpeta = Path.GetDirectoryName(ruta);

                if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var linea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | API | {ex.GetType().Name} | {ex.Message}{Environment.NewLine}";

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
