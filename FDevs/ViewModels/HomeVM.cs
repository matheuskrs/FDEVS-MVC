using FDevs.Models;

namespace FDevs.ViewModels;

public class HomeVM
{
    public List<Trilha> Trilhas { get; set; }
    public List<Usuario> Usuarios { get; set; }
    public List<UsuarioCurso> Cursos { get; set; }
    public List<UsuarioEstadoVideo> UsuarioEstadoVideos { get; set; }
}