using System.ComponentModel.DataAnnotations;

namespace AmparoSus.Api.DTOs;

// DTO: garante campos obrigatórios e valores aceitos no cadastro da solicitação.
public class SolicitacaoEntrada
{
    [Range(1, int.MaxValue, ErrorMessage = "O IdosoId precisa ser maior que zero.")]
    public int IdosoId { get; set; }

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [RegularExpression("^(ORIENTACAO|ACOMPANHAMENTO|VISITA)$",
        ErrorMessage = "Use ORIENTACAO, ACOMPANHAMENTO ou VISITA.")]
    public string Tipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(250, MinimumLength = 5,
        ErrorMessage = "A descrição deve ter entre 5 e 250 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O status é obrigatório.")]
    [RegularExpression("^(PENDENTE|EM_ATENDIMENTO|CONCLUIDA)$",
        ErrorMessage = "Use PENDENTE, EM_ATENDIMENTO ou CONCLUIDA.")]
    public string Status { get; set; } = "PENDENTE";
}
