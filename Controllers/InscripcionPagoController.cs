using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/InscripcionesPago")]
    public class InscripcionPagoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionPago> Logger;

        public InscripcionPagoController(KalumDbContext _DbContext, ILogger<InscripcionPago> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionPago>>> Get()
        {
            List<InscripcionPago> inscripcionPago = null;
            Logger.LogDebug("Iniciando el proceso de consulta de las inscripciones de pago en la BD");
            inscripcionPago = await DbContext.InscripcionPago.Include(a => a.Aspirante).AsSplitQuery().ToListAsync();
            if (inscripcionPago == null || inscripcionPago.Count == 0)
            {
                Logger.LogWarning("No existen inscripciones de pago");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(inscripcionPago);
        }
        [HttpGet("{id}", Name = "GetInscripcionPago")]

        public async Task<ActionResult<InscripcionPago>> GetInscripcionPago(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de la inscripcion de pago con id " + id);
            var inscripcionPago = await DbContext.InscripcionPago.AsSplitQuery().FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if (inscripcionPago == null)
            {
                Logger.LogWarning("No existe la inscripcion de pago con id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del id de forma exitosa!");
            return Ok(inscripcionPago);
        }
        [HttpPost]
        public async Task<ActionResult<InscripcionPago>> Post([FromBody] InscripcionPago value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una inscripcion de pago");
            value.BoletaPago = Guid.NewGuid().ToString().ToUpper();
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if (aspirante == null)
            {
                Logger.LogInformation("No existe el aspirante con no. de expediente " + value.NoExpediente);
                return BadRequest();
            }
            await DbContext.InscripcionPago.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar una inscripcion de pago");
            return new CreatedAtRouteResult("GetInscripcionPago", new { id = value.BoletaPago }, value);
        }

        [HttpDelete("{id}", Name = "DeleteInscripcionPago")]

        public async Task<ActionResult<InscripcionPago>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar una inscripcion de pago con id " + id);
            InscripcionPago inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if (inscripcionPago == null)
            {
                Logger.LogWarning("No se encontro la inscripcion de pago");
                return NotFound();
            }
            else
            {
                DbContext.InscripcionPago.Remove(inscripcionPago);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado la inscripcion de pago con id " + id);
                return inscripcionPago;
            }
        }

        [HttpPut("{id}", Name = "UpdateInscripcionPago")]

        public async Task<ActionResult<InscripcionPago>> Put(string id, [FromBody] InscripcionPago value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización de la inscripcion de pago con id " + id);
            InscripcionPago inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if (inscripcionPago == null)
            {
                Logger.LogWarning("No se encontro la inscripcion de pago");
                return BadRequest();
            }
            inscripcionPago.FechaPago = value.FechaPago;
            inscripcionPago.Monto = value.Monto;
            DbContext.Entry(inscripcionPago).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado la inscripcion de pago");
            return NoContent();
        }
    }
}