using System.ComponentModel.DataAnnotations;
using WebApiKalum.Entities;

namespace WebApiKalum_Backend.Entities
{
    public class Inscripcion
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string InscripcionId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Ciclo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaInscripcion { get; set; }
        public string Carne { get; set; }
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        public virtual Jornada Jornada { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public virtual Alumno Alumno { get; set; }
    }
}