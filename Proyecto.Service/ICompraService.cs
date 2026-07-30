namespace Proyecto.Service
{
    public interface ICompraService
    {
        CompraResultado ConfirmarCompra(string usuarioId, string metodoPago);
    }
}