using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{



    [Table("Video")]
    public class Video
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(60, ErrorMessage = "Informe um título com menos de 60 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O link é obrigatório.")]
        [StringLength(700, ErrorMessage = "Informe um link com menos de 700 caracteres.")]
        [Display(Name = "URL (Embed)")]
        public string URL { get; set; }
        [Required]
        [Display(Name = "Módulo")]
        public int ModuloId { get; set; }
        [ForeignKey("ModuloId")]
        public Modulo Modulo { get; set; }
        public ICollection<UsuarioEstadoVideo> UsuarioEstadoVideos { get; set; }
    }

}