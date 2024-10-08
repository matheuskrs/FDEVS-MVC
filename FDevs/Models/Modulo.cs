using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDevs.Models;

[Table("Modulo")]
public class Modulo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Informe um nome com menos de 50 caracteres.")]
    public string Nome { get; set; }

    [Required]
    [Display(Name = "Estado de conclusão")]
    public int EstadoId { get; set; }
    [ForeignKey("EstadoId")]
    public Estado Estado { get; set; }

    [Required]
    [Display(Name = "Curso")]
    public int CursoId { get; set; }
    [ForeignKey("CursoId")]
    public Curso Curso { get; set; }
    public ICollection<Video> Videos { get; set; }
}
