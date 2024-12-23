using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("UsuarioCurso")]
    public class UsuarioCurso
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Usuário")]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Key, Column(Order = 2)]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }
        [ForeignKey("CursoId")]
        public Curso Curso { get; set; }
    }
}