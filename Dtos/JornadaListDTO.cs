namespace WebApiKalum_Backend.Dtos
{
    public class JornadaListDTO
    {
        public string JornadaId { get; set; }
        public string JornadaTipo { get; set; }
        public string DescripcionJornada { get; set; }
        public List<AspiranteListCTDTO> Aspirantes { get; set; }
        public List<InscripcionListDTO> Inscripciones { get; set; }
    }
}