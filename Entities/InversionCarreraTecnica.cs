using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum_Backend.Entities
{
    public class InversionCarreraTecnica
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string InversionId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Precision(10, 2)]
        public Decimal MontoInscripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int NumeroPagos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Precision(10, 2)]
        public Decimal MontoPago { get; set; }
        public string CarreraId { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}