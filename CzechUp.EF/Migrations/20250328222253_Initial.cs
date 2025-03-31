using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExerciseType = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    AnswerOptions = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralExercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralTopics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                });

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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    TranslatedLanguageId = table.Column<int>(type: "integer", nullable: false),
                    RequiredLanguageLevelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_LanguageLevels_RequiredLanguageLevelId",
                        column: x => x.RequiredLanguageLevelId,
                        principalTable: "LanguageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Languages_TranslatedLanguageId",
                        column: x => x.TranslatedLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralWordExamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneralOriginalWordId = table.Column<int>(type: "integer", nullable: false),
                    OriginalExample = table.Column<string>(type: "text", nullable: false),
                    TranslatedExample = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordExamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralWordExamples_GeneralOriginalWords_GeneralOriginalWor~",
                        column: x => x.GeneralOriginalWordId,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralWordForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordNumber = table.Column<int>(type: "integer", nullable: false),
                    WordForm = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    GeneralOriginalWordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralWordForms_GeneralOriginalWords_GeneralOriginalWordId",
                        column: x => x.GeneralOriginalWordId,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "UserExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneralExerciseId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CorrectAnswerCount = table.Column<int>(type: "integer", nullable: false),
                    WrongAnswerCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExercises_GeneralExercises_GeneralExerciseId",
                        column: x => x.GeneralExerciseId,
                        principalTable: "GeneralExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExercises_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRuleNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRuleNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRuleNotes_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRuleNotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TagType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GeneralTopicId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTopics_GeneralTopics_GeneralTopicId",
                        column: x => x.GeneralTopicId,
                        principalTable: "GeneralTopics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTopics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRuleUserTag",
                columns: table => new
                {
                    UserRuleId = table.Column<int>(type: "integer", nullable: false),
                    UserTagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRuleUserTag", x => new { x.UserRuleId, x.UserTagId });
                    table.ForeignKey(
                        name: "FK_UserRuleUserTag_Rules_UserRuleId",
                        column: x => x.UserRuleId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRuleUserTag_UserTags_UserTagId",
                        column: x => x.UserTagId,
                        principalTable: "UserTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOriginalWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Word = table.Column<string>(type: "text", nullable: false),
                    LanguageLevelId = table.Column<int>(type: "integer", nullable: true),
                    UserTopicId = table.Column<int>(type: "integer", nullable: true),
                    GeneralOriginalWordId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOriginalWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_GeneralOriginalWords_GeneralOriginalWordId",
                        column: x => x.GeneralOriginalWordId,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Id");
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
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOriginalWordUserTag",
                columns: table => new
                {
                    UserOriginalWordId = table.Column<int>(type: "integer", nullable: false),
                    UserTagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOriginalWordUserTag", x => new { x.UserOriginalWordId, x.UserTagId });
                    table.ForeignKey(
                        name: "FK_UserOriginalWordUserTag_UserOriginalWords_UserOriginalWordId",
                        column: x => x.UserOriginalWordId,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOriginalWordUserTag_UserTags_UserTagId",
                        column: x => x.UserTagId,
                        principalTable: "UserTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordExamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserOriginalWordId = table.Column<int>(type: "integer", nullable: false),
                    OriginalExample = table.Column<string>(type: "text", nullable: false),
                    TranslatedExample = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordExamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWordExamples_UserOriginalWords_UserOriginalWordId",
                        column: x => x.UserOriginalWordId,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordForm = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    UserOriginalWordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWordForms_UserOriginalWords_UserOriginalWordId",
                        column: x => x.UserOriginalWordId,
                        principalTable: "UserOriginalWords",
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
                    UserOriginalWordId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WasLearned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWordTranslations_UserOriginalWords_UserOriginalWordId",
                        column: x => x.UserOriginalWordId,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWordTranslations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWords_GeneralTopicId",
                table: "GeneralOriginalWords",
                column: "GeneralTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWords_LanguageLevelId",
                table: "GeneralOriginalWords",
                column: "LanguageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordExamples_GeneralOriginalWordId",
                table: "GeneralWordExamples",
                column: "GeneralOriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordForms_GeneralOriginalWordId",
                table: "GeneralWordForms",
                column: "GeneralOriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordTranslations_GeneralOriginalWordId",
                table: "GeneralWordTranslations",
                column: "GeneralOriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordTranslations_LanguageId",
                table: "GeneralWordTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_GeneralExerciseId",
                table: "UserExercises",
                column: "GeneralExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_UserId",
                table: "UserExercises",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_GeneralOriginalWordId",
                table: "UserOriginalWords",
                column: "GeneralOriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_LanguageLevelId",
                table: "UserOriginalWords",
                column: "LanguageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_UserId",
                table: "UserOriginalWords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_UserTopicId",
                table: "UserOriginalWords",
                column: "UserTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWordUserTag_UserTagId",
                table: "UserOriginalWordUserTag",
                column: "UserTagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRuleNotes_RuleId",
                table: "UserRuleNotes",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRuleNotes_UserId",
                table: "UserRuleNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRuleUserTag_UserTagId",
                table: "UserRuleUserTag",
                column: "UserTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RequiredLanguageLevelId",
                table: "Users",
                column: "RequiredLanguageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TranslatedLanguageId",
                table: "Users",
                column: "TranslatedLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTags_UserId",
                table: "UserTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_GeneralTopicId",
                table: "UserTopics",
                column: "GeneralTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_UserId",
                table: "UserTopics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordExamples_UserOriginalWordId",
                table: "UserWordExamples",
                column: "UserOriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordForms_UserOriginalWordId",
                table: "UserWordForms",
                column: "UserOriginalWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordTranslations_UserId",
                table: "UserWordTranslations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordTranslations_UserOriginalWordId",
                table: "UserWordTranslations",
                column: "UserOriginalWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralWordExamples");

            migrationBuilder.DropTable(
                name: "GeneralWordForms");

            migrationBuilder.DropTable(
                name: "GeneralWordTranslations");

            migrationBuilder.DropTable(
                name: "UserExercises");

            migrationBuilder.DropTable(
                name: "UserOriginalWordUserTag");

            migrationBuilder.DropTable(
                name: "UserRuleNotes");

            migrationBuilder.DropTable(
                name: "UserRuleUserTag");

            migrationBuilder.DropTable(
                name: "UserWordExamples");

            migrationBuilder.DropTable(
                name: "UserWordForms");

            migrationBuilder.DropTable(
                name: "UserWordTranslations");

            migrationBuilder.DropTable(
                name: "GeneralExercises");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "UserTags");

            migrationBuilder.DropTable(
                name: "UserOriginalWords");

            migrationBuilder.DropTable(
                name: "GeneralOriginalWords");

            migrationBuilder.DropTable(
                name: "UserTopics");

            migrationBuilder.DropTable(
                name: "GeneralTopics");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LanguageLevels");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
