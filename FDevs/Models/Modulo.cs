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
    public string UsuarioId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }
    [Required]
    public int StatusId { get; set; }
    [ForeignKey("StatusId")]
    public Status Status { get; set; }

    [Required]
    public int CursoId { get; set; }
    [ForeignKey("CursoId")]
    public Curso Curso { get; set; }
    public ICollection<Video> Videos { get; set; }
}
