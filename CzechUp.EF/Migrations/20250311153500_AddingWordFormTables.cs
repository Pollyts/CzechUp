using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddingWordFormTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralWordForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneralWordId = table.Column<int>(type: "integer", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralWordForms_GeneralWords_GeneralWordId",
                        column: x => x.GeneralWordId,
                        principalTable: "GeneralWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserWordId = table.Column<int>(type: "integer", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWordForms_UserWords_UserWordId",
                        column: x => x.UserWordId,
                        principalTable: "UserWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordForms_GeneralWordId",
                table: "GeneralWordForms",
                column: "GeneralWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordForms_UserWordId",
                table: "UserWordForms",
                column: "UserWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralWordForms");

            migrationBuilder.DropTable(
                name: "UserWordForms");
        }
    }
}
