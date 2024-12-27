using FDevs.Models;

namespace FDevs.Services.EstadoModuloService
{
    public interface IEstadoModuloService
    {
        Task<List<UsuarioEstadoModulo>> GetUsuarioEstadoModulosAsync();
        Task<UsuarioEstadoModulo> GetUsuarioEstadoModuloByIdAsync(string usuarioId, int moduloId);
        Task<List<UsuarioEstadoModulo>> GetByUsuarioIdAsync(string usuarioId);
        Task<UsuarioEstadoModulo> AtualizarEstadoModulo(UsuarioEstadoModulo usuarioEstadoAntigo);
        Task<UsuarioEstadoModulo> AtualizarEstadoModuloParaConcluido(UsuarioEstadoModulo usuarioEstadoAntigo);
        Task<int> ObterQuantidadeModulosConcluidos(string usuarioId, Modulo modulo);
        Task<int> ObterQuantidadeModulos(Modulo modulo);

    }
}
