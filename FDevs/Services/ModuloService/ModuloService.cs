using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.ModuloService
{
    public class ModuloService : IModuloService
    {
        private readonly AppDbContext _context;

        public ModuloService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Modulo>> GetModulosAsync()
        {
            var modulos = await _context.Modulos
                .Include(m => m.Curso)
                .ToListAsync();
            return modulos;
        }

        public async Task<Modulo> GetModuloByIdAsync(int id)
        {
            Modulo modulo = await _context.Modulos
                .Include(m => m.Curso)
                .Include(m => m.Videos)
                .Include(m => m.UsuarioEstadoModulos)
                .SingleOrDefaultAsync(m => m.Id == id);
            return modulo;
        }

        public async Task<Modulo> Create(Modulo modulo)
        {
            await _context.AddAsync(modulo);
            await _context.SaveChangesAsync();
            return modulo;
        }

        public async Task<Modulo> Update(Modulo modulo)
        {
            _context.Update(modulo);
            await _context.SaveChangesAsync();
            return modulo;
        }

        public async Task<bool> Delete(int id)
        {
            Modulo modulo = await _context.Modulos.FirstOrDefaultAsync(m => m.Id == id);
            if (modulo == null) return false;

            _context.Remove(modulo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
