using FDevs.Models;

namespace FDevs.ViewModels;

public class HomeVM
{
    // public List<Curso> Cursos { get; set; }
    public List<Trilha> Trilhas { get; set; }
    public List<Usuario> Usuarios { get; set; }
    public List<UsuarioCurso> Cursos { get; set; }
    public List<Estado> Estado { get; set; }
}