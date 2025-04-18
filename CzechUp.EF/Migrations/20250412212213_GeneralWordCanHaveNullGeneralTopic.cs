using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class GeneralWordCanHaveNullGeneralTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicGuid",
                table: "GeneralOriginalWords");

            migrationBuilder.AlterColumn<Guid>(
                name: "GeneralTopicGuid",
                table: "GeneralOriginalWords",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicGuid",
                table: "GeneralOriginalWords",
                column: "GeneralTopicGuid",
                principalTable: "GeneralTopics",
                principalColumn: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicGuid",
                table: "GeneralOriginalWords");

            migrationBuilder.AlterColumn<Guid>(
                name: "GeneralTopicGuid",
                table: "GeneralOriginalWords",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicGuid",
                table: "GeneralOriginalWords",
                column: "GeneralTopicGuid",
                principalTable: "GeneralTopics",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
