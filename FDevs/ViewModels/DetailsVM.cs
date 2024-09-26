using FDevs.Models;

namespace FDevs.ViewModels;

public class DetailsVM
{

    public Curso Curso { get; set; }
    public Curso Atual { get; set; }
    public List<Video> Videos { get; set; }
    public List<Modulo> Modulos { get; set; }
    public int? SelectedVideoId { get; set; }
}