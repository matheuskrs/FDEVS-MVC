using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("Resposta")]
    public class Resposta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Required]
        [Display(Name = "Questão")]
        public int QuestaoId { get; set; }
        [ForeignKey("QuestaoId")]
        public Questao Questao { get; set; }

        [Required(ErrorMessage = "Você deve obrigatoriamente escolher uma alternativa.")]
        [Display(Name = "Alternativa")]
        public int AlternativaId { get; set; }
        [ForeignKey("AlternativaId")]
        public Alternativa Alternativa { get; set; }

    }

}