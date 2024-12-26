using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.EstadoCursoService
{
    public class EstadoCursoService : IEstadoCursoService
    {
        private readonly AppDbContext _context;

        public EstadoCursoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UsuarioEstadoCurso>> GetUsuarioEstadoCursosAsync()
        {
            List<UsuarioEstadoCurso> usuarioEstadoCursos = await _context.UsuarioEstadoCursos.ToListAsync();
            return usuarioEstadoCursos;
        }

        public async Task<UsuarioEstadoCurso> GetUsuarioEstadoCursoByIdAsync(string usuarioId, int estadoId, int cursoId)
        {
            UsuarioEstadoCurso usuarioEstadoCurso = await _context.UsuarioEstadoCursos
            .Include(uem => uem.Usuario)
            .Include(uem => uem.Estado)
            .Include(uem => uem.Curso)
            .FirstOrDefaultAsync(uem =>
                uem.UsuarioId == usuarioId &&
                uem.EstadoId == estadoId &&
                uem.CursoId == cursoId);

            return usuarioEstadoCurso;
        }

        public async Task<List<UsuarioEstadoCurso>> GetByUsuarioIdAsync(string usuarioId)
        {
            List<UsuarioEstadoCurso> usuarioEstadoCursos = await _context.UsuarioEstadoCursos
                .Include(uec => uec.Usuario)
                .Include(uec => uec.Estado)
                .Include(uec => uec.Curso)
                .Where(uec => uec.UsuarioId == usuarioId)
                .ToListAsync();

            return usuarioEstadoCursos;
        }
    }
}
