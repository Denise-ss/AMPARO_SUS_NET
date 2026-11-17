-- AMPARO SUS | CONSULTAS DE VERIFICACAO NO ORACLE SQL DEVELOPER
-- Nossa API cria e altera registros; aqui usamos apenas SELECT para conferir.
-- O Entity Framework Core cria a estrutura do banco pelas Migrations.
-- Ctrl + Enter executa a consulta e mostra o resultado em formato de tabela.

-- TESTE 01 - Ver quais tabelas do AMPARO SUS existem no usuario Oracle.
SELECT table_name
FROM user_tables
WHERE table_name IN ('AMP_IDOSOS', 'AMP_SOLICITACOES')
ORDER BY table_name;

-- TESTE 02 - Consultar um idoso cadastrado pela API.
-- Substitua o numero 1 pelo ID devolvido no POST de cadastro.
SELECT ID, NOME, TELEFONE, CIDADE
FROM AMP_IDOSOS
WHERE ID = 1;

-- TESTE 03 - Ver solicitacoes junto dos dados dos idosos.
-- INNER JOIN une as duas tabelas quando I.ID = S.IDOSO_ID.
SELECT
    I.ID AS ID_IDOSO,
    I.NOME AS NOME_IDOSO,
    S.ID AS ID_SOLICITACAO,
    S.TIPO,
    S.STATUS
FROM AMP_IDOSOS I
INNER JOIN AMP_SOLICITACOES S
    ON I.ID = S.IDOSO_ID
ORDER BY I.ID, S.ID;

-- TESTE 04 - Consultar todas as solicitacoes do idoso de ID 1.
-- No registro da nossa demonstracao, essa consulta retornou duas linhas
-- antes da exclusao usada para exemplificar a operacao DELETE.
SELECT
    I.ID AS ID_IDOSO,
    I.NOME AS NOME_IDOSO,
    S.ID AS ID_SOLICITACAO,
    S.TIPO,
    S.STATUS
FROM AMP_IDOSOS I
INNER JOIN AMP_SOLICITACOES S
    ON I.ID = S.IDOSO_ID
WHERE I.ID = 1
ORDER BY S.ID;
