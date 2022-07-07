using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Dtos;
using WebApiKalum_Backend.Entities;
using WebApiKalum_Backend.Utilities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumnos")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        private readonly IMapper Mapper;

        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> Get()
        {
            List<Alumno> lista = null;
            Logger.LogDebug("Iniciando el proceso de consulta de los alumnos en la BD");
            lista = await DbContext.Alumno.Include(ins => ins.Inscripciones).Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().ToListAsync();
            if (lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen alumnos");
                return new NoContentResult();
            }
            List<AlumnoListDTO> alumno = Mapper.Map<List<AlumnoListDTO>>(lista);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(alumno);

        }
        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> GetPagination(int page)
        {
            var queryable = this.DbContext.Alumno.Include(ins => ins.Inscripciones).Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().AsQueryable();
            var paginacion = new HttpResponsePagination<Alumno>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }
        }
        [HttpGet("{id}", Name = "GetAlumno")]

        public async Task<ActionResult<AlumnoListDTO>> GetAlumno(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del alumno con carne " + id);
            var alumno = await DbContext.Alumno.Include(ins => ins.Inscripciones).Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().FirstOrDefaultAsync(al => al.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el carne " + id);
                return new NoContentResult();
            }
            AlumnoListDTO lista = Mapper.Map<AlumnoListDTO>(alumno);
            Logger.LogInformation("Se ejecuto la petición del carne de forma exitosa!");
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<Alumno>> Post([FromBody] Alumno value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un alumno");
            await DbContext.Alumno.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar un alumno");
            return new CreatedAtRouteResult("GetAlumno", new { id = value.Carne }, value);
        }

        [HttpDelete("{id}", Name = "DeleteAlumno")]

        public async Task<ActionResult<Alumno>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar un alumno con carné " + id);
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning("No se encontro el alumno");
                return NotFound();
            }
            else
            {
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado el alumno con carné " + id);
                return alumno;

            }
        }

        [HttpPut("{id}", Name = "UpdateAlumno")]

        public async Task<ActionResult<Alumno>> Put(string id, [FromBody] Alumno value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización del alumno con carne " + id);
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning("No se encontro el alumno");
                return BadRequest();
            }
            alumno.Apellidos = value.Apellidos;
            alumno.Nombres = value.Nombres;
            alumno.Direccion = value.Direccion;
            alumno.Telefono = value.Telefono;
            alumno.Email = value.Email;
            DbContext.Entry(alumno).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado el alumno");
            return NoContent();
        }

    }
}