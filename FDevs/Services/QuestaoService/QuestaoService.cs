using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.QuestaoService
{
    public class QuestaoService : IQuestaoService
    {
        private readonly AppDbContext _context;

        public QuestaoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Questao>> GetQuestoesAsync()
        {
            List<Questao> questoes = await _context.Questoes.ToListAsync();
            return questoes;
        }
        public async Task<Questao> GetQuestaoByIdAsync(int id)
        {
            Questao questao = await _context.Questoes
                .Include(q => q.Alternativas)
                .FirstOrDefaultAsync(q => q.Id == id);
            return questao;
        }

        public async Task<Questao> Create(Questao questao)
        {
            _context.Add(questao);
            await _context.SaveChangesAsync();
            return questao;
        }

        public async Task<Questao> Update(Questao questao)
        {
            _context.Update(questao);
            await _context.SaveChangesAsync();
            return questao;
        }

        public async Task<bool> Delete(int id)
        {
            Questao questao = await GetQuestaoByIdAsync(id);
            if (questao == null) return false;
            _context.Remove(questao);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
