using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("Curso")]
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do curso.")]
        [StringLength(50, ErrorMessage = "Informe um nome com menos de 50 caracteres.")]
        public string Nome { get; set; }

        [StringLength(500)]
        public string Foto { get; set; }

        [Required(ErrorMessage = "Informe uma trilha para o curso.")]
        [Display(Name = "Trilha")]
        public int TrilhaId { get; set; }
        [ForeignKey("TrilhaId")]
        public Trilha Trilha { get; set; }

        public ICollection<Modulo> Modulos { get; set; }
        public ICollection<Prova> Provas { get; set; }
        public ICollection<UsuarioEstadoCurso> UsuarioEstadoCursos { get; set; }

    }

}