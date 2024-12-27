using FDevs.Models;

namespace FDevs.Services.EstadoCursoService
{
    public interface IEstadoCursoService
    {
        Task<List<UsuarioEstadoCurso>> GetUsuarioEstadoCursosAsync();
        Task<UsuarioEstadoCurso> GetUsuarioEstadoCursoByIdAsync(string usuarioId, int cursoId);
        Task<List<UsuarioEstadoCurso>> GetByUsuarioIdAsync(string usuarioId);
        Task<UsuarioEstadoCurso> AtualizarEstadoCurso(UsuarioEstadoCurso usuarioEstadoAntigo);
        Task<UsuarioEstadoCurso> AtualizarEstadoCursoParaConcluido(UsuarioEstadoCurso usuarioEstadoAntigo);

    }
}
