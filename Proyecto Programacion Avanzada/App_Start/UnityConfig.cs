using Proyecto.Repository;
using Proyecto.Service;
using Proyecto.Service.Services;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Proyecto_Programacion_Avanzada
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<ICarteleraService, CarteleraService>();
            container.RegisterType<ICarritoService, CarritoService>();
            container.RegisterType<ICompraService, CompraService>();
            container.RegisterType<IOrdenService, OrdenService>();

            container.RegisterType<ConciertoService>();
            container.RegisterType<CategoriaService>();
            container.RegisterType<LugarService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver =
                new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}
