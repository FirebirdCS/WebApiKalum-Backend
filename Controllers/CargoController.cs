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
    [Route("v1/KalumManagement/Cargos")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger;
        private readonly IMapper Mapper;

        public CargoController(KalumDbContext _DbContext, ILogger<CargoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> Get()
        {
            List<Cargo> cargo = null;
            Logger.LogDebug("Iniciando el proceso de consulta de los cargos en la BD");
            cargo = await DbContext.Cargo.Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().ToListAsync();
            if (cargo == null || cargo.Count == 0)
            {
                Logger.LogWarning("No existen cargos");
                return new NoContentResult();
            }
            List<CargoListDTO> lista = Mapper.Map<List<CargoListDTO>>(cargo);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(lista);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> GetPagination(int page)
        {
            var queryable = this.DbContext.Cargo.Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().AsQueryable();
            var paginacion = new HttpResponsePagination<Cargo>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }
        }

        [HttpGet("{id}", Name = "GetCargo")]

        public async Task<ActionResult<CargoListDTO>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del cargo con id " + id);
            var cargo = await DbContext.Cargo.Include(cpc => cpc.CuentasPorCobrar).AsSplitQuery().FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning("No existe el cargo con id " + id);
                return new NoContentResult();
            }
            CargoListDTO lista = Mapper.Map<CargoListDTO>(cargo);
            Logger.LogInformation("Se ejecuto la petición del id de forma exitosa!");
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<Cargo>> Post([FromBody] Cargo value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un cargo");
            value.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar un cargo");
            return new CreatedAtRouteResult("GetCargo", new { id = value.CargoId }, value);
        }

        [HttpDelete("{id}", Name = "DeleteCargo")]

        public async Task<ActionResult<Cargo>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar un cargo con id " + id);
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning("No se encontro el cargo");
                return NotFound();
            }
            else
            {
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado el cargo con id " + id);
                return cargo;

            }
        }

        [HttpPut("{id}", Name = "UpdateCargo")]

        public async Task<ActionResult<Cargo>> Put(string id, [FromBody] Cargo value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización del cargo con id " + id);
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning("No se encontro el cargo");
                return BadRequest();
            }
            cargo.Descripcion = value.Descripcion;
            cargo.Prefijo = value.Prefijo;
            cargo.Monto = value.Monto;
            cargo.PorcentajeMora = value.PorcentajeMora;
            DbContext.Entry(cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado el cargo");
            return NoContent();
        }
    }
}