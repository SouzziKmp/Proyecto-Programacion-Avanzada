using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Proyecto.Data.Entities;
using Proyecto.Repository;
using Proyecto_Programacion_Avanzada.Models.Api;

namespace Proyecto_Programacion_Avanzada.Controllers.Api
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/resenas")]
    public class ResenasApiController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResenasApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("pendientes")]
        public IHttpActionResult Pendientes()
        {
            var resenas = _unitOfWork.Repository<Resena>()
                .Query()
                .Where(r => r.Estado == 0)
                .OrderBy(r => r.FechaCreacion)
                .Select(r => new
                {
                    r.ResenaId,
                    r.Comentario,
                    r.Calificacion,
                    r.FechaCreacion,
                    Concierto = r.Concierto.Titulo,
                    Usuario = r.Usuario.Nombre + " " + r.Usuario.Apellidos
                })
                .ToList();

            return Ok(resenas);
        }

        [HttpPut]
        [Route("{id:int}/moderar")]
        public IHttpActionResult Moderar(int id, [FromBody] ResenaModeracionDto dto)
        {
            var repo = _unitOfWork.Repository<Resena>();
            var resena = repo.GetById(id);

            if (resena == null)
                return NotFound();

            resena.Estado = (byte)(dto.Aprobar ? 1 : 2);
            resena.FechaModeracion = DateTime.Now;
            resena.ModeradoPorId = User.Identity.GetUserId();

            repo.Update(resena);
            _unitOfWork.SaveChanges();

            return Ok(new { resena.ResenaId, resena.Estado });
        }
    }
}