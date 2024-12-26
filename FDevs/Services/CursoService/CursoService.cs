using FDevs.Data;
using FDevs.Models;
using FDevs.Services.ExclusaoService;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.CursoService
{


    public class CursoService : ICursoService
    {
        private readonly AppDbContext _context;

        public CursoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Curso>> GetCursosAsync()
        {
            var cursos = await _context.Cursos
                .Include(c => c.Modulos)
                .Include(c => c.Trilha)
                .Include(c => c.Provas)
                .Include(c => c.UsuarioEstadoCursos)
                .ToListAsync();
            return cursos;
        }

        public async Task<Curso> GetCursoByIdAsync(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Modulos)
                .Include(c => c.Trilha)
                .Include(c => c.Provas)
                .Include(c => c.UsuarioEstadoCursos)
                .SingleOrDefaultAsync(c => c.Id == id);
            return curso;
        }

        public async Task<Curso> GetCursoAsNoTracking(int id)
        {
            var curso = await _context.Cursos.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            return curso;
        }

        public async Task<Curso> Create(Curso curso)
        {
            await _context.AddAsync(curso);
            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task<bool> Delete(int id)
        {
            Curso curso = await GetCursoByIdAsync(id);
            if (curso == null) return false;
            _context.Remove(curso);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Curso> Update(Curso curso)
        {
            _context.Update(curso);
            await _context.SaveChangesAsync();
            return curso;
        }
    }

}