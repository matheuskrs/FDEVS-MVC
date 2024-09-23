using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Usuario")]
public class Usuario
{
    [Key]
    public string UsuarioId { get; set; }
    [Required]
    [StringLength(60, ErrorMessage="Informe um nome com menos de 60 caracteres.")]
    public string Nome { get; set; }
}

