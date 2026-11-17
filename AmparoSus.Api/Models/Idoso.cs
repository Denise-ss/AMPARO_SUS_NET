namespace AmparoSus.Api.Models;

// ENTIDADE: representa uma pessoa idosa cadastrada no projeto.
// A propriedade Solicitacoes representa o relacionamento 1 para muitos (1:N).
public class Idoso
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;

    // Um idoso pode possuir várias solicitações de apoio.
    public ICollection<SolicitacaoApoio> Solicitacoes { get; set; } = new List<SolicitacaoApoio>();
}
