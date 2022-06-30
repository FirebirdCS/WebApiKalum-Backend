namespace WebApiKalum_Backend.Dtos
{
    public class InversionCarreraTecnicaListDTO
    {
        public Decimal MontoInscripcion { get; set; }
        public int NumeroPagos { get; set; }
        public Decimal MontoPago { get; set; }
        public CarreraTecnicaListIPDTO CarreraTecnica { get; set; }
    }
}