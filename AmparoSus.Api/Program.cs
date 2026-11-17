
using AmparoSus.Api.Data;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Configuramos os Controllers.
// Eles recebem as requisições HTTP da nossa API,
// como GET, POST, PUT e DELETE.
builder.Services.AddControllers();

// 2) Buscamos a conexão com o banco Oracle da FIAP.
// A senha fica salva nos Segredos do Usuário do Visual Studio,
// para não ser enviada ao GitHub.
var conexaoOracle = builder.Configuration.GetConnectionString("Oracle");

// Verificamos se a conexão foi configurada.
if (string.IsNullOrWhiteSpace(conexaoOracle))
{
    throw new InvalidOperationException(
        "A conexão Oracle não foi configurada. Abra o README, seção Segredos do Usuário.");
}

// 3) Configuramos o Entity Framework Core.
// Ele permite trabalhar com as tabelas do Oracle
// utilizando nossas classes C#.

// CORREÇÃO DO ERRO ORA-00904: FALSE
// Definimos compatibilidade com Oracle 19.
// Assim, o Entity Framework evita gerar expressões
// booleanas incompatíveis com versões antigas do Oracle.
builder.Services.AddDbContext<AmparoSusDbContext>(opcoes =>
    opcoes.UseOracle(conexaoOracle, oracle =>
        oracle.UseOracleSQLCompatibility(
            OracleSQLCompatibility.DatabaseVersion19)));

// 4) Construímos a aplicação.
var app = builder.Build();

// 5) Tratamos erros inesperados da API.
// Evitamos mostrar informações internas do sistema,
// como detalhes do banco de dados, para quem utiliza a API.
app.UseExceptionHandler(erroApp => erroApp.Run(async contexto =>
{
    contexto.Response.StatusCode =
        StatusCodes.Status500InternalServerError;

    contexto.Response.ContentType = "application/json";

    await contexto.Response.WriteAsJsonAsync(new
    {
        erro = "Falha interna na API. Verifique o console do Visual Studio e a conexão Oracle."
    });
}));

// 6) Criamos a rota inicial da aplicação.
// Ao acessar http://localhost:5185,
// mostramos as informações básicas do AMPARO SUS.
app.MapGet("/", () => Results.Ok(new
{
    projeto = "AMPARO SUS",
    descricao = "API acadêmica para cadastro de idosos e solicitações de apoio",
    rotas = new[]
    {
        "/api/idosos",
        "/api/solicitacoes"
    }
}));

// 7) Habilitamos as rotas dos Controllers.
// Isso permite acessar os cadastros e as solicitações.
app.MapControllers();

// 8) Iniciamos a API.
app.Run();
