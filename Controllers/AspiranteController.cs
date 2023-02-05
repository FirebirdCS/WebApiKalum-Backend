using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Dtos;
using WebApiKalum_Backend.Entities;
using WebApiKalum_Backend.Services;
using WebApiKalum_Backend.Utilities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Aspirantes")]
    public class AspiranteController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;
        private readonly IUtilsService UtilsService;

        public AspiranteController(KalumDbContext _DbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper, IUtilsService _UtilsService)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
            this.UtilsService = _UtilsService;
        }
        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            List<Aspirante> lista = null;
            Logger.LogDebug("Iniciando proceso de consulta de aspirantes a la BD");
            lista = await DbContext.Aspirante.Include(a => a.CarreraTecnica).Include(a => a.Jornada).Include(a => a.ExamenAdmision).AsSplitQuery().ToListAsync();
            if (lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen aspirantes");
                return new NoContentResult();
            }
            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(lista);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(aspirantes);
        }
        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> GetPagination(int page)
        {
            var queryable = this.DbContext.Aspirante.Include(a => a.CarreraTecnica).Include(a => a.Jornada).Include(a => a.ExamenAdmision).AsSplitQuery().AsQueryable();
            int registros = await queryable.CountAsync();
            if (registros == 0)
            {
                return NoContent();
            }
            else
            {
                var aspirantes = await queryable.OrderBy(aspirante => aspirante.NoExpediente).Paginar(page).ToListAsync();
                PaginationResponse<AspiranteListDTO> response = new PaginationResponse<AspiranteListDTO>(Mapper.Map<List<AspiranteListDTO>>(aspirantes), page, registros);
                return Ok(response);
            }
        }
        [HttpGet("{id}", Name = "GetAspirante")]
        public async Task<ActionResult<AspiranteListDTO>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el no. expediente " + id);
            var aspirante = await DbContext.Aspirante.Include(a => a.CarreraTecnica).Include(a => a.Jornada).Include(a => a.ExamenAdmision).AsSplitQuery().FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning("No existe el aspirante con no. de expediente " + id);
                return new NoContentResult();
            }
            AspiranteListDTO lista = Mapper.Map<AspiranteListDTO>(aspirante);
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(lista);
        }
        [HttpPost]
        public async Task<ActionResult<AspiranteListDTO>> Post([FromBody] AspiranteCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un Aspirante nuevo");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation("No existe la carrera técnica con id " + value.CarreraId);
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if (jornada == null)
            {
                Logger.LogInformation("No existe la jornada con id " + value.JornadaId);
                return BadRequest();
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ex => ex.ExamenId == value.ExamenId);
            if (examenAdmision == null)
            {
                Logger.LogInformation("No existe el examen con id " + value.ExamenId);
                return BadRequest();
            }
            bool result = await this.UtilsService.CrearExpedienteAsync(value);
            CandidateRecordResponse candidateRecordResponse = new CandidateRecordResponse();
            if (result)
            {
                candidateRecordResponse.Status = "Ok";
                candidateRecordResponse.Mensaje = $"El proceso de solicitud del expediente fue creado exitosamente, pronto recibira su número de expediente al correo {value.Email}";
            }
            else
            {
                candidateRecordResponse.Status = "Error";
                candidateRecordResponse.Mensaje = $"Hubo un problema al crear la solicitud intente de nuevo o más tarde";
            }
            Logger.LogInformation($"Se ha creato la solicitud del aspirante con exito");
            return Ok(candidateRecordResponse);
        }

        [HttpDelete("{id}", Name = "DeleteAspirante")]

        public async Task<ActionResult<Aspirante>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar un aspirante con expediente " + id);
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning("No se encontro el aspirante");
                return NotFound();
            }
            else
            {
                DbContext.Aspirante.Remove(aspirante);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado el aspirante con no. expediente " + id);
                return aspirante;

            }
        }

        [HttpPut("{id}", Name = "UpdateAspirante")]

        public async Task<ActionResult<Aspirante>> Put(string id, [FromBody] Aspirante value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización del examen de admisión con no. expediente " + id);
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning("No se encontro el aspirante");
                return BadRequest();
            }
            aspirante.Apellidos = value.Apellidos;
            aspirante.Nombres = value.Nombres;
            aspirante.Direccion = value.Direccion;
            aspirante.Telefono = value.Telefono;
            aspirante.Estatus = value.Estatus;
            aspirante.CarreraId = value.CarreraId;
            aspirante.JornadaId = value.JornadaId;
            aspirante.ExamenId = value.ExamenId;
            DbContext.Entry(aspirante).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado el aspirante");
            return NoContent();
        }
    }
}