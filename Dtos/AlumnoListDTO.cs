namespace WebApiKalum_Backend.Dtos
{
    public class AlumnoListDTO
    {
        public string Carne { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public List<InscripcionListDTO> Inscripciones { get; set; }
        public List<CuentaPorCobrarListDTO> CuentasPorCobrar { get; set; }
    }
}