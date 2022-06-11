using System.ComponentModel.DataAnnotations;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum.Entities
{
    public class CarreraTecnica
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CarreraId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Nombre { get; set; }

        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<InversionCarreraTecnica> InversionesCarrerasTecnicas { get; set; }

    }
}