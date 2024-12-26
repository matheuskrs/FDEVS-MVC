using FDevs.Models;

namespace FDevs.Services.CursoService
{
    public interface ICursoService
    {
        Task<List<Curso>> GetCursosAsync();
        Task<Curso> GetCursoByIdAsync(int id);
        Task<Curso> GetCursoAsNoTracking(int id);
        Task<Curso> Create(Curso curso);
        Task<Curso> Update(Curso curso);
        Task<bool> Delete(int id);
    }
}