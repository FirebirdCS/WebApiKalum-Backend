namespace WebApiKalum_Backend.Entities
{
    public class ResultadoExamenAdmision
    {
        public string NoExpediente { get; set; }
        public string Anio { get; set; }
        public string DescripcionResultado { get; set; }
        public int Nota { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}