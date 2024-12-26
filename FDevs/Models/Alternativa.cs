using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("Alternativa")]
    public class Alternativa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe um texto para a alternativa.")]
        [StringLength(500, ErrorMessage = "Informe um enunciado com menos de 500 caracteres.")]
        public string Texto { get; set; }
        [Required(ErrorMessage = "Informe se a alternativa é a correta ou não.")]
        public bool Correta { get; set; }

        [Required(ErrorMessage = "Informe qual a questão da alternativa.")]
        [DisplayName("Questão")]
        public int QuestaoId { get; set; }
        [ForeignKey("QuestaoId")]
        public Questao Questao { get; set; }
        public ICollection<Resposta> Respostas { get; set; }
    }
}