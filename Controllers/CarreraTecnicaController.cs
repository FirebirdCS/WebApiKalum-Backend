using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Dtos;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CarrerasTecnicas")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper;

        public CarreraTecnicaController(KalumDbContext _DbContext, ILogger<CarreraTecnicaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<CarreraTecnicaListDTO>>> Get()
        {
            List<CarreraTecnica> lista = null;
            Logger.LogDebug("Iniciando proceso de consulta de carreras técnicas a la BD");
            lista = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(ins => ins.Inscripciones).AsSplitQuery().ToListAsync();
            if (lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen carreras técnicas");
                return new NoContentResult();
            }
            List<CarreraTecnicaListDTO> carrerasTecnicas = Mapper.Map<List<CarreraTecnicaListDTO>>(lista);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(carrerasTecnicas);
        }
        [HttpGet("{id}", Name = "GetCarreraTecnica")]
        public async Task<ActionResult<CarreraTecnica>> GetCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var carrera = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(ins => ins.Inscripciones).AsSplitQuery().FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if (carrera == null)
            {
                Logger.LogWarning("No existe la carrera técnica con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición del ID de forma exitosa!");
            return Ok(carrera);
        }
        [HttpPost]
        public async Task<ActionResult<CarreraTecnica>> Post([FromBody] CarreraTecnicaCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una CarreraTecnica nueva");
            CarreraTecnica nuevo = Mapper.Map<CarreraTecnica>(value);
            nuevo.CarreraId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.CarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar una CarreraTecnica");
            return new CreatedAtRouteResult("GetCarreraTecnica", new { id = nuevo.CarreraId }, nuevo);
        }

        [HttpDelete("{id}", Name = "DeleteCarreraTecnica")]

        public async Task<ActionResult<CarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar una CarreraTecnica con id " + id);
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if (carreraTecnica == null)
            {
                Logger.LogWarning("No se encontro la carrera técnica");
                return NotFound();
            }
            else
            {
                DbContext.CarreraTecnica.Remove(carreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado la CarreraTecnica con id " + id);
                return carreraTecnica;
            }
        }

        [HttpPut("{id}", Name = "UpdateCarreraTecnica")]

        public async Task<ActionResult<CarreraTecnica>> Put(string id, [FromBody] CarreraTecnica value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización de la CarreraTecnica con id " + id);
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if (carreraTecnica == null)
            {
                Logger.LogWarning("No se encontro la carrera técnica");
                return BadRequest();
            }
            carreraTecnica.Nombre = value.Nombre;
            DbContext.Entry(carreraTecnica).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado la CarreraTecnica");
            return NoContent();
        }

    }
}