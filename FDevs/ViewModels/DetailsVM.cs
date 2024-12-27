using FDevs.Models;

namespace FDevs.ViewModels
{


    public class DetailsVM
    {
        public Curso CursoAtual { get; set; }
        public Video VideoAtual { get; set; }
        public Video ProximoVideo { get; set; }
        public Video VideoAnterior { get; set; }
        public List<Video> Videos { get; set; }
        public IEnumerable<UsuarioEstadoVideo> UsuarioEstadoVideos { get; set; }
        public IEnumerable<UsuarioEstadoModulo> UsuarioEstadoModulos { get; set; }
        public List<Modulo> Modulos { get; set; }
        public int SelectedVideoId { get; set; }
        public int? QuestaoId { get; set; }
    }
}