using Microsoft.AspNetCore.Mvc;
using WebApiKalum;

namespace WebApiKalum_Backend.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/InversionesCarrerasTecnicas")]
    public class InversionCarreraTecnicaController
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InversionCarreraTecnicaController> Logger;

        public InversionCarreraTecnicaController(KalumDbContext _DbContext, ILogger<InversionCarreraTecnicaController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
    }
}