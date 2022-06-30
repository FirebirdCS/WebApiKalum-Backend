namespace WebApiKalum_Backend.Dtos
{
    public class CargoListDTO
    {
        public string Prefijo { get; set; }
        public Decimal Monto { get; set; }
        public List<CuentaPorCobrarListDTO> CuentasPorCobrar { get; set; }
    }
}