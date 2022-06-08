using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumnos")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;

        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> Get()
        {
            List<Alumno> alumno = null;
            Logger.LogDebug("Iniciando el proceso de consulta de los alumnos en la BD");
            alumno = await DbContext.Alumno.Include(ins => ins.Inscripciones).Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().ToListAsync();
            if (alumno == null || alumno.Count == 0)
            {
                Logger.LogWarning("No existen alumnos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(alumno);

        }
        [HttpGet("{id}", Name = "GetAlumno")]

        public async Task<ActionResult<Alumno>> GetAlumno(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del alumno con carne " + id);
            var alumno = await DbContext.Alumno.Include(ins => ins.Inscripciones).Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().FirstOrDefaultAsync(al => al.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el carne " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del carne de forma exitosa!");
            return Ok(alumno);
        }
    }
}