using WebApiKalum_Backend.Dtos;

namespace WebApiKalum_Backend.Services
{
    public interface IUtilsService
    {
        public Task<bool> CrearSolicitudAsync(EnrollmentDTO value);
        public Task<bool> CrearExpedienteAsync(AspiranteCreateDTO value);
    }
}