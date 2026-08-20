using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Proyecto.Service;
using Proyecto_Programacion_Avanzada.Models.Api;

namespace Proyecto_Programacion_Avanzada.Controllers.Api
{
    [Authorize(Roles = "Asociado")]
    [RoutePrefix("api/entradas")]
    public class EntradasApiController : ApiController
    {
        private readonly ICarritoService _carritoService;
        private readonly ICompraService _compraService;

        public EntradasApiController(ICarritoService carritoService, ICompraService compraService)
        {
            _carritoService = carritoService;
            _compraService = compraService;
        }

        private string ObtenerUsuarioId() => User.Identity.GetUserId();

        [HttpPost]
        [Route("agregar")]
        public IHttpActionResult Agregar([FromBody] AgregarCarritoDto dto)
        {
            try
            {
                _carritoService.AgregarEntrada(ObtenerUsuarioId(), dto.TipoEntradaId, dto.Cantidad);
                return Ok(new { mensaje = "Entrada agregada al carrito." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("comprar")]
        public IHttpActionResult Comprar()
        {
            var resultado = _compraService.ConfirmarCompra(ObtenerUsuarioId(), "Tarjeta");

            if (!resultado.Exitoso)
                return BadRequest(resultado.Mensaje);

            return Ok(new { resultado.Mensaje, resultado.OrdenId });
        }
    }
}