using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Dtos;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Inscripciones")]
    public class InscripcionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<Inscripcion> Logger;
        private readonly IMapper Mapper;

        public InscripcionController(KalumDbContext _DbContext, ILogger<Inscripcion> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> Get()
        {
            List<Inscripcion> inscripcion = null;
            Logger.LogDebug("Iniciando el proceso de consulta de las inscripciones en la BD");
            inscripcion = await DbContext.Inscripcion.AsSplitQuery().ToListAsync();
            if (inscripcion == null || inscripcion.Count == 0)
            {
                Logger.LogWarning("No existen inscripciones");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(inscripcion);
        }

        [HttpPost("Enrollments")]
        public async Task<ActionResult<ResponseEnrollmentDTO>> EnrollmentCreateAsync([FromBody] EnrollmentDTO value)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if (aspirante == null)
            {
                Logger.LogInformation("No existe el aspirante con no. de expediente " + value.NoExpediente);
                return BadRequest();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation("No existe la carrera técnica con id " + value.CarreraId);
                return BadRequest();
            }
            bool respuesta = await CrearSolicitudAsync(value);
            if (respuesta == true)
            {
                ResponseEnrollmentDTO response = new ResponseEnrollmentDTO();
                response.HttpStatus = 201;
                response.Message = "El proceso de inscripción se ha realizado con exito";
                return Ok(response);
            }
            else
            {
                return StatusCode(503, value);
            }
        }

        private async Task<bool> CrearSolicitudAsync(EnrollmentDTO value)
        {
            bool proceso = false;
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conexion = null;
            IModel channel = null;
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            try
            {
                conexion = factory.CreateConnection();
                channel = conexion.CreateModel();
                channel.BasicPublish("kalum.exchange.enrollment", "", null, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
                proceso = true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                channel.Close();
                conexion.Close();
            }
            return proceso;
        }

        [HttpGet("{id}", Name = "GetInscripcion")]
        public async Task<ActionResult<Inscripcion>> GetInscripcion(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de la inscripcion con id " + id);
            var inscripcion = await DbContext.Inscripcion.AsSplitQuery().FirstOrDefaultAsync(ins => ins.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning("No existe la inscripcion con id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del id de forma exitosa!");
            return Ok(inscripcion);
        }

        [HttpPost]
        public async Task<ActionResult<Inscripcion>> Post([FromBody] Inscripcion value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una inscripcion");
            value.InscripcionId = Guid.NewGuid().ToString().ToUpper();
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == value.Carne);
            if (alumno == null)
            {
                Logger.LogInformation("No existe el alumno con Carne " + value.Carne);
                return BadRequest();
            }
            CarreraTecnica carrera = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carrera == null)
            {
                Logger.LogInformation("No existe el cargo con id " + value.CarreraId);
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if (jornada == null)
            {
                Logger.LogInformation("No existe la jornada con id " + value.JornadaId);
                return BadRequest();
            }
            await DbContext.Inscripcion.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar una inscripcion");
            return new CreatedAtRouteResult("GetInscripcion", new { id = value.InscripcionId }, value);
        }

        [HttpDelete("{id}", Name = "DeleteInscripcion")]

        public async Task<ActionResult<Inscripcion>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar una inscripcion con id " + id);
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(ins => ins.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning("No se encontro la inscripcion");
                return NotFound();
            }
            else
            {
                DbContext.Inscripcion.Remove(inscripcion);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado la inscripcion con id " + id);
                return inscripcion;

            }
        }

        [HttpPut("{id}", Name = "UpdateInscripcion")]

        public async Task<ActionResult<Inscripcion>> Put(string id, [FromBody] Inscripcion value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización de la inscripcion con id " + id);
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(ins => ins.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning("No se encontro la inscripcion");
                return BadRequest();
            }
            inscripcion.Ciclo = value.Ciclo;
            inscripcion.FechaInscripcion = value.FechaInscripcion;
            DbContext.Entry(inscripcion).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado la inscripcion");
            return NoContent();
        }

    }

}