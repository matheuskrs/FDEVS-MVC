namespace FDevs.Models
{
    public class Progresso
    {
        public int? QtdCursos { get; set; }
        public int? QtdAndamento { get; set; }
        public int? QtdConcluido { get; set; }
        public int? QtdNaoIniciado { get; set; }
        public double? ProgressoVermelho { get; set; }
        public double? ProgressoAmarelo { get; set; }
        public double? ProgressoVerde { get; set; }
        public IEnumerable<UsuarioEstadoVideo> UsuarioEstadoVideos { get; set; }
        public IEnumerable<UsuarioEstadoModulo> UsuarioEstadoModulos { get; set; }
        public IEnumerable<UsuarioEstadoCurso> UsuarioEstadoCursos { get; set; }
    }
}
