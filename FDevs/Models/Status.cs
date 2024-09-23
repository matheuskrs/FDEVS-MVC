using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Status")]
public class Status
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50, ErrorMessage="Informe um nome com menos de 50 caracteres.")]
    public string Nome { get; set; }
    [Required]
    [StringLength(50)]
    public string Cor { get; set; }
}

