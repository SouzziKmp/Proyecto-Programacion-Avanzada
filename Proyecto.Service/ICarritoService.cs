using Proyecto.Data.Entities;

namespace Proyecto.Service
{
    public interface ICarritoService
    {
        Carrito ObtenerCarritoActivo(string usuarioId);
        void AgregarEntrada(string usuarioId, int tipoEntradaId, int cantidad);
        void ActualizarCantidad(string usuarioId, int carritoDetalleId, int cantidad);
        void EliminarDetalle(string usuarioId, int carritoDetalleId);
    }
}
