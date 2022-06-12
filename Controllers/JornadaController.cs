using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornadas")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;

        public JornadaController(KalumDbContext _DbContext, ILogger<JornadaController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jornada>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando el proceso de consulta de las jornadas en la BD");
            jornadas = await DbContext.Jornada.Include(a => a.Aspirantes).Include(ins => ins.Inscripciones).AsSplitQuery().ToListAsync();
            if (jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existen jornadas");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(jornadas);

        }
        [HttpGet("{id}", Name = "GetJornada")]

        public async Task<ActionResult<Jornada>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de Jornada con id " + id);
            var jornada = await DbContext.Jornada.Include(a => a.Aspirantes).Include(ins => ins.Inscripciones).AsSplitQuery().FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No existe la jornada con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(jornada);
        }
        [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una Jornada nueva");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar una Jornada");
            return new CreatedAtRouteResult("GetJornada", new { id = value.JornadaId }, value);
        }

        [HttpDelete("{id}", Name = "DeleteJornada")]

        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar una Jornada con id " + id);
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No se encontro la jornada");
                return NotFound();
            }
            else
            {
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado la Jornada con id " + id);
                return jornada;
            }
        }

        [HttpPut("{id}", Name = "UpdateJornada")]

        public async Task<ActionResult<Jornada>> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización de la Jornada con id " + id);
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No se encontro la jornada");
                return BadRequest();
            }
            jornada.JornadaTipo = value.JornadaTipo;
            jornada.DescripcionJornada = value.DescripcionJornada;
            DbContext.Entry(jornada).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado la Jornada");
            return NoContent();
        }
    }
}