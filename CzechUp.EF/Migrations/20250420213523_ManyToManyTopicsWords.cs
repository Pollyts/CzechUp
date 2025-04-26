using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyTopicsWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicGuid",
                table: "GeneralOriginalWords");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOriginalWords_UserTopics_UserTopicGuid",
                table: "UserOriginalWords");

            migrationBuilder.DropIndex(
                name: "IX_UserOriginalWords_UserTopicGuid",
                table: "UserOriginalWords");

            migrationBuilder.DropIndex(
                name: "IX_GeneralOriginalWords_GeneralTopicGuid",
                table: "GeneralOriginalWords");

            migrationBuilder.DropColumn(
                name: "UserTopicGuid",
                table: "UserOriginalWords");

            migrationBuilder.DropColumn(
                name: "GeneralTopicGuid",
                table: "GeneralOriginalWords");

            migrationBuilder.CreateTable(
                name: "GeneralOriginalWordGeneralTopics",
                columns: table => new
                {
                    GeneralOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneralTopicGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralOriginalWordGeneralTopics", x => new { x.GeneralOriginalWordGuid, x.GeneralTopicGuid });
                    table.ForeignKey(
                        name: "FK_GeneralOriginalWordGeneralTopics_GeneralOriginalWords_Gener~",
                        column: x => x.GeneralOriginalWordGuid,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralOriginalWordGeneralTopics_GeneralTopics_GeneralTopic~",
                        column: x => x.GeneralTopicGuid,
                        principalTable: "GeneralTopics",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOriginalWordUserTopic",
                columns: table => new
                {
                    UserOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTopicGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOriginalWordUserTopic", x => new { x.UserOriginalWordGuid, x.UserTopicGuid });
                    table.ForeignKey(
                        name: "FK_UserOriginalWordUserTopic_UserOriginalWords_UserOriginalWor~",
                        column: x => x.UserOriginalWordGuid,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOriginalWordUserTopic_UserTopics_UserTopicGuid",
                        column: x => x.UserTopicGuid,
                        principalTable: "UserTopics",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWordGeneralTopics_GeneralTopicGuid",
                table: "GeneralOriginalWordGeneralTopics",
                column: "GeneralTopicGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWordUserTopic_UserTopicGuid",
                table: "UserOriginalWordUserTopic",
                column: "UserTopicGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralOriginalWordGeneralTopics");

            migrationBuilder.DropTable(
                name: "UserOriginalWordUserTopic");

            migrationBuilder.AddColumn<Guid>(
                name: "UserTopicGuid",
                table: "UserOriginalWords",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GeneralTopicGuid",
                table: "GeneralOriginalWords",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_UserTopicGuid",
                table: "UserOriginalWords",
                column: "UserTopicGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWords_GeneralTopicGuid",
                table: "GeneralOriginalWords",
                column: "GeneralTopicGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicGuid",
                table: "GeneralOriginalWords",
                column: "GeneralTopicGuid",
                principalTable: "GeneralTopics",
                principalColumn: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOriginalWords_UserTopics_UserTopicGuid",
                table: "UserOriginalWords",
                column: "UserTopicGuid",
                principalTable: "UserTopics",
                principalColumn: "Guid");
        }
    }
}
