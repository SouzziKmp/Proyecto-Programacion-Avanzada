using Proyecto.Repository;
using Proyecto.Service;
using Proyecto.Service.Services;
using System.Web.Http;
using System.Web.Mvc;
using Unity;

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

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver =
                new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}