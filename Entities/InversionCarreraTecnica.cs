using WebApiKalum.Entities;

namespace WebApiKalum_Backend.Entities
{
    public class InversionCarreraTecnica
    {
        public string InversionId { get; set; }
        public Decimal MontoInscripcion { get; set; }
        public int NumeroPagos { get; set; }
        public Decimal MontoPago { get; set; }
        public string CarreraId { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}