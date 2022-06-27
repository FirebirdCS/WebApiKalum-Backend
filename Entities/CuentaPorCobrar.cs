using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum_Backend.Entities
{
    public class CuentaPorCobrar
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CuentaId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Anio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaAplica { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Precision(10, 2)]
        public Decimal MontoCargo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Precision(10, 2)]
        public Decimal Mora { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Precision(10, 2)]
        public Decimal Descuento { get; set; }

        public string Carne { get; set; }
        public string CargoId { get; set; }

        public virtual Alumno Alumno { get; set; }

        public virtual Cargo Cargo { get; set; }

    }
}