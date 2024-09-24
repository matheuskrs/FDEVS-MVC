using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Video")]
public class Video
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50, ErrorMessage="Informe um nome com menos de 40 caracteres.")]
    public string Titulo { get; set; }

    [Required]
    [StringLength(500, ErrorMessage="Informe um link com menos de 500 caracteres.")]
    public string URL { get; set; }
    [Required]
    public int ModuloId { get; set; }
    [ForeignKey("ModuloId")]
    public Modulo Modulo { get; set; }
    [Required]
    public int StatusId { get; set; }
    [ForeignKey("StatusId")]
    public Status Status { get; set; }
}

