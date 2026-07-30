using System.Collections.Generic;
using Proyecto.Data.Entities;

namespace Proyecto.Service
{
    public interface ICarteleraService
    {
        IEnumerable<Concierto> ObtenerCartelera();
        Concierto ObtenerDetalle(int conciertoId);
        ConciertoImagen ObtenerImagenPrincipal(int conciertoId);
    }
}