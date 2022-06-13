using System.ComponentModel.DataAnnotations;

namespace WebApiKalum_Backend.Entities
{
    public class Alumno
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Carne { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(64, MinimumLength = 4, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "La cantidad mínima es de {2} y la máxima es {1} caracteres para el campo {0}")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentaPorCobrar> CuentasPorCobrar { get; set; }
    }
}