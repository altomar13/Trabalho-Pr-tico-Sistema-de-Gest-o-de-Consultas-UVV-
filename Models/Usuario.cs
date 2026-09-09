using System.ComponentModel.DataAnnotations;

namespace SistemaGestaoConsultasUVV.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 120 caracteres.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(160)]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    // Armazena o hash da senha, nunca a senha em texto puro.
    [Required]
    public string Senha { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Data de Cadastro")]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
