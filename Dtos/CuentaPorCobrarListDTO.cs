namespace WebApiKalum_Backend.Dtos
{
    public class CuentaPorCobrarListDTO
    {
        public string CuentaId { get; set; }
        public string Descripcion { get; set; }
        public Decimal MontoCargo { get; set; }
    }
}