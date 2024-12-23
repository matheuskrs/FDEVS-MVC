using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("Trilha")]
    public class Trilha
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "Informe um nome com menos de 50 caracteres.")]
        public string Nome { get; set; }
        [StringLength(500)]
        public string Foto { get; set; }
        public ICollection<Curso> Cursos { get; set; }
    }

}