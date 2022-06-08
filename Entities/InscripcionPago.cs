namespace WebApiKalum_Backend.Entities
{
    public class InscripcionPago
    {
        public String BoletaPago { get; set; }
        public DateTime FechaPago { get; set; }
        public Double Monto { get; set; }
        public String Anio { get; set; }
        public String NoExpediente { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}