using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTopicTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTopicUserTag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTopicUserTag",
                columns: table => new
                {
                    UserTagGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTopicGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTopicUserTag", x => new { x.UserTagGuid, x.UserTopicGuid });
                    table.ForeignKey(
                        name: "FK_UserTopicUserTag_UserTags_UserTagGuid",
                        column: x => x.UserTagGuid,
                        principalTable: "UserTags",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTopicUserTag_UserTopics_UserTopicGuid",
                        column: x => x.UserTopicGuid,
                        principalTable: "UserTopics",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTopicUserTag_UserTopicGuid",
                table: "UserTopicUserTag",
                column: "UserTopicGuid");
        }
    }
}
