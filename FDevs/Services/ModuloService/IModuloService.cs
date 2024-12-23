using FDevs.Models;

namespace FDevs.Services.ModuloService
{
    public interface IModuloService
    {
        Task<List<Modulo>> GetModulosAsync();
        Task<Modulo> GetModuloByIdAsync(int id);
        Task<Modulo> Create(Modulo modulo);
        Task<Modulo> Update(Modulo modulo);
        Task<bool> Delete(int id);
    }
}
