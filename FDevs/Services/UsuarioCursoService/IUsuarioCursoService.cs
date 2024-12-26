using FDevs.Models;

namespace FDevs.Services.UsuarioCursoService
{
    public interface IUsuarioCursoService
    {
        public Task<List<UsuarioCurso>> GetUsuarioCursosAsync();
        public Task<List<UsuarioCurso>> GetCursosPorUsuarioAsync(string usuarioId);
    }
}
