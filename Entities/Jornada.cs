namespace WebApiKalum_Backend.Entities
{
    public class Jornada
    {
        public string JornadaId { get; set; }
        public string JornadaTipo { get; set; }
        public string DescripcionJornada { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
    }
}