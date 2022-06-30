using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum_Backend.Dtos;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CuentasPorCobrar")]
    public class CuentaPorCobrarController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CuentaPorCobrarController> Logger;
        private readonly IMapper Mapper;

        public CuentaPorCobrarController(KalumDbContext _DbContext, ILogger<CuentaPorCobrarController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaPorCobrarListDTO>>> Get()
        {
            List<CuentaPorCobrar> cuenta = null;
            Logger.LogDebug("Iniciando el proceso de consulta de las cuentas en la BD");
            cuenta = await DbContext.CuentaPorCobrar.AsSplitQuery().ToListAsync();
            if (cuenta == null || cuenta.Count == 0)
            {
                Logger.LogWarning("No existen cargos");
                return new NoContentResult();
            }
            List<CuentaPorCobrarListDTO> lista = Mapper.Map<List<CuentaPorCobrarListDTO>>(cuenta);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(lista);
        }
        [HttpGet("{id}", Name = "GetCuentaPorCobrar")]

        public async Task<ActionResult<CuentaPorCobrarListDTO>> GetCuentaPorCobrar(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de la cuenta por cobrar con id " + id);
            var cuenta = await DbContext.CuentaPorCobrar.AsSplitQuery().FirstOrDefaultAsync(cpc => cpc.CuentaId == id);
            if (cuenta == null)
            {
                Logger.LogWarning("No existe la cuenta por cobrar con id " + id);
                return new NoContentResult();
            }
            CuentaPorCobrarListDTO lista = Mapper.Map<CuentaPorCobrarListDTO>(cuenta);
            Logger.LogInformation("Se ejecuto la petición del id de forma exitosa!");
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<CuentaPorCobrar>> Post([FromBody] CuentaPorCobrar value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una cuenta por cobrar");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == value.Carne);
            if (alumno == null)
            {
                Logger.LogInformation("No existe el alumno con Carne " + value.Carne);
                return BadRequest();
            }
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == value.CargoId);
            if (cargo == null)
            {
                Logger.LogInformation("No existe el cargo con id " + value.CargoId);
                return BadRequest();
            }
            await DbContext.CuentaPorCobrar.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se finalizó el proceso de agregar una cuenta por cobrar");
            return new CreatedAtRouteResult("GetCuentaPorCobrar", new { id = value.CuentaId }, value);
        }

        [HttpDelete("{id}", Name = "DeleteCuentaPorCobrar")]

        public async Task<ActionResult<CuentaPorCobrar>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminar una cuenta por cobrar con id " + id);
            CuentaPorCobrar cuenta = await DbContext.CuentaPorCobrar.FirstOrDefaultAsync(cpc => cpc.CuentaId == id);
            if (cuenta == null)
            {
                Logger.LogWarning("No se encontro la cuenta");
                return NotFound();
            }
            else
            {
                DbContext.CuentaPorCobrar.Remove(cuenta);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Se ha eliminado la cuenta con id " + id);
                return cuenta;

            }
        }
        [HttpPut("{id}", Name = "UpdateCuentaPorCobrar")]

        public async Task<ActionResult<CuentaPorCobrar>> Put(string id, [FromBody] CuentaPorCobrar value)
        {
            Logger.LogDebug("Iniciando el proceso de actualización de la cuenta con id " + id);
            CuentaPorCobrar cuenta = await DbContext.CuentaPorCobrar.FirstOrDefaultAsync(cpc => cpc.CuentaId == id);
            if (cuenta == null)
            {
                Logger.LogWarning("No se encontro la cuenta");
                return BadRequest();
            }
            cuenta.Descripcion = value.Descripcion;
            cuenta.FechaCargo = value.FechaCargo;
            cuenta.FechaAplica = value.FechaAplica;
            cuenta.MontoCargo = value.MontoCargo;
            cuenta.Mora = value.Mora;
            cuenta.Descuento = value.Descuento;
            DbContext.Entry(cuenta).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha actualizado la cuenta");
            return NoContent();
        }


    }
}