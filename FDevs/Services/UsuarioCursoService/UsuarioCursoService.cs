using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.UsuarioCursoService
{
    public class UsuarioCursoService : IUsuarioCursoService
    {
        private readonly AppDbContext _context;

        public UsuarioCursoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UsuarioCurso>> GetCursosPorUsuarioAsync(string usuarioId)
        {
            var cursos = await _context.UsuarioCursos
                .Include(uc => uc.Usuario)
                .Include(uc => uc.Curso)
                .ThenInclude(c => c.Modulos)
                .ThenInclude(m => m.Videos)
                .Where(uc => uc.UsuarioId == usuarioId)
                .ToListAsync();
            return cursos;
        }

        public async Task<List<UsuarioCurso>> GetCursosPorCursoId(int cursoId)
        {
            var cursos = await _context.UsuarioCursos
                .Include(uc => uc.Curso)
                .ThenInclude(c => c.Modulos)
                .Where(uc => uc.CursoId == cursoId)
                .ToListAsync();
            return cursos;
        }

        public async Task<List<UsuarioCurso>> GetUsuarioCursosAsync()
        {
            var usuarioCursos = await _context.UsuarioCursos
                .Include(uc => uc.Usuario)
                .Include(uc => uc.Curso)
                .ToListAsync();
            return usuarioCursos;
        }

        public async Task<UsuarioCurso> GetCursoPorUsuarioCursoAsync(string usuarioId, int cursoId)
        {
            var usuarioCurso = await _context.UsuarioCursos
                .Include(uc => uc.Usuario)
                .Include(uc => uc.Curso)
                .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CursoId == cursoId);
            return usuarioCurso;
        }
    }
}
