namespace WebApiKalum_Backend.Entities
{
    public class Alumno
    {
        public string Carne { get; set; }
        public string Apellidos { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentasPorCobrar> CuentasPorCobrars { get; set; }
    }
}