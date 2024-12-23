using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("Questao")]
    public class Questao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(500, ErrorMessage = "Informe um enunciado com menos de 500 caracteres.")]
        public string Texto { get; set; }

        [Required]
        [Display(Name = "Nome da Prova")]
        public int ProvaId { get; set; }
        [ForeignKey("ProvaId")]
        public Prova Prova { get; set; }

        public ICollection<Alternativa> Alternativas { get; set; }
        public ICollection<Resposta> Respostas { get; set; }
    }

}