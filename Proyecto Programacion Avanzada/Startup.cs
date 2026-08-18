using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(
    typeof(Proyecto_Programacion_Avanzada.Startup)
)]

namespace Proyecto_Programacion_Avanzada
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}