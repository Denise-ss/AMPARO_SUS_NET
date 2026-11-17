using AmparoSus.Api.Data;
using AmparoSus.Api.DTOs;
using AmparoSus.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmparoSus.Api.Controllers;

// CONTROLLER: recebe as requisições para /api/idosos.
[ApiController]
[Route("api/idosos")]
public class IdososController : ControllerBase
{
    private readonly AmparoSusDbContext _banco;

    // O ASP.NET entrega o DbContext já configurado (injeção de dependência).
    public IdososController(AmparoSusDbContext banco) => _banco = banco;

    // GET /api/idosos -> HTTP 200 e lista.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IdosoResposta>>> Listar()
    {
        var idosos = await _banco.Idosos.AsNoTracking().OrderBy(x => x.Id).ToListAsync();
        return Ok(idosos.Select(IdosoResposta.DoModelo));
    }

    // GET /api/idosos/1 -> HTTP 200 ou HTTP 404.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IdosoResposta>> BuscarPorId(int id)
    {
        var idoso = await _banco.Idosos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (idoso is null)
            return NotFound(new { erro = "Idoso não encontrado." });

        return Ok(IdosoResposta.DoModelo(idoso));
    }

    // POST /api/idosos -> grava no Oracle e retorna HTTP 201.
    [HttpPost]
    public async Task<ActionResult<IdosoResposta>> Cadastrar([FromBody] IdosoEntrada entrada)
    {
        if (entrada.Nome.Trim().Length < 3 || string.IsNullOrWhiteSpace(entrada.Cidade))
            return BadRequest(new { erro = "Informe nome com ao menos 3 letras e cidade válida." });

        var idoso = new Idoso
        {
            Nome = entrada.Nome.Trim(),
            Telefone = entrada.Telefone.Trim(),
            Cidade = entrada.Cidade.Trim()
        };

        _banco.Idosos.Add(idoso);
        await _banco.SaveChangesAsync(); // EF Core transforma a inclusão em INSERT no Oracle.

        return CreatedAtAction(nameof(BuscarPorId), new { id = idoso.Id },
            IdosoResposta.DoModelo(idoso));
    }

    // PUT /api/idosos/1 -> altera o registro e retorna HTTP 204.
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] IdosoEntrada entrada)
    {
        if (entrada.Nome.Trim().Length < 3 || string.IsNullOrWhiteSpace(entrada.Cidade))
            return BadRequest(new { erro = "Informe nome com ao menos 3 letras e cidade válida." });

        var idoso = await _banco.Idosos.FindAsync(id);
        if (idoso is null)
            return NotFound(new { erro = "Idoso não encontrado." });

        idoso.Nome = entrada.Nome.Trim();
        idoso.Telefone = entrada.Telefone.Trim();
        idoso.Cidade = entrada.Cidade.Trim();
        await _banco.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/idosos/1 -> remove o registro e retorna HTTP 204.
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var idoso = await _banco.Idosos.FindAsync(id);
        if (idoso is null)
            return NotFound(new { erro = "Idoso não encontrado." });

        _banco.Idosos.Remove(idoso);
        await _banco.SaveChangesAsync();
        return NoContent();
    }
}
