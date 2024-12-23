using FDevs.Models;
using FDevs.Services.CursoService;

namespace FDevs.Services.ExclusaoService
{
    public interface IExclusaoService
    {
        string PermitirExcluirCurso(Curso curso);
        string PermitirExcluirEstado(Estado estado);
        string PermitirExcluirModulo(Modulo modulo);
    }
}
