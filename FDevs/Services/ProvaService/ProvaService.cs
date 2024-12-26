using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.ProvaService
{
    public class ProvaService : IProvaService
    {
        private readonly AppDbContext _context;

        public ProvaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Prova>> GetProvasAsync()
        {
            List<Prova> provas = await _context.Provas
                .Include(p => p.Curso)
                .ToListAsync();
            return provas;
        }
        public async Task<Prova> GetProvaByIdAsync(int id)
        {
            Prova prova = await _context.Provas
                .Include(p => p.Curso)
                .Include(p => p.Questoes)
                .SingleOrDefaultAsync(p => p.Id == id);
            return prova;
        }

        public async Task<Prova> Create(Prova prova)
        {
            _context.Provas.Add(prova);
            await _context.SaveChangesAsync();
            return prova;
        }
        public async Task<Prova> Update(Prova prova)
        {
            _context.Provas.Update(prova);
            await _context.SaveChangesAsync();
            return prova;
        }

        public async Task<bool> Delete(int id)
        {
            Prova prova = await GetProvaByIdAsync(id);
            if (prova == null) return false;
            _context.Provas.Remove(prova);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
