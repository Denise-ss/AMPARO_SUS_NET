using AmparoSus.Api.Models;

namespace AmparoSus.Api.DTOs;

// DTO DE SAÍDA: facilita a visualização do relacionamento no JSON.
public record SolicitacaoResposta(
    int Id, int IdosoId, string Tipo, string Descricao, string Status, DateTime CriadaEmUtc)
{
    public static SolicitacaoResposta DoModelo(SolicitacaoApoio x) =>
        new(x.Id, x.IdosoId, x.Tipo, x.Descricao, x.Status, x.CriadaEmUtc);
}
