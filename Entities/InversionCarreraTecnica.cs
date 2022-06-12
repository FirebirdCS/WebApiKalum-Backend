using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum_Backend.Entities
{
    public class InversionCarreraTecnica
    {
        public string InversionId { get; set; }
        [Precision(10, 2)]
        public Double MontoInscripcion { get; set; }
        public int NumeroPagos { get; set; }
        [Precision(10, 2)]
        public Double MontoPago { get; set; }
        public string CarreraId { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}