using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddingGeneralWordId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WordNumber",
                table: "GeneralWordForms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GeneralOriginalWordId",
                table: "GeneralWordExamples",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OriginalExample",
                table: "GeneralWordExamples",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TranslatedExample",
                table: "GeneralWordExamples",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordExamples_GeneralOriginalWordId",
                table: "GeneralWordExamples",
                column: "GeneralOriginalWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralWordExamples_GeneralOriginalWords_GeneralOriginalWor~",
                table: "GeneralWordExamples",
                column: "GeneralOriginalWordId",
                principalTable: "GeneralOriginalWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralWordExamples_GeneralOriginalWords_GeneralOriginalWor~",
                table: "GeneralWordExamples");

            migrationBuilder.DropIndex(
                name: "IX_GeneralWordExamples_GeneralOriginalWordId",
                table: "GeneralWordExamples");

            migrationBuilder.DropColumn(
                name: "WordNumber",
                table: "GeneralWordForms");

            migrationBuilder.DropColumn(
                name: "GeneralOriginalWordId",
                table: "GeneralWordExamples");

            migrationBuilder.DropColumn(
                name: "OriginalExample",
                table: "GeneralWordExamples");

            migrationBuilder.DropColumn(
                name: "TranslatedExample",
                table: "GeneralWordExamples");
        }
    }
}
