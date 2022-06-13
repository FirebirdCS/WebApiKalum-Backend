using System.ComponentModel.DataAnnotations;
using WebApiKalum.Entities;

namespace WebApiKalum_Backend.Entities
{
    public class Aspirante
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Estatus { get; set; }
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        public string ExamenId { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public virtual Jornada Jornada { get; set; }
        public virtual ExamenAdmision ExamenAdmision { get; set; }
        public virtual List<ResultadoExamenAdmision> ResultadosExamenAdmisiones { get; set; }
        public virtual List<InscripcionPago> InscripcionesPago { get; set; }
    }
}