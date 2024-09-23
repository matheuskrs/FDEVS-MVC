using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Resposta")]
public class Resposta
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    [ForeignKey("Usuario")]
    public Usuario Usuario { get; set; }

    [Required]
    public bool QuestaoId { get; set; }
    [ForeignKey("QuestaoId")]
    public Questao Questao { get; set; }
    
    [Required]
    public int AlternativaId { get; set; }
    [ForeignKey("AlternativaId")]
    public Alternativa Alternativa { get; set; }
}

