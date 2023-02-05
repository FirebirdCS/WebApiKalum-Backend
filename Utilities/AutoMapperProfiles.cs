using AutoMapper;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Dtos;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum_Backend.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<JornadaCreateDTO, Jornada>();
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<ExamenAdmisionCreateDTO, ExamenAdmision>();
            CreateMap<Aspirante, AspiranteListCTDTO>().ConstructUsing(e => new AspiranteListCTDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing(e => new AspiranteListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Inscripcion, InscripcionCreateDTO>();
            CreateMap<CarreraTecnica, CarreraTecnicaListDTO>();
            CreateMap<Inscripcion, InscripcionListDTO>();
            CreateMap<Jornada, JornadaListDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionListDTO>();
            CreateMap<CuentaPorCobrar, CuentaPorCobrarListDTO>();
            CreateMap<Cargo, CargoListDTO>();
            CreateMap<InscripcionPago, InscripcionPagoListDTO>();
            CreateMap<CarreraTecnica, CarreraTecnicaListIPDTO>();
            CreateMap<InversionCarreraTecnica, InversionCarreraTecnicaListDTO>();
            CreateMap<Alumno, AlumnoListDTO>().ConstructUsing(e => new AlumnoListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
        }
    }
}