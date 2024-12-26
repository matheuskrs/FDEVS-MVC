using FDevs.Models;

namespace FDevs.Services.AlternativaService
{
    public interface IAlternativaService
    {
        Task<List<Alternativa>> GetAlternativasAsync();
        Task<Alternativa> GetAlternativaByIdAsync(int id);
        Task<Alternativa> Create(Alternativa alternativa);
        Task<Alternativa> Update(Alternativa alternativa);
        Task<bool> Delete(int id);
    }
}
