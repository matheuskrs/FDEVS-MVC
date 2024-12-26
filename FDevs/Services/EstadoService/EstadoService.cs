using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.EstadoService
{


    public class EstadoService : IEstadoService
    {
        private readonly AppDbContext _context;

        public EstadoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estado>> GetEstadosAsync()
        {
            var estados = await _context.Estados
                .ToListAsync();
            return estados;
        }

        public async Task<Estado> GetEstadoByIdAsync(int id)
        {
            var estado = await _context.Estados
                .Include(e => e.UsuarioEstadoCursos)
                .Include(e => e.UsuarioEstadoVideos)
                .Include(e => e.UsuarioEstadoModulos)
                .SingleOrDefaultAsync(e => e.Id == id);
            return estado;
        }

        public async Task<Estado> Create(Estado estado)
        {
            await _context.AddAsync(estado);
            await _context.SaveChangesAsync();
            return estado;
        }

        public async Task<Estado> Update(Estado estado)
        {
            _context.Update(estado);
            await _context.SaveChangesAsync();
            return estado;
        }

        public async Task<bool> Delete(int id)
        {
            Estado estado = await GetEstadoByIdAsync(id);
            if (estado == null) return false;
            _context.Remove(estado);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}