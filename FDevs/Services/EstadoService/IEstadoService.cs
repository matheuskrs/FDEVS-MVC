using FDevs.Models;

namespace FDevs.Services.EstadoService
{
    public interface IEstadoService
    {
        Task<List<Estado>> GetEstadosAsync();
        Task<Estado> GetEstadoByIdAsync(int id);
        Task<Estado> Create(Estado estado);
        Task<Estado> Update(Estado estado);
        Task<bool> Delete(int id);
    }
}