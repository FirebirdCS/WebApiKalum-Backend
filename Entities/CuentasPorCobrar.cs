namespace WebApiKalum_Backend.Entities
{
    public class CuentasPorCobrar
    {
        public string CuentaId { get; set; }
        public string Anio { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCargo { get; set; }
        public DateTime FechaAplica { get; set; }
        public Double MontoCargo { get; set; }
        public Double Mora { get; set; }
        public Double Descuento { get; set; }
        public string Carne { get; set; }
        public string CargoId { get; set; }

        public virtual Alumno Alumno { get; set; }

        public virtual Cargo Cargo { get; set; }

    }
}