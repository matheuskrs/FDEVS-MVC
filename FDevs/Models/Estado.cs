using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("Estado")]
    public class Estado
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "Informe um nome com menos de 50 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "É obrigatório informar uma cor para o estado.")]
        [StringLength(50)]
        public string Cor { get; set; }
        public ICollection<UsuarioEstadoVideo> UsuarioEstadoVideos { get; set; }
        public ICollection<UsuarioEstadoCurso> UsuarioEstadoCursos { get; set; }
        public ICollection<UsuarioEstadoModulo> UsuarioEstadoModulos { get; set; }

    }

}