using System;
using System.Data.Entity;
using System.Linq;
using Proyecto.Data.Entities;
using Proyecto.Repository;

namespace Proyecto.Service
{
    public class CompraService : ICompraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CompraResultado ConfirmarCompra(string usuarioId, string metodoPago)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var carrito = _unitOfWork.Repository<Carrito>()
                        .Query()
                        .Include(c => c.Detalles.Select(d => d.TipoEntrada))
                        .FirstOrDefault(c => c.UsuarioId == usuarioId && c.Estado == 1);

                    if (carrito == null || !carrito.Detalles.Any())
                        return CompraResultado.Error("El carrito está vacío.");

                    foreach (var detalle in carrito.Detalles)
                    {
                        if (detalle.TipoEntrada.Disponibles < detalle.Cantidad)
                        {
                            return CompraResultado.Error(
                                $"No hay suficientes entradas disponibles para {detalle.TipoEntrada.Nombre}."
                            );
                        }
                    }

                    var total = carrito.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

                    var orden = new Orden
                    {
                        UsuarioId = usuarioId,
                        FechaOrden = DateTime.Now,
                        Total = total,
                        Estado = 2
                    };

                    _unitOfWork.Repository<Orden>().Add(orden);
                    _unitOfWork.SaveChanges();

                    foreach (var item in carrito.Detalles)
                    {
                        item.TipoEntrada.Disponibles -= item.Cantidad;
                        _unitOfWork.Repository<TipoEntrada>().Update(item.TipoEntrada);

                        var ordenDetalle = new OrdenDetalle
                        {
                            OrdenId = orden.OrdenId,
                            TipoEntradaId = item.TipoEntradaId,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = item.PrecioUnitario
                        };

                        _unitOfWork.Repository<OrdenDetalle>().Add(ordenDetalle);
                        _unitOfWork.SaveChanges();

                        for (int i = 0; i < item.Cantidad; i++)
                        {
                            _unitOfWork.Repository<Ticket>().Add(new Ticket
                            {
                                OrdenDetalleId = ordenDetalle.OrdenDetalleId,
                                CodigoUnico = Guid.NewGuid(),
                                Estado = 1,
                                FechaEmision = DateTime.Now
                            });
                        }
                    }

                    _unitOfWork.Repository<Pago>().Add(new Pago
                    {
                        OrdenId = orden.OrdenId,
                        Metodo = metodoPago,
                        Monto = total,
                        Estado = 1,
                        FechaPago = DateTime.Now
                    });

                    carrito.Estado = 2;
                    _unitOfWork.Repository<Carrito>().Update(carrito);

                    _unitOfWork.SaveChanges();
                    transaction.Commit();

                    return CompraResultado.Ok(orden.OrdenId);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return CompraResultado.Error("No se pudo procesar la compra: " + ex.Message);
                }
            }
        }
    }
}
