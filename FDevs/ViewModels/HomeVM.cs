using FDevs.Models;

namespace FDevs.ViewModels
{


    public class HomeVM
    {
        public List<UsuarioCurso> Cursos { get; set; }
        public List<Trilha> Trilhas { get; set; }
        public Progresso Progresso { get; set; }
        public List<Estado> Estados { get; set; }
        public List<Video> Videos { get; set; }
    }
}