using System.Collections.Generic;
using Proyecto.Data.Entities;

namespace Proyecto.Service
{
    public interface IOrdenService
    {
        IEnumerable<Orden> ObtenerHistorial(string usuarioId);
        Orden ObtenerDetalle(string usuarioId, int ordenId);
    }
}