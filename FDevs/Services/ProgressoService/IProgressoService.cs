using FDevs.Models;

namespace FDevs.Services.ProgressoService
{
    public interface IProgressoService
    {
        Task<Progresso> ObterProgressoAsync(string usuarioId);

        Task<Progresso> CalcularProgressosEmPorcentagemAsync(Progresso progresso, string usuarioId);
    }
}
