using System.ComponentModel.DataAnnotations;
using WebApiKalum_Backend.Helpers;

namespace WebApiKalum_Backend.Entities
{
    public class ResultadoExamenAdmision
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Anio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string DescripcionResultado { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Nota { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}