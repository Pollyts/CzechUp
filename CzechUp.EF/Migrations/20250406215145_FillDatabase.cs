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
            migrationBuilder.InsertData(
    table: "Languages",
    columns: new[] { "Name" },
    values: new object[,]
    {
{ "RU" },
{ "CZ" },
{ "EN" },
    });

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


            migrationBuilder.InsertData(
                table: "TagTypes",
                columns: new[] { "TagTypeEnum" },
                values: new object[,]
                {
{ "0" },
{ "1" },
{ "2" },
{ "3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
