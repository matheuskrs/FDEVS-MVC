using FDevs.Models;

namespace FDevs.Services.UsuarioCursoService
{
    public interface IUsuarioCursoService
    {
        public Task<List<UsuarioCurso>> GetUsuarioCursosAsync();
        public Task<List<UsuarioCurso>> GetCursosPorUsuarioAsync(string usuarioId);
        public Task<UsuarioCurso> GetCursoPorUsuarioCursoAsync(string usuarioId, int cursoId);
        public Task<List<UsuarioCurso>> GetCursosPorCursoId(int cursoId);
    }
}
