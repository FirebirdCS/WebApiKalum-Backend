using System.ComponentModel.DataAnnotations;
using WebApiKalum.Entities;
using Microsoft.VisualBasic;
using WebApiKalum_Backend.Helpers;

namespace WebApiKalum_Backend.Entities
{
    public class Aspirante
    {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(12, MinimumLength = 11, ErrorMessage = "El campo debe ser de 11 caracteres como mínimo y máximo")]
        [NoExpediente]
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
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Email { get; set; }
        public string Estatus { get; set; } = "NO ASIGNADO";
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