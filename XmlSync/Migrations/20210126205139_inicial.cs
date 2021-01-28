using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XmlSync.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XML_INFOs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NOME_XML = table.Column<string>(nullable: true),
                    CONTEUDO = table.Column<string>(nullable: true),
                    DATA_CRIAÇÃO = table.Column<DateTime>(nullable: false),
                    DATA_SINCRONIZACAO = table.Column<DateTime>(nullable: false),
                    SINCRONIZADO = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XML_INFOs", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XML_INFOs");
        }
    }
}
