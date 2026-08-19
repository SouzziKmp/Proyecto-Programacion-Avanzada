using Proyecto.Repository;
using Proyecto.Service;
using Proyecto.Service.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Proyecto.Service.Services;

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

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}