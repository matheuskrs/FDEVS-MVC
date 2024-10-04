using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Curso")]
public class Curso
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage="Informe o nome do curso.")]
    [StringLength(50, ErrorMessage="Informe um nome com menos de 50 caracteres.")]
    public string Nome { get; set; }

    [Required]
    [StringLength(500)]
    public string Foto { get; set; }
    
    public DateOnly? DataConclusao { get; set; }

    [Required(ErrorMessage="Informe uma trilha para o curso.")]
    public int TrilhaId { get; set; }
    [ForeignKey("TrilhaId")]
    public Trilha Trilha { get; set; }

    [Required(ErrorMessage="Informe o estado de conclusão inicial do curso.")]
    public int EstadoId { get; set; }
    [ForeignKey("EstadoId")]
    public Estado Estado { get; set; }
    public ICollection<Modulo> Modulos { get; set; }
    public ICollection<Prova> Provas { get; set; }

}

