using FDevs.Models;

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
            if (estado == null) return "Não foi possível encontrar o estado.";
            var usuarioEstadoCurso = estado.UsuarioEstadoCursos != null && estado.UsuarioEstadoCursos.Any();
            var usuarioEstadoModulo = estado.UsuarioEstadoModulos != null && estado.UsuarioEstadoModulos.Any();
            var usuarioEstadoVideo = estado.UsuarioEstadoVideos != null && estado.UsuarioEstadoVideos.Any();


            if (usuarioEstadoCurso || usuarioEstadoModulo || usuarioEstadoVideo)
                return $"O estado \"{estado.Nome}\" não pode ser excluído pois já existem registros na(s) tabela(s): \"{(usuarioEstadoCurso ? "UsuarioEstadoCursos" : "")}  {(usuarioEstadoModulo ? "UsuarioEstadoModulos" : "")} {(usuarioEstadoVideo ? "UsuarioEstadoVideos" : "")}\" associados a ele!";

            return null;
        }

        public string PermitirExcluirModulo(Modulo modulo)
        {
            if (modulo == null) return "Não foi possível encontrar o estado.";

            var videosDoModulo = modulo.Videos.Any();

            if (videosDoModulo)
                return $"O módulo \"{modulo.Nome}\" não pode ser excluído pois já existem registros na tabela: \"{(videosDoModulo ? "VÍDEOS" : "")}\" associados a ele!";

            return null;
        }
        
        public string PermitirExcluirTrilha(Trilha trilha)
        {
            if (trilha == null) return "Não foi possível encontrar a trilha.";

            var cursosDaTrilha = trilha.Cursos.Any();
            if (cursosDaTrilha)
                return $"A trilha \"{trilha.Nome}\" não pode ser excluída, pois já existem registros na tabela: \"CURSOS\" associados a ele!";

            return null;
        }

        public string PermitirExcluirProva(Prova prova)
        {
            if (prova == null) return "Não foi possível encontrar a prova.";

            var questoesDaProva = prova.Questoes.Any();

            if (questoesDaProva)
                return $"A Prova \"{prova.Nome}\" não pode ser excluída pois já existem registros na tabela: \"{(questoesDaProva ? "QUESTÕES" : "")}\" associados a ela!";

            return null;
        }

        public string PermitirExcluirAlternativa(Alternativa alternativa)
        {
            if (alternativa == null) return "Não foi possível encontrar a alternativa.";

            var respostas = alternativa.Respostas.Any();

            if (respostas)
                return $"A alternativa não pode ser excluída pois já existem registros na tabela: \"RESPOSTAS\" associados a ela!";

            return null;
        }

        public string PermitirExcluirQuestao(Questao questao)
        {
            if (questao == null) return "Não foi possível encontrar a questão.";

            var alternativasDaQuestao = questao.Alternativas.Any();

            if (alternativasDaQuestao)
                return $"A questão não pode ser excluída pois já existem registros na tabela: \"{(alternativasDaQuestao ? "ALTERNATIVAS" : "")}\"associados a ele!";

            return null;
        }
    }
}
