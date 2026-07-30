using System;
using System.Data.Entity;
using System.Linq;
using Proyecto.Data.Entities;
using Proyecto.Repository;

namespace Proyecto.Service
{
    public class CarritoService : ICarritoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarritoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Carrito ObtenerCarritoActivo(string usuarioId)
        {
            return _unitOfWork.Repository<Carrito>()
                .Query()
                .Include(c => c.Detalles.Select(d => d.TipoEntrada.Concierto))
                .FirstOrDefault(c => c.UsuarioId == usuarioId && c.Estado == 1);
        }

        public void AgregarEntrada(string usuarioId, int tipoEntradaId, int cantidad)
        {
            if (cantidad <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            var tipoEntrada = _unitOfWork.Repository<TipoEntrada>()
                .Query()
                .Include(t => t.Concierto)
                .FirstOrDefault(t => t.TipoEntradaId == tipoEntradaId);

            if (tipoEntrada == null || tipoEntrada.Concierto.Estado != 1)
                throw new InvalidOperationException("La entrada seleccionada no existe.");

            var carrito = ObtenerCarritoActivo(usuarioId);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UsuarioId = usuarioId,
                    FechaCreacion = DateTime.Now,
                    Estado = 1
                };

                _unitOfWork.Repository<Carrito>().Add(carrito);
                _unitOfWork.SaveChanges();
            }

            var detalle = carrito.Detalles.FirstOrDefault(d => d.TipoEntradaId == tipoEntradaId);

            if (detalle == null)
            {
                detalle = new CarritoDetalle
                {
                    CarritoId = carrito.CarritoId,
                    TipoEntradaId = tipoEntradaId,
                    Cantidad = cantidad,
                    PrecioUnitario = tipoEntrada.Precio
                };

                _unitOfWork.Repository<CarritoDetalle>().Add(detalle);
            }
            else
            {
                detalle.Cantidad += cantidad;
                _unitOfWork.Repository<CarritoDetalle>().Update(detalle);
            }

            _unitOfWork.SaveChanges();
        }

        public void ActualizarCantidad(string usuarioId, int carritoDetalleId, int cantidad)
        {
            if (cantidad <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            var carrito = ObtenerCarritoActivo(usuarioId);

            if (carrito == null)
                throw new InvalidOperationException("No hay carrito activo.");

            var detalle = carrito.Detalles.FirstOrDefault(d => d.CarritoDetalleId == carritoDetalleId);

            if (detalle == null)
                throw new InvalidOperationException("El detalle no existe.");

            detalle.Cantidad = cantidad;

            _unitOfWork.Repository<CarritoDetalle>().Update(detalle);
            _unitOfWork.SaveChanges();
        }

        public void EliminarDetalle(string usuarioId, int carritoDetalleId)
        {
            var carrito = ObtenerCarritoActivo(usuarioId);

            if (carrito == null)
                return;

            var detalle = carrito.Detalles.FirstOrDefault(d => d.CarritoDetalleId == carritoDetalleId);

            if (detalle == null)
                return;

            _unitOfWork.Repository<CarritoDetalle>().Remove(detalle);
            _unitOfWork.SaveChanges();
        }
    }
}
