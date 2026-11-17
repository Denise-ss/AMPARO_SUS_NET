using AmparoSus.Api.Models;

namespace AmparoSus.Api.DTOs;

// DTO DE SAÍDA: devolve somente os campos necessários, sem referências circulares.
public record IdosoResposta(int Id, string Nome, string Telefone, string Cidade)
{
    public static IdosoResposta DoModelo(Idoso x) =>
        new(x.Id, x.Nome, x.Telefone, x.Cidade);
}
