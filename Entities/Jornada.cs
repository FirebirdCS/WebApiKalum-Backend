using System.ComponentModel.DataAnnotations;

namespace WebApiKalum_Backend.Entities
{
    public class Jornada
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string JornadaId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "La cantidad mínima es de {2} caracteres para el campo {0}")]
        public string JornadaTipo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string DescripcionJornada { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
    }
}