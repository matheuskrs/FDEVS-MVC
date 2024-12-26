using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.TrilhaService
{
    public class TrilhaService : ITrilhaService
    {
        private readonly AppDbContext _context;

        public TrilhaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trilha>> GetTrilhasAsync()
        {
            List<Trilha> trilhas = await _context.Trilhas
                .ToListAsync();
            return trilhas;
        }

        public async Task<Trilha> GetTrilhaByIdAsync(int id)
        {
            Trilha trilha = await _context.Trilhas
                .Include(t => t.Cursos)
                .SingleOrDefaultAsync(c => c.Id == id);
            return trilha;
        }
        public async Task<List<Trilha>> GetTrilhasPorUsuarioAsync(string usuarioId)
        {
            var trilhas = await _context.Trilhas
            .Where(t => _context.Cursos
                .Any(c => c.TrilhaId == t.Id &&
                          _context.UsuarioCursos.Any(uc => uc.CursoId == c.Id && uc.UsuarioId == usuarioId)))
            .ToListAsync();
            return trilhas;
        }

        public async Task<Trilha> GetTrilhaAsNoTracking(int id)
        {
            Trilha trilha = await _context.Trilhas
                .Include(t => t.Cursos)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);
            return trilha;
        }

        public async Task<Trilha> Create(Trilha trilha)
        {
            if (trilha == null) throw new Exception("Ocorreu um erro ao tentar criar a trilha, tente novamente.");
            _context.Add(trilha);
            await _context.SaveChangesAsync();
            return trilha;
        }

        public async Task<Trilha> Update(Trilha trilha)
        {
            if (trilha == null) throw new Exception("Ocorreu um erro ao tentar atualizar a trilha, tente novamente.");

            _context.Update(trilha);
            await _context.SaveChangesAsync();
            return trilha;
        }

        public async Task<bool> Delete(int id)
        {
            Trilha trilha = await GetTrilhaByIdAsync(id);
            if (trilha == null) return false;
            _context.Remove(trilha);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
