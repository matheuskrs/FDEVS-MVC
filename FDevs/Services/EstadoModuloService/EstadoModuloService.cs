using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<UsuarioEstadoModulo> GetUsuarioEstadoModuloByIdAsync(string usuarioId, int estadoId, int moduloId)
        {
            UsuarioEstadoModulo usuarioEstadoModulo = await _context.UsuarioEstadoModulos
                .Include(uem => uem.Usuario)
                .Include(uem => uem.Estado)
                .Include(uem => uem.Modulo)
                .FirstOrDefaultAsync(uem => 
                    uem.UsuarioId == usuarioId && 
                    uem.EstadoId == estadoId && 
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

    }
}
