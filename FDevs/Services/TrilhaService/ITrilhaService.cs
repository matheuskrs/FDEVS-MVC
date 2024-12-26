using FDevs.Models;

namespace FDevs.Services.TrilhaService
{
    public interface ITrilhaService
    {
        Task<List<Trilha>> GetTrilhasAsync();
        Task<Trilha> GetTrilhaByIdAsync(int id);
        Task<List<Trilha>> GetTrilhasPorUsuarioAsync(string usuarioId);
        Task<Trilha> GetTrilhaAsNoTracking(int id);
        Task<Trilha> Create(Trilha trilha);
        Task<Trilha> Update(Trilha trilha);
        Task<bool> Delete(int id);
    }
}
