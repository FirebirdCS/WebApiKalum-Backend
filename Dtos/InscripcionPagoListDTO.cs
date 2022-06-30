namespace WebApiKalum_Backend.Dtos
{
    public class InscripcionPagoListDTO
    {
        public DateTime FechaPago { get; set; }
        public Decimal Monto { get; set; }
        public String NoExpediente { get; set; }
        public AspiranteListCTDTO Aspirante { get; set; }
    }
}