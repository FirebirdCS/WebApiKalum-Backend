namespace WebApiKalum_Backend.Dtos
{
    public class ExamenAdmisionListDTO
    {
        public string ExamenId { get; set; }
        public DateTime FechaExamen { get; set; }
        public List<AspiranteListCTDTO> Aspirantes { get; set; }
    }
}