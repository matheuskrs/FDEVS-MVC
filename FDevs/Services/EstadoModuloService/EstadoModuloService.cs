using FDevs.Data;
using FDevs.Models;
using FDevs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDevs.Services.EstadoModuloService
{
    public class EstadoModuloService : IEstadoModuloService
    {
        private readonly AppDbContext _context;

        public EstadoModuloService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UsuarioEstadoModulo>> GetUsuarioEstadoModulosAsync()
        {
            List<UsuarioEstadoModulo> usuarioEstadoModulos = await _context.UsuarioEstadoModulos.ToListAsync();
            return usuarioEstadoModulos;
        }

        public async Task<UsuarioEstadoModulo> GetUsuarioEstadoModuloByIdAsync(string usuarioId, int moduloId)
        {
            UsuarioEstadoModulo usuarioEstadoModulo = await _context.UsuarioEstadoModulos
                .Include(uem => uem.Usuario)
                .Include(uem => uem.Estado)
                .Include(uem => uem.Modulo)
                .FirstOrDefaultAsync(uem =>
                    uem.UsuarioId == usuarioId &&
                    uem.ModuloId == moduloId);

            return usuarioEstadoModulo;
        }

        public async Task<List<UsuarioEstadoModulo>> GetByUsuarioIdAsync(string usuarioId)
        {
            List<UsuarioEstadoModulo> usuarioEstadoModulos = await _context.UsuarioEstadoModulos
                .Include(uem => uem.Usuario)
                .Include(uem => uem.Estado)
                .Include(uem => uem.Modulo)
                .Where(uem => uem.UsuarioId == usuarioId)
                .ToListAsync();

            return usuarioEstadoModulos;
        }

        public async Task<UsuarioEstadoModulo> AtualizarEstadoModulo(UsuarioEstadoModulo usuarioEstadoAntigo)
        {
            if(usuarioEstadoAntigo.EstadoId != 3) return null;
            _context.Remove(usuarioEstadoAntigo);
            await _context.SaveChangesAsync();

            var novoUsuarioEstadoModulo = new UsuarioEstadoModulo
            {
                UsuarioId = usuarioEstadoAntigo.UsuarioId,
                ModuloId = usuarioEstadoAntigo.ModuloId,
                EstadoId = 1
            };

            _context.Add(novoUsuarioEstadoModulo);
            await _context.SaveChangesAsync();
            return novoUsuarioEstadoModulo;
        }

        public async Task<UsuarioEstadoModulo> AtualizarEstadoModuloParaConcluido(UsuarioEstadoModulo usuarioEstadoAntigo)
        {
            _context.Remove(usuarioEstadoAntigo);
            await _context.SaveChangesAsync();

            var novoUsuarioEstadoModulo = new UsuarioEstadoModulo
            {
                UsuarioId = usuarioEstadoAntigo.UsuarioId,
                ModuloId = usuarioEstadoAntigo.ModuloId,
                EstadoId = 2
            };

            _context.Add(novoUsuarioEstadoModulo);
            await _context.SaveChangesAsync();
            return novoUsuarioEstadoModulo;
        }

        public async Task<int> ObterQuantidadeModulosConcluidos(string usuarioId, Modulo modulo)
        {
            return await _context.UsuarioEstadoModulos
                .CountAsync(uem => uem.UsuarioId == usuarioId && uem.EstadoId == 2 && uem.Modulo.CursoId == modulo.CursoId);
        }

        public async Task<int> ObterQuantidadeModulos(Modulo modulo)
        {
            return await _context.Modulos.CountAsync(m => m.CursoId == modulo.CursoId);
        }
    }
}
