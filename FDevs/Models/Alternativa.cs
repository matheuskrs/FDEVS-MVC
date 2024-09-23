using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Alternativa")]
public class Alternativa
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(500, ErrorMessage="Informe um enunciado com menos de 500 caracteres.")]
    public string Texto { get; set; }

    [Required]
    public bool QuestaoId { get; set; }
    [ForeignKey("QuestaoId")]
    public Questao Questao { get; set; }
    
}

