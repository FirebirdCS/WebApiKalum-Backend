using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum_Backend.Entities
{
    public class Cargo
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CargoId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Prefijo { get; set; }
        [Precision(10, 2)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public Decimal Monto { get; set; }
        public bool GenerarMora { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int PorcentajeMora { get; set; }
        public virtual List<CuentaPorCobrar> CuentasPorCobrar { get; set; }
    }
}