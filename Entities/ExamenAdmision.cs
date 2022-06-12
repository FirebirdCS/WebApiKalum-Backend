using System.ComponentModel.DataAnnotations;

namespace WebApiKalum_Backend.Entities
{
    public class ExamenAdmision
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ExamenId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime FechaExamen { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
    }
}