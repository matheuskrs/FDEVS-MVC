using FDevs.Models;

namespace FDevs.Services.EstadoVideoService
{
    public interface IEstadoVideoService
    {
        Task<UsuarioEstadoVideo> CreateUsuarioEstadoVideo(UsuarioEstadoVideo novoUsuarioEstadoVideo);
        Task<bool> Delete(UsuarioEstadoVideo usuarioEstadoVideo);
        Task<List<UsuarioEstadoVideo>> GetUsuarioEstadoVideosAsync();
        Task<List<UsuarioEstadoVideo>> GetUsuarioEstadoVideosByVideoId(int videoId);
        Task<UsuarioEstadoVideo> GetUsuarioEstadoVideosByIdAsync(string usuarioId, int videoId);
        Task<List<UsuarioEstadoVideo>> GetByUsuarioIdAsync(string usuarioId);
        Task<bool> AtualizarEstadoVideoParaAndamentoAsync(UsuarioEstadoVideo estadoVideo);
        Task<bool> AtualizarEstadoVideoParaConcluidoAsync(UsuarioEstadoVideo estadoVideo);
        Task<int> ObterQuantidadeVideosConcluidos(string usuarioId, Video video);
        Task<int> ObterQuantidadeVideos(Video video);
    }
}
