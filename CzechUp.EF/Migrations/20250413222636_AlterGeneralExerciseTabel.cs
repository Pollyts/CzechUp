using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class AlterGeneralExerciseTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GeneralOriginalWordGuid",
                table: "GeneralExercises",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GeneralTopicGuid",
                table: "GeneralExercises",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExercises_GeneralOriginalWordGuid",
                table: "GeneralExercises",
                column: "GeneralOriginalWordGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExercises_GeneralTopicGuid",
                table: "GeneralExercises",
                column: "GeneralTopicGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralExercises_GeneralOriginalWords_GeneralOriginalWordGu~",
                table: "GeneralExercises",
                column: "GeneralOriginalWordGuid",
                principalTable: "GeneralOriginalWords",
                principalColumn: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralExercises_GeneralTopics_GeneralTopicGuid",
                table: "GeneralExercises",
                column: "GeneralTopicGuid",
                principalTable: "GeneralTopics",
                principalColumn: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralExercises_GeneralOriginalWords_GeneralOriginalWordGu~",
                table: "GeneralExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralExercises_GeneralTopics_GeneralTopicGuid",
                table: "GeneralExercises");

            migrationBuilder.DropIndex(
                name: "IX_GeneralExercises_GeneralOriginalWordGuid",
                table: "GeneralExercises");

            migrationBuilder.DropIndex(
                name: "IX_GeneralExercises_GeneralTopicGuid",
                table: "GeneralExercises");

            migrationBuilder.DropColumn(
                name: "GeneralOriginalWordGuid",
                table: "GeneralExercises");

            migrationBuilder.DropColumn(
                name: "GeneralTopicGuid",
                table: "GeneralExercises");
        }
    }
}
