using System.ComponentModel.DataAnnotations;

namespace AmparoSus.Api.DTOs;

// DTO DE ENTRADA: dados enviados pelo usuário no POST e no PUT.
// DataAnnotations ajudam o ASP.NET a rejeitar campos inválidos com HTTP 400.
public class IdosoEntrada
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "O telefone deve ter entre 8 e 20 caracteres.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(80, ErrorMessage = "A cidade deve ter no máximo 80 caracteres.")]
    public string Cidade { get; set; } = string.Empty;
}
