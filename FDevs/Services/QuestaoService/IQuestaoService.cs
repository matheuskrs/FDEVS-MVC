using FDevs.Models;

namespace FDevs.Services.QuestaoService
{
    public interface IQuestaoService
    {
        Task<List<Questao>> GetQuestoesAsync();
        Task<Questao> GetQuestaoByIdAsync(int id);
        Task<Questao> Create(Questao questao);
        Task<Questao> Update(Questao questao);
        Task<bool> Delete(int id);
    }
}
