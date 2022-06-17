using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Aspirantes")]
    public class AspiranteController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;

        public AspiranteController(KalumDbContext _DbContext, ILogger<AspiranteController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aspirante>>> Get()
        {
            List<Aspirante> aspirantes = null;
            Logger.LogDebug("Iniciando proceso de consulta de aspirantes a la BD");
            aspirantes = await DbContext.Aspirante.AsSplitQuery().ToListAsync();
            if (aspirantes == null || aspirantes.Count == 0)
            {
                Logger.LogWarning("No existen aspirantes");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(aspirantes);
        }
        [HttpGet("{id}", Name = "GetAspirante")]
        public async Task<ActionResult<Aspirante>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el no. expediente " + id);
            var aspirante = await DbContext.Aspirante.AsSplitQuery().FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning("No existe el aspirante con no. de expediente " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(aspirante);
        }
    }
}