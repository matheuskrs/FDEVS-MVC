using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FDevs.Models;

[Table("Modulo")]
public class Modulo
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50, ErrorMessage="Informe um nome com menos de 40 caracteres.")]
    public string Nome { get; set; }

    [Required]
    [StringLength(500, ErrorMessage="Informe um link com menos de 500 caracteres.")]
    public string URL { get; set; }
    [Required]
    public int UsuarioId { get; set; }

    
}

