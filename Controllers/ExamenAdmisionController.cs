using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenesAdmision")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;

        public ExamenAdmisionController(KalumDbContext _DbContext, ILogger<ExamenAdmisionController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmision>>> Get()
        {
            List<ExamenAdmision> examen = null;
            Logger.LogDebug("Iniciando el proceso de consulta de los examenes de admision en la BD");
            examen = await DbContext.ExamenAdmision.Include(a => a.Aspirantes).AsSplitQuery().ToListAsync();
            if (examen == null || examen.Count == 0)
            {
                Logger.LogWarning("No existen examenes de admision");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(examen);
        }
        [HttpGet("{id}", Name = "GetExamenAdmision")]

        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del examen con id " + id);
            var examen = await DbContext.ExamenAdmision.Include(a => a.Aspirantes).AsSplitQuery().FirstOrDefaultAsync(ex => ex.ExamenId == id);
            if (examen == null)
            {
                Logger.LogWarning("No existe el examen con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(examen);
        }
    }
}