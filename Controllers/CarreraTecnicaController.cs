using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;

        public CarreraTecnicaController(KalumDbContext _DbContext)
        {
            this.DbContext = _DbContext;
        }
        [HttpGet]
        public ActionResult<List<CarreraTecnica>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            carrerasTecnicas = DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(ins => ins.Inscripciones).Include(ict => ict.InversionesCarrerasTecnicas).ToList();
            if (carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                return new NoContentResult();
            }
            return Ok(carrerasTecnicas);
        }
    }
}