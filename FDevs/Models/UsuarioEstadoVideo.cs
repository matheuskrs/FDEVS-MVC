using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models
{


    [Table("UsuarioEstadoVideo")]
    public class UsuarioEstadoVideo
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Usuário")]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Key, Column(Order = 2)]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public Estado Estado { get; set; }

        [Key, Column(Order = 3)]
        [Display(Name = "Vídeo")]
        public int VideoId { get; set; }
        [ForeignKey("VideoId")]
        public Video Video { get; set; }
    }
}