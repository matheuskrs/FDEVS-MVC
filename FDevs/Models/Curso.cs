using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Curso")]
public class Curso
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage="Informe um nome com menos de 50 caracteres.")]
    public string Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string Foto { get; set; }
    
    public DateTime? DataConclusao { get; set; }

    [Required]
    public int TrilhaId { get; set; }
    [ForeignKey("TrilhaId")]
    public Trilha Trilha { get; set; }

    [Required]
    public int StatusId { get; set; }
    [ForeignKey("StatusId")]
    public Status Status { get; set; }

}

