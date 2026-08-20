using System.Web;
using System.Web.Mvc;
using Proyecto_Programacion_Avanzada.Filters;

namespace Proyecto_Programacion_Avanzada
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ManejadorExcepcionesAttribute());
        }
    }
}
