using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.RespostaService
{
    public class RespostaService : IRespostaService
    {
        private readonly AppDbContext _context;

        public RespostaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Resposta>> GetRespostasAsync()
        {
            List<Resposta> respostas = await _context.Respostas
                .Include(r => r.Usuario)
                .Include(r => r.Questao)
                .ThenInclude(q => q.Prova)
                .Include(r => r.Alternativa)
                .ToListAsync();
            return respostas;
        }

        public async Task<Resposta> GetRespostaByIdAsync(int id)
        {
            Resposta resposta = await _context.Respostas.SingleOrDefaultAsync(r => r.Id == id);
            return resposta;
        }

        public async Task<Resposta> Create(Resposta resposta)
        {
            _context.Add(resposta);
            await _context.SaveChangesAsync();
            return resposta;
        }

        public async Task<Resposta> Update(Resposta resposta)
        {
            _context.Update(resposta);
            await _context.SaveChangesAsync();
            return resposta;
        }

        public async Task<bool> Delete(int id)
        {
            Resposta resposta = await GetRespostaByIdAsync(id);
            if (resposta == null) return false;
            _context.Respostas.Remove(resposta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
