using FDevs.Models;

namespace FDevs.Services.EstadoModuloService
{
    public interface IEstadoModuloService
    {
        Task<List<UsuarioEstadoModulo>> GetUsuarioEstadoModulosAsync();
        Task<UsuarioEstadoModulo> GetUsuarioEstadoModuloByIdAsync(string usuarioId, int estadoId, int moduloId);
        Task<List<UsuarioEstadoModulo>> GetByUsuarioIdAsync(string usuarioId);
    }
}
