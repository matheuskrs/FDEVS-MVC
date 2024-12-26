using FDevs.Models;

namespace FDevs.Services.ProvaService
{
    public interface IProvaService
    {
        Task<List<Prova>> GetProvasAsync();
        Task<Prova> GetProvaByIdAsync(int id);
        Task<Prova> Create(Prova prova);
        Task<Prova> Update(Prova prova);
        Task<bool> Delete(int id);
    }
}
