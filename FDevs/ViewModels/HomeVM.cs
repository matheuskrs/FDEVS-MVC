using FDevs.Models;

namespace FDevs.ViewModels;

public class HomeVM
{
    public List<Curso> Cursos { get; set; }
    public List<Trilha> Trilhas { get; set; }
    public List<Usuario> Usuarios { get; set; }
    public List<UsuarioCurso> UsuarioCursos { get; set; }
    public List<Status> Status { get; set; }
    
}