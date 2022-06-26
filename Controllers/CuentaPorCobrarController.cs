using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CuentasPorCobrar")]
    public class CuentaPorCobrarController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CuentaPorCobrarController> Logger;

        public CuentaPorCobrarController(KalumDbContext _DbContext, ILogger<CuentaPorCobrarController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaPorCobrar>>> Get()
        {
            List<CuentaPorCobrar> cuenta = null;
            Logger.LogDebug("Iniciando el proceso de consulta de las cuentas en la BD");
            cuenta = await DbContext.CuentaPorCobrar.AsSplitQuery().ToListAsync();
            if (cuenta == null || cuenta.Count == 0)
            {
                Logger.LogWarning("No existen cargos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa!");
            return Ok(cuenta);
        }

    }
}