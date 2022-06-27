using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApiKalum_Backend.Helpers;

namespace WebApiKalum_Backend.Entities
{
    public class InscripcionPago
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public String BoletaPago { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Precision(10, 2)]
        public Decimal Monto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public String Anio { get; set; }
        [NoExpediente]
        public String NoExpediente { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}