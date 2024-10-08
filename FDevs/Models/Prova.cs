using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Prova")]
public class Prova
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Informe um nome com menos de 50 caracteres.")]
    public string Nome { get; set; }

    [Required]
    public int CursoId { get; set; }
    [ForeignKey("CursoId")]
    public Curso Curso { get; set; }
    public ICollection<Questao> Questoes { get; set; }
}

