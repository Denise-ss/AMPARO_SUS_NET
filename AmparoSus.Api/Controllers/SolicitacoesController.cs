using AmparoSus.Api.Data;
using AmparoSus.Api.DTOs;
using AmparoSus.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmparoSus.Api.Controllers;

// CONTROLLER: recebe requisições para /api/solicitacoes.
[ApiController]
[Route("api/solicitacoes")]
public class SolicitacoesController : ControllerBase
{
    private readonly AmparoSusDbContext _banco;
    public SolicitacoesController(AmparoSusDbContext banco) => _banco = banco;

    // GET /api/solicitacoes -> HTTP 200.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitacaoResposta>>> Listar()
    {
        var lista = await _banco.Solicitacoes.AsNoTracking().OrderBy(x => x.Id).ToListAsync();
        return Ok(lista.Select(SolicitacaoResposta.DoModelo));
    }

    // GET /api/solicitacoes/1 -> HTTP 200 ou 404.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SolicitacaoResposta>> BuscarPorId(int id)
    {
        var solicitacao = await _banco.Solicitacoes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (solicitacao is null)
            return NotFound(new { erro = "Solicitação não encontrada." });

        return Ok(SolicitacaoResposta.DoModelo(solicitacao));
    }

    // POST /api/solicitacoes -> HTTP 201 quando o idoso cadastrado existe.
    [HttpPost]
    public async Task<ActionResult<SolicitacaoResposta>> Cadastrar([FromBody] SolicitacaoEntrada entrada)
    {
        // Não podemos criar solicitação para um idoso inexistente.
        if (!await _banco.Idosos.AnyAsync(x => x.Id == entrada.IdosoId))
            return BadRequest(new { erro = "O IdosoId informado não existe no cadastro." });

        if (entrada.Descricao.Trim().Length < 5)
            return BadRequest(new { erro = "Descrição deve conter pelo menos 5 caracteres úteis." });

        var solicitacao = new SolicitacaoApoio
        {
            IdosoId = entrada.IdosoId,
            Tipo = entrada.Tipo,
            Descricao = entrada.Descricao.Trim(),
            Status = entrada.Status,
            CriadaEmUtc = DateTime.UtcNow
        };

        _banco.Solicitacoes.Add(solicitacao);
        await _banco.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = solicitacao.Id },
            SolicitacaoResposta.DoModelo(solicitacao));
    }

    // PUT /api/solicitacoes/1 -> HTTP 204.
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] SolicitacaoEntrada entrada)
    {
        var solicitacao = await _banco.Solicitacoes.FindAsync(id);
        if (solicitacao is null)
            return NotFound(new { erro = "Solicitação não encontrada." });

        if (!await _banco.Idosos.AnyAsync(x => x.Id == entrada.IdosoId))
            return BadRequest(new { erro = "O IdosoId informado não existe no cadastro." });

        if (entrada.Descricao.Trim().Length < 5)
            return BadRequest(new { erro = "Descrição deve conter pelo menos 5 caracteres úteis." });

        solicitacao.IdosoId = entrada.IdosoId;
        solicitacao.Tipo = entrada.Tipo;
        solicitacao.Descricao = entrada.Descricao.Trim();
        solicitacao.Status = entrada.Status;
        // A data original de criação é mantida quando uma solicitação é alterada.
        await _banco.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/solicitacoes/1 -> HTTP 204.
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var solicitacao = await _banco.Solicitacoes.FindAsync(id);
        if (solicitacao is null)
            return NotFound(new { erro = "Solicitação não encontrada." });

        _banco.Solicitacoes.Remove(solicitacao);
        await _banco.SaveChangesAsync();
        return NoContent();
    }
}
