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
        private readonly ILogger<CarreraTecnicaController> Logger;

        public JornadaController(KalumDbContext _DbContext, ILogger<CarreraTecnicaController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jornada>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando el proceso de consulta de las jornadas en la BD");
            jornadas = await DbContext.Jornada.ToListAsync();
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
            var jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No existe la jornada con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(jornada);
        }
    }
}