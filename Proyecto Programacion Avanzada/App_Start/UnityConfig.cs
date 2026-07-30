using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
<<<<<<< HEAD
=======
using Proyecto.Repository;
using Proyecto.Service;
>>>>>>> e2f3b01558ed925e2221b1ceb0a64ba3b01104e0

namespace Proyecto_Programacion_Avanzada
{
    public static class UnityConfig
    {
<<<<<<< HEAD
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
=======
        public static void RegisterComponents() 

        {
			var container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<ICarteleraService, CarteleraService>();
            container.RegisterType<ICarritoService, CarritoService>();
            container.RegisterType<ICompraService, CompraService>();
            container.RegisterType<IOrdenService, OrdenService>();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

>>>>>>> e2f3b01558ed925e2221b1ceb0a64ba3b01104e0
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}