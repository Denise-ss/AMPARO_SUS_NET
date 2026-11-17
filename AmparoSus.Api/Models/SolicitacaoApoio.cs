namespace AmparoSus.Api.Models;

// ENTIDADE: registra uma solicitação de apoio criada por uma pessoa idosa.
public class SolicitacaoApoio
{
    public int Id { get; set; }

    // Chave estrangeira: identifica a qual idoso a solicitação pertence.
    public int IdosoId { get; set; }
    public Idoso Idoso { get; set; } = null!;

    public string Tipo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Status { get; set; } = "PENDENTE";
    public DateTime CriadaEmUtc { get; set; }
}
