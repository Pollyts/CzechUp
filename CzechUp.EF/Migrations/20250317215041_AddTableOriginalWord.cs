using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddTableOriginalWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralWordForms_GeneralWords_GeneralWordId",
                table: "GeneralWordForms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWordForms_UserWords_UserWordId",
                table: "UserWordForms");

            migrationBuilder.DropTable(
                name: "UserWords");

            migrationBuilder.DropTable(
                name: "GeneralWords");

            migrationBuilder.DropIndex(
                name: "IX_UserWordForms_UserWordId",
                table: "UserWordForms");

            migrationBuilder.RenameColumn(
                name: "GeneralWordId",
                table: "GeneralWordForms",
                newName: "OriginalWordId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralWordForms_GeneralWordId",
                table: "GeneralWordForms",
                newName: "IX_GeneralWordForms_OriginalWordId");

            migrationBuilder.AddColumn<int>(
                name: "OriginalWordId",
                table: "UserWordForms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WordForm",
                table: "UserWordForms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordForm",
                table: "GeneralWordForms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GeneralOriginalWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Word = table.Column<string>(type: "text", nullable: false),
                    LanguageLevelId = table.Column<int>(type: "integer", nullable: false),
                    GeneralTopicId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralOriginalWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicId",
                        column: x => x.GeneralTopicId,
                        principalTable: "GeneralTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralOriginalWords_LanguageLevels_LanguageLevelId",
                        column: x => x.LanguageLevelId,
                        principalTable: "LanguageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralWordExamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordExamples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOriginalWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Word = table.Column<string>(type: "text", nullable: false),
                    LanguageLevelId = table.Column<int>(type: "integer", nullable: true),
                    UserTopicId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOriginalWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_LanguageLevels_LanguageLevelId",
                        column: x => x.LanguageLevelId,
                        principalTable: "LanguageLevels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_UserTopics_UserTopicId",
                        column: x => x.UserTopicId,
                        principalTable: "UserTopics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserWordExamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordExamples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralWordTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Translation = table.Column<string>(type: "text", nullable: false),
                    GeneralOriginalWordId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralWordTranslations_GeneralOriginalWords_GeneralOrigina~",
                        column: x => x.GeneralOriginalWordId,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralWordTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Translation = table.Column<string>(type: "text", nullable: false),
                    UserOriginalWordId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TopicId = table.Column<int>(type: "integer", nullable: true),
                    OriginalWordId = table.Column<int>(type: "integer", nullable: true),
                    WasLearned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWordTranslations_UserOriginalWords_OriginalWordId",
                        column: x => x.OriginalWordId,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWordTranslations_UserTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "UserTopics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWordTranslations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWordForms_OriginalWordId",
                table: "UserWordForms",
                column: "OriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWords_GeneralTopicId",
                table: "GeneralOriginalWords",
                column: "GeneralTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWords_LanguageLevelId",
                table: "GeneralOriginalWords",
                column: "LanguageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordTranslations_GeneralOriginalWordId",
                table: "GeneralWordTranslations",
                column: "GeneralOriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordTranslations_LanguageId",
                table: "GeneralWordTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_LanguageLevelId",
                table: "UserOriginalWords",
                column: "LanguageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_UserTopicId",
                table: "UserOriginalWords",
                column: "UserTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordTranslations_OriginalWordId",
                table: "UserWordTranslations",
                column: "OriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordTranslations_TopicId",
                table: "UserWordTranslations",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordTranslations_UserId",
                table: "UserWordTranslations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralWordForms_GeneralOriginalWords_OriginalWordId",
                table: "GeneralWordForms",
                column: "OriginalWordId",
                principalTable: "GeneralOriginalWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWordForms_UserOriginalWords_OriginalWordId",
                table: "UserWordForms",
                column: "OriginalWordId",
                principalTable: "UserOriginalWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralWordForms_GeneralOriginalWords_OriginalWordId",
                table: "GeneralWordForms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWordForms_UserOriginalWords_OriginalWordId",
                table: "UserWordForms");

            migrationBuilder.DropTable(
                name: "GeneralWordExamples");

            migrationBuilder.DropTable(
                name: "GeneralWordTranslations");

            migrationBuilder.DropTable(
                name: "UserWordExamples");

            migrationBuilder.DropTable(
                name: "UserWordTranslations");

            migrationBuilder.DropTable(
                name: "GeneralOriginalWords");

            migrationBuilder.DropTable(
                name: "UserOriginalWords");

            migrationBuilder.DropIndex(
                name: "IX_UserWordForms_OriginalWordId",
                table: "UserWordForms");

            migrationBuilder.DropColumn(
                name: "OriginalWordId",
                table: "UserWordForms");

            migrationBuilder.DropColumn(
                name: "WordForm",
                table: "UserWordForms");

            migrationBuilder.DropColumn(
                name: "WordForm",
                table: "GeneralWordForms");

            migrationBuilder.RenameColumn(
                name: "OriginalWordId",
                table: "GeneralWordForms",
                newName: "GeneralWordId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralWordForms_OriginalWordId",
                table: "GeneralWordForms",
                newName: "IX_GeneralWordForms_GeneralWordId");

            migrationBuilder.CreateTable(
                name: "GeneralWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneralTopicId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    LanguageLevelId = table.Column<int>(type: "integer", nullable: false),
                    Original = table.Column<string>(type: "text", nullable: false),
                    Translation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralWords_GeneralTopics_GeneralTopicId",
                        column: x => x.GeneralTopicId,
                        principalTable: "GeneralTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralWords_LanguageLevels_LanguageLevelId",
                        column: x => x.LanguageLevelId,
                        principalTable: "LanguageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralWords_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneralWordId = table.Column<int>(type: "integer", nullable: true),
                    TopicId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WasLearned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWords_GeneralWords_GeneralWordId",
                        column: x => x.GeneralWordId,
                        principalTable: "GeneralWords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWords_UserTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "UserTopics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWordForms_UserWordId",
                table: "UserWordForms",
                column: "UserWordId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWords_GeneralTopicId",
                table: "GeneralWords",
                column: "GeneralTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWords_LanguageId",
                table: "GeneralWords",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWords_LanguageLevelId",
                table: "GeneralWords",
                column: "LanguageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWords_GeneralWordId",
                table: "UserWords",
                column: "GeneralWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWords_TopicId",
                table: "UserWords",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWords_UserId",
                table: "UserWords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralWordForms_GeneralWords_GeneralWordId",
                table: "GeneralWordForms",
                column: "GeneralWordId",
                principalTable: "GeneralWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWordForms_UserWords_UserWordId",
                table: "UserWordForms",
                column: "UserWordId",
                principalTable: "UserWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
