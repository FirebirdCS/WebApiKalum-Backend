using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum_Backend.Entities;
using WebApiKalum_Backend.Utilities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ResultadosExamenAdmision")]
    public class ResultadoExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ResultadoExamenAdmisionController> Logger;

        public ResultadoExamenAdmisionController(KalumDbContext _DbContext, ILogger<ResultadoExamenAdmisionController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmision>>> Get()
        {
            List<ResultadoExamenAdmision> resultado = null;
            Logger.LogDebug("Iniciando el proceso de consulta de los resultados de examenes de admision en la BD");
            resultado = await DbContext.ResultadoExamenAdmision.Include(a => a.Aspirante).AsSplitQuery().ToListAsync();
            if (resultado == null || resultado.Count == 0)
            {
                Logger.LogWarning("No existen resultados de examenes de admision");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(resultado);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmision>>> GetPagination(int page)
        {
            var queryable = DbContext.ResultadoExamenAdmision.Include(a => a.Aspirante).AsSplitQuery().AsQueryable();
            var paginacion = new HttpResponsePagination<ResultadoExamenAdmision>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }
        }

        [HttpGet("{id}", Name = "GetResultadoExamenAdmision")]

        public async Task<ActionResult<ResultadoExamenAdmision>> GetResultadoExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del resultado de examen con no. de expediente " + id);
            var resultado = await DbContext.ResultadoExamenAdmision.Include(a => a.Aspirante).AsSplitQuery().FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if (resultado == null)
            {
                Logger.LogWarning("No existe el resultado de examen con el no. de expediente " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<ResultadoExamenAdmision>> Post([FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un resultado de examen de admision");

            await DbContext.ResultadoExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar un resultado de examen de admisión");
            return new CreatedAtRouteResult("GetResultadoExamenAdmision", new { id = value.NoExpediente }, value);
        }

        [HttpDelete("{id}", Name = "DeleteResultadoExamenAdmision")]

        public async Task<ActionResult<ResultadoExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar un resultado de examen de admision con no. de expediente " + id);
            ResultadoExamenAdmision resultado = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if (resultado == null)
            {
                Logger.LogWarning("No se encontro el resultado de examen de admision");
                return NotFound();
            }
            else
            {
                DbContext.ResultadoExamenAdmision.Remove(resultado);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado el resultado examen de admisión con no. de expediente " + id);
                return resultado;

            }
        }

        [HttpPut("{id}", Name = "UpdateResultadoExamenAdmision")]

        public async Task<ActionResult<ResultadoExamenAdmision>> Put(string id, [FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización del resultado de examen de admisión con no. expediente " + id);
            ResultadoExamenAdmision resultado = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if (resultado == null)
            {
                Logger.LogWarning("No se encontro el resultado de examen de admisión");
                return BadRequest();
            }
            resultado.DescripcionResultado = value.DescripcionResultado;
            resultado.Nota = value.Nota;
            DbContext.Entry(resultado).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado el resultado de examen de admisión");
            return NoContent();
        }
    }

}