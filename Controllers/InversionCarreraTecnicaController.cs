using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/InversionesCarrerasTecnicas")]
    public class InversionCarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InversionCarreraTecnicaController> Logger;

        public InversionCarreraTecnicaController(KalumDbContext _DbContext, ILogger<InversionCarreraTecnicaController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InversionCarreraTecnica>>> Get()
        {
            List<InversionCarreraTecnica> ict = null;
            Logger.LogDebug("Iniciando el proceso de consulta de las inversiones en la BD");
            ict = await DbContext.InversionCarreraTecnica.AsSplitQuery().ToListAsync();
            if (ict == null || ict.Count == 0)
            {
                Logger.LogWarning("No existen inversiones");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(ict);
        }

        [HttpGet("{id}", Name = "GetInversionCarreraTecnica")]

        public async Task<ActionResult<InversionCarreraTecnica>> GetInversionCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de la inversion con id " + id);
            var ict = await DbContext.InversionCarreraTecnica.AsSplitQuery().FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (ict == null)
            {
                Logger.LogWarning("No existe la inversion con id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del id de forma exitosa!");
            return Ok(ict);
        }
        [HttpPost]
        public async Task<ActionResult<InversionCarreraTecnica>> Post([FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una inversión nueva");
            value.InversionId = Guid.NewGuid().ToString().ToUpper();
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation("No existe la carrera técnica con id " + value.CarreraId);
                return BadRequest();
            }
            await DbContext.InversionCarreraTecnica.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar una inversión nueva");
            return new CreatedAtRouteResult("GetInversionCarreraTecnica", new { id = value.InversionId }, value);
        }

        [HttpDelete("{id}", Name = "DeleteInversionCarreraTecnica")]

        public async Task<ActionResult<InversionCarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar una inversion con id " + id);
            InversionCarreraTecnica ict = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (ict == null)
            {
                Logger.LogWarning("No se encontro la inversión");
                return NotFound();
            }
            else
            {
                DbContext.InversionCarreraTecnica.Remove(ict);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado la inversión con id " + id);
                return ict;
            }
        }

        [HttpPut("{id}", Name = "UpdateInversionCarreraTecnica")]

        public async Task<ActionResult<InversionCarreraTecnica>> Put(string id, [FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización de la inversion con id " + id);
            InversionCarreraTecnica ict = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (ict == null)
            {
                Logger.LogWarning("No se encontro la inversión");
                return BadRequest();
            }
            ict.MontoInscripcion = value.MontoInscripcion;
            ict.NumeroPagos = value.NumeroPagos;
            ict.MontoPago = value.MontoPago;
            DbContext.Entry(ict).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado la inversión");
            return NoContent();
        }
    }
}