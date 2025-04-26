using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LanguageLevelGuid",
                table: "GeneralExercises",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TranslatedLanguageGuid",
                table: "GeneralExercises",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExercises_LanguageLevelGuid",
                table: "GeneralExercises",
                column: "LanguageLevelGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExercises_TranslatedLanguageGuid",
                table: "GeneralExercises",
                column: "TranslatedLanguageGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralExercises_LanguageLevels_LanguageLevelGuid",
                table: "GeneralExercises",
                column: "LanguageLevelGuid",
                principalTable: "LanguageLevels",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralExercises_Languages_TranslatedLanguageGuid",
                table: "GeneralExercises",
                column: "TranslatedLanguageGuid",
                principalTable: "Languages",
                principalColumn: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralExercises_LanguageLevels_LanguageLevelGuid",
                table: "GeneralExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralExercises_Languages_TranslatedLanguageGuid",
                table: "GeneralExercises");

            migrationBuilder.DropIndex(
                name: "IX_GeneralExercises_LanguageLevelGuid",
                table: "GeneralExercises");

            migrationBuilder.DropIndex(
                name: "IX_GeneralExercises_TranslatedLanguageGuid",
                table: "GeneralExercises");

            migrationBuilder.DropColumn(
                name: "LanguageLevelGuid",
                table: "GeneralExercises");

            migrationBuilder.DropColumn(
                name: "TranslatedLanguageGuid",
                table: "GeneralExercises");
        }
    }
}
