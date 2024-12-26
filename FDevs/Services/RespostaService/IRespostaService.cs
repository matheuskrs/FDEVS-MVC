using FDevs.Models;

namespace FDevs.Services.RespostaService
{
    public interface IRespostaService
    {
        Task<List<Resposta>> GetRespostasAsync();
        Task<Resposta> GetRespostaByIdAsync(int id);
        Task<Resposta> Create(Resposta resposta);
        Task<Resposta> Update(Resposta resposta);
        Task<bool> Delete(int id);
    }
}
