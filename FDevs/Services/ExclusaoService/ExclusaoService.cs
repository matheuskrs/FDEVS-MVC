
using FDevs.Data;
using FDevs.Models;
using FDevs.Services.CursoService;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.ExclusaoService
{
    public class ExclusaoService : IExclusaoService
    {
        public string PermitirExcluirCurso(Curso curso)
        {
            if (curso == null) return "Não foi possível encontrar o curso.";
            var usuarioCursos = curso.UsuarioEstadoCursos != null && curso.UsuarioEstadoCursos.Any();

            var modulosCurso = curso.Modulos.Any();
            if (usuarioCursos || modulosCurso)
                return $"O curso \"{curso.Nome}\" não pode ser excluído pois já existem registros nas tabelas: {(usuarioCursos ? "USUÁRIOS" : "")} {(modulosCurso ? "MÓDULOS" : "")}.";
            
            return null;
        }

        public string PermitirExcluirEstado(Estado estado)
        {
            var usuarioEstadoCurso = estado.UsuarioEstadoCursos != null && estado.UsuarioEstadoVideos.Any();
            var usuarioEstadoModulo = estado.UsuarioEstadoModulos != null && estado.UsuarioEstadoModulos.Any();
            var usuarioEstadoVideo = estado.UsuarioEstadoVideos != null && estado.UsuarioEstadoCursos.Any();

            if (estado == null) return "Não foi possível encontrar o estado.";

            if (usuarioEstadoCurso || usuarioEstadoModulo || usuarioEstadoVideo)
                return $"O estado \"{estado.Nome}\" não pode ser excluído pois já existem registros na(s) tabela(s): \"{(usuarioEstadoCurso ? "UsuarioEstadoCursos" : "")}  {(usuarioEstadoModulo ? "UsuarioEstadoModulos" : "")} {(usuarioEstadoVideo ? "UsuarioEstadoVideos" : "")}\" associados a ele!";

            return null;
        }

        public string PermitirExcluirModulo(Modulo modulo)
        {
            var videosDoModulo = modulo.Videos.Any();
            if (modulo == null) return "Não foi possível encontrar o estado.";

            if (videosDoModulo)
                return $"O módulo \"{modulo.Nome}\" não pode ser excluído pois já existem registros na tabela: \"{(videosDoModulo ? "VÍDEOS" : "")}\" associados a ele!";

            return null;
        }
    }
}
