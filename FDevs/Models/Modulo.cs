using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDevs.Models
{



    [Table("Modulo")]
    public class Modulo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "Informe um nome com menos de 50 caracteres.")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }
        [ForeignKey("CursoId")]
        public Curso Curso { get; set; }
        public ICollection<Video> Videos { get; set; }
        public ICollection<UsuarioEstadoModulo> UsuarioEstadoModulos { get; set; }
    }
}