using FDevs.Models;

namespace FDevs.Services.EstadoVideoService
{
    public interface IEstadoVideoService
    {
        Task<List<UsuarioEstadoVideo>> GetUsuarioEstadoVideosAsync();
        Task<UsuarioEstadoVideo> GetUsuarioEstadoVideosByIdAsync(string usuarioId, int videoId);
        Task<List<UsuarioEstadoVideo>> GetByUsuarioIdAsync(string usuarioId);
    }
}
