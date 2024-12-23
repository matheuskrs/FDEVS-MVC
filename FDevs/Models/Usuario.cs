using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace FDevs.Models
{


    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]

        public IdentityUser ContaUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [StringLength(300)]
        public string Foto { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<UsuarioCurso> Cursos { get; set; }
        public ICollection<Modulo> Modulos { get; set; }
        public ICollection<Resposta> Respostas { get; set; }
        public ICollection<UsuarioEstadoVideo> UsuarioEstadoVideos { get; set; }
        public ICollection<UsuarioEstadoCurso> UsuarioEstadoCursos { get; set; }
    }
}