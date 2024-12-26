using FDevs.Models;
using System.Runtime.CompilerServices;

namespace FDevs.Services.ProgressoService
{
    public interface IProgressoService
    {
        Task<Progresso> ObterProgressoAsync(string usuarioId);
        Task<bool> UpdateProgressoVideoParaAndamentoAsync(UsuarioEstadoVideo estadoVideo);
    }
}
