using FDevs.Models;

namespace FDevs.Services.ExclusaoService
{
    public interface IExclusaoService
    {
        string PermitirExcluirCurso(Curso curso);
        string PermitirExcluirEstado(Estado estado);
        string PermitirExcluirModulo(Modulo modulo);
        string PermitirExcluirTrilha(Trilha trilha);
        string PermitirExcluirProva(Prova prova);
        string PermitirExcluirAlternativa(Alternativa alternativa);
        string PermitirExcluirQuestao(Questao questao);
    }
}
