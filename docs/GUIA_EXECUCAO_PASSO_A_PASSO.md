# GUIA DE EXECUÇÃO — AMPARO SUS

Este guia acompanha nosso README. Ele mostra como executar o projeto, desde a abertura do Visual Studio até a confirmação de dados no Oracle.

## 1. Abra a solução

No Visual Studio, use **Arquivo > Abrir > Projeto/Solução** e escolha `AMPARO_SUS_NET.sln`. Na coluna da direita aparecerá `AmparoSus.Api`, com as pastas Controllers, Data, DTOs, Models e Migrations.

## 2. Compile o código

Use **Compilação > Compilar Solução** (`Ctrl + Shift + B`). Depois selecione **Exibir > Saída > Compilação** para ver o resultado. Compilar significa transformar as classes C# em um programa executável.

## 3. Configure o banco

No Oracle SQL Developer, abra sua conexão da FIAP. Em **Propriedades**, veja host, porta e SID/serviço. Em nosso ambiente de testes, os dados de rede foram `oracle.fiap.com.br`, porta `1521` e SID `ORCL`.

No Visual Studio, botão direito em `AmparoSus.Api` > **Gerenciar Segredos do Usuário**. No `secrets.json` local, informe a string de conexão do README, usando suas próprias credenciais. Salve. **O `secrets.json` não é copiado para o GitHub.**

## 4. Prepare as tabelas

Abra **Ferramentas > Gerenciador de Pacotes NuGet > Console do Gerenciador de Pacotes**. No campo **Projeto padrão**, escolha `AmparoSus.Api`. Execute `Update-Database -Context AmparoSusDbContext`. Esse comando aplica as Migrations do projeto ao Oracle.

No SQL Developer, expanda **Tabelas** e procure `AMP_IDOSOS` e `AMP_SOLICITACOES`. Você também pode executar a consulta SQL 01.

## 5. Inicie a API

Selecione o perfil `http` na barra do Visual Studio e clique no botão verde. Acesse `http://localhost:5185/` no navegador. A resposta inicial mostra o nome AMPARO SUS e as rotas disponíveis.

## 6. Envie uma requisição de cadastro

Abra `AmparoSus.Api.http`. No bloco POST de idosos, clique em **Enviar solicitação**. O painel à direita mostrará `201 Created` e um novo número `id`. Esse número identifica a pessoa no banco.

## 7. Consulte no Oracle

No SQL Developer, abra a planilha SQL e execute o SELECT do cadastro (`Ctrl + Enter`). Ele deverá mostrar o mesmo ID e os dados registrados pela API. Não inserimos dados manualmente pelo SQL Developer; usamos a API para cadastrar e o SELECT para consultar.

## 8. Crie uma solicitação ligada ao idoso

No bloco POST de solicitações, informe no campo `idosoId` o ID recebido no cadastro do idoso. Envie a requisição. A API salva a solicitação, devolve `201 Created` e informa o ID do novo pedido.

## 9. Confira a relação 1:N

Execute o `INNER JOIN` do arquivo `AMPARO_SUS_Consultas_Oracle.sql`. Ele combina a tabela dos idosos com a das solicitações. Se registrarmos dois pedidos para o mesmo idoso, veremos o mesmo `ID_IDOSO` em duas linhas, cada uma com um `ID_SOLICITACAO` diferente.

## 10. Entenda o CRUD

CRUD é o nome das quatro operações básicas: **Create (criar), Read (ler), Update (atualizar) e Delete (excluir)**. No nosso projeto usamos POST, GET, PUT e DELETE para realizar essas operações. Temos um GET para listar e outro para buscar por ID, então são cinco rotas por entidade.

## 11. Entenda os códigos HTTP

`200` significa que a consulta funcionou. `201` confirma um novo cadastro. `204` significa que a atualização/exclusão funcionou e não devolveu corpo. `400` mostra que a validação recusou dados que não seguem as regras. `404` mostra que um ID consultado não existe. Os dois últimos também são comportamentos esperados quando fazemos testes intencionais de validação.

## 12. Como ler o código

- `Program.cs`: prepara o aplicativo, registra os Controllers e lê a conexão com o Oracle.
- `Idoso.cs` e `SolicitacaoApoio.cs`: representam os dados que trabalhamos.
- `AmparoSusDbContext.cs`: descreve a ligação entre as classes C# e as tabelas.
- `IdososController.cs` e `SolicitacoesController.cs`: recebem requisições e retornam respostas HTTP.
- `DTOs`: definem os dados aceitos e devolvidos, com as validações de entrada.
- `Migrations`: guardam as versões da estrutura do banco de dados.
- `AmparoSus.Api.http`: reúne as chamadas que usamos para testar a API.

Nossas imagens e os resultados observados estão registrados no relatório PDF dentro da pasta `docs`.
