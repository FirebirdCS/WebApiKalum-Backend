using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Dtos;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenesAdmision")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        private readonly IMapper Mapper;

        public ExamenAdmisionController(KalumDbContext _DbContext, ILogger<ExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionListDTO>>> Get()
        {
            List<ExamenAdmision> examen = null;
            Logger.LogDebug("Iniciando el proceso de consulta de los examenes de admision en la BD");
            examen = await DbContext.ExamenAdmision.Include(a => a.Aspirantes).AsSplitQuery().ToListAsync();
            if (examen == null || examen.Count == 0)
            {
                Logger.LogWarning("No existen examenes de admision");
                return new NoContentResult();
            }
            List<ExamenAdmisionListDTO> lista = Mapper.Map<List<ExamenAdmisionListDTO>>(examen);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(lista);
        }
        [HttpGet("{id}", Name = "GetExamenAdmision")]

        public async Task<ActionResult<ExamenAdmisionListDTO>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del examen con id " + id);
            var examen = await DbContext.ExamenAdmision.Include(a => a.Aspirantes).AsSplitQuery().FirstOrDefaultAsync(ex => ex.ExamenId == id);
            if (examen == null)
            {
                Logger.LogWarning("No existe el examen con el id " + id);
                return new NoContentResult();
            }
            ExamenAdmisionListDTO lista = Mapper.Map<ExamenAdmisionListDTO>(examen);
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una Examen de Admision");
            value.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar un examen de admisión");
            return new CreatedAtRouteResult("GetExamenAdmision", new { id = value.ExamenId }, value);
        }

        [HttpDelete("{id}", Name = "DeleteExamenAdmision")]

        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar un Examen de Admision con id " + id);
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ex => ex.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning("No se encontro el examen de admision");
                return NotFound();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado el examen de admisión con id " + id);
                return examenAdmision;

            }
        }

        [HttpPut("{id}", Name = "UpdateExamenAdmision")]

        public async Task<ActionResult<ExamenAdmision>> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización del examen de admisión con id " + id);
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ex => ex.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning("No se encontro el examen de admisión");
                return BadRequest();
            }
            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado el examen de admisión");
            return NoContent();
        }
    }
}