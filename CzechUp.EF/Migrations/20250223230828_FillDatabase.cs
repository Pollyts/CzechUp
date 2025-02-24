using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class FillDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneralTopicId",
                table: "GeneralWords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWords_GeneralTopicId",
                table: "GeneralWords",
                column: "GeneralTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralWords_GeneralTopics_GeneralTopicId",
                table: "GeneralWords",
                column: "GeneralTopicId",
                principalTable: "GeneralTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.InsertData(
                table: "LanguageLevels",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "A1" },
                    { "A2" },
                    { "B1" },
                    { "B2" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "RU" },
                    { "CZ" },
                });

            migrationBuilder.InsertData(
                table: "GeneralTopics",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Osobní údaje" },
                    { "Dům, domácnost a nejbližší okolí" },
                    { "Jídlo a pití" },
                    { "Každodenní život" },
                    { "Volný čas a zábava" },
                    { "Zdraví a péče o tělo" },
                    { "Nakupování" },
                    { "Služby" },
                    { "Cestování a doprava" },
                    { "Vzdělání" },
                    { "Okolní prostředí" },
                    { "Počasí a klima" },
                    { "Vztahy s lidmi" },
                    { "Jazyk a komunikace" }       
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralWords_GeneralTopics_GeneralTopicId",
                table: "GeneralWords");

            migrationBuilder.DropIndex(
                name: "IX_GeneralWords_GeneralTopicId",
                table: "GeneralWords");

            migrationBuilder.DropColumn(
                name: "GeneralTopicId",
                table: "GeneralWords");
        }
    }
}
