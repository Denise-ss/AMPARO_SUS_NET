using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmparoSus.Api.Migrations
{
    /// <inheritdoc />
    public partial class InicialAmparoSus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AMP_IDOSOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CIDADE = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMP_IDOSOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AMP_SOLICITACOES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IDOSO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    CRIADA_EM_UTC = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMP_SOLICITACOES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AMP_SOLICITACOES_AMP_IDOSOS_IDOSO_ID",
                        column: x => x.IDOSO_ID,
                        principalTable: "AMP_IDOSOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AMP_SOLICITACOES_IDOSO_ID",
                table: "AMP_SOLICITACOES",
                column: "IDOSO_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AMP_SOLICITACOES");

            migrationBuilder.DropTable(
                name: "AMP_IDOSOS");
        }
    }
}
