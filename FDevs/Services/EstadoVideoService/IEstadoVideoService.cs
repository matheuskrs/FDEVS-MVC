using FDevs.Models;

namespace FDevs.Services.EstadoVideoService
{
    public interface IEstadoVideoService
    {
        Task<List<UsuarioEstadoVideo>> GetUsuarioEstadoVideosAsync();
        Task<UsuarioEstadoVideo> GetUsuarioEstadoVideosByIdAsync(string usuarioId, int videoId);
        Task<List<UsuarioEstadoVideo>> GetByUsuarioIdAsync(string usuarioId);
        Task<bool> AtualizarEstadoVideoParaAndamentoAsync(UsuarioEstadoVideo estadoVideo);
        Task<bool> AtualizarEstadoVideoParaConcluidoAsync(UsuarioEstadoVideo estadoVideo);
        Task<int> ObterQuantidadeVideosConcluidos(string usuarioId, Video video);
        Task<int> ObterQuantidadeVideos(Video video);
    }
}
