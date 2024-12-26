using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.AlternativaService
{
    public class AlternativaService : IAlternativaService
    {
        private readonly AppDbContext _context;

        public AlternativaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alternativa>> GetAlternativasAsync()
        {
            List<Alternativa> alternativas = await _context.Alternativas
                .Include(a => a.Questao)
                .ThenInclude(a => a.Prova)
                .ToListAsync();
            return alternativas;
        }

        public async Task<Alternativa> GetAlternativaByIdAsync(int id)
        {
            Alternativa alternativa = await _context.Alternativas
                .Include(a => a.Questao)
                .Include(a => a.Respostas)
                .SingleOrDefaultAsync(a => a.Id == id);
            return alternativa;
        }

        public async Task<Alternativa> Create(Alternativa alternativa)
        {
            _context.Add(alternativa);
            await _context.SaveChangesAsync();
            return alternativa;
        }

        public async Task<Alternativa> Update(Alternativa alternativa)
        {
            _context.Update(alternativa);
            await _context.SaveChangesAsync();
            return alternativa;
        }

        public async Task<bool> Delete(int id)
        {
            Alternativa alternativa = await GetAlternativaByIdAsync(id);
            if (alternativa == null) return false; 
            _context.Remove(alternativa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
