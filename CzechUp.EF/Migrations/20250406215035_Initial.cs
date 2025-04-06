using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"pgcrypto\";");


            migrationBuilder.CreateTable(
                name: "GeneralExercises",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExerciseType = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    AnswerOptions = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralExercises", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "GeneralTopics",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralTopics", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "LanguageLevels",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageLevels", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "TagTypes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    TagTypeEnum = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTypes", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "GeneralOriginalWords",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Word = table.Column<string>(type: "text", nullable: false),
                    LanguageLevelGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneralTopicGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralOriginalWords", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_GeneralOriginalWords_GeneralTopics_GeneralTopicGuid",
                        column: x => x.GeneralTopicGuid,
                        principalTable: "GeneralTopics",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralOriginalWords_LanguageLevels_LanguageLevelGuid",
                        column: x => x.LanguageLevelGuid,
                        principalTable: "LanguageLevels",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    TranslatedLanguageGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    RequiredLanguageLevelGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Users_LanguageLevels_RequiredLanguageLevelGuid",
                        column: x => x.RequiredLanguageLevelGuid,
                        principalTable: "LanguageLevels",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Languages_TranslatedLanguageGuid",
                        column: x => x.TranslatedLanguageGuid,
                        principalTable: "Languages",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralWordExamples",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    GeneralOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalExample = table.Column<string>(type: "text", nullable: false),
                    TranslatedExample = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordExamples", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_GeneralWordExamples_GeneralOriginalWords_GeneralOriginalWor~",
                        column: x => x.GeneralOriginalWordGuid,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralWordExamples_Languages_LanguageGuid",
                        column: x => x.LanguageGuid,
                        principalTable: "Languages",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralWordForms",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WordNumber = table.Column<int>(type: "integer", nullable: false),
                    WordForm = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    GeneralOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordForms", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_GeneralWordForms_GeneralOriginalWords_GeneralOriginalWordGu~",
                        column: x => x.GeneralOriginalWordGuid,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralWordTranslations",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Translation = table.Column<string>(type: "text", nullable: false),
                    GeneralOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralWordTranslations", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_GeneralWordTranslations_GeneralOriginalWords_GeneralOrigina~",
                        column: x => x.GeneralOriginalWordGuid,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralWordTranslations_Languages_LanguageGuid",
                        column: x => x.LanguageGuid,
                        principalTable: "Languages",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExercises",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    GeneralExerciseGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrectAnswerCount = table.Column<int>(type: "integer", nullable: false),
                    WrongAnswerCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExercises", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserExercises_GeneralExercises_GeneralExerciseGuid",
                        column: x => x.GeneralExerciseGuid,
                        principalTable: "GeneralExercises",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExercises_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRuleNotes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    RuleGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRuleNotes", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserRuleNotes_Rules_RuleGuid",
                        column: x => x.RuleGuid,
                        principalTable: "Rules",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRuleNotes_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTags",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTags", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserTags_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTopics",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneralTopicGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTopics", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserTopics_GeneralTopics_GeneralTopicGuid",
                        column: x => x.GeneralTopicGuid,
                        principalTable: "GeneralTopics",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_UserTopics_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExerciseUserTag",
                columns: table => new
                {
                    UserExerciseGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTagGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExerciseUserTag", x => new { x.UserExerciseGuid, x.UserTagGuid });
                    table.ForeignKey(
                        name: "FK_UserExerciseUserTag_UserExercises_UserExerciseGuid",
                        column: x => x.UserExerciseGuid,
                        principalTable: "UserExercises",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExerciseUserTag_UserTags_UserTagGuid",
                        column: x => x.UserTagGuid,
                        principalTable: "UserTags",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRuleUserTag",
                columns: table => new
                {
                    UserRuleGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTagGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRuleUserTag", x => new { x.UserRuleGuid, x.UserTagGuid });
                    table.ForeignKey(
                        name: "FK_UserRuleUserTag_Rules_UserRuleGuid",
                        column: x => x.UserRuleGuid,
                        principalTable: "Rules",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRuleUserTag_UserTags_UserTagGuid",
                        column: x => x.UserTagGuid,
                        principalTable: "UserTags",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTagTagTypes",
                columns: table => new
                {
                    TagTypeGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTagGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTagTagTypes", x => new { x.TagTypeGuid, x.UserTagGuid });
                    table.ForeignKey(
                        name: "FK_UserTagTagTypes_TagTypes_TagTypeGuid",
                        column: x => x.TagTypeGuid,
                        principalTable: "TagTypes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTagTagTypes_UserTags_UserTagGuid",
                        column: x => x.UserTagGuid,
                        principalTable: "UserTags",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOriginalWords",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Word = table.Column<string>(type: "text", nullable: false),
                    LanguageLevelGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    UserTopicGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    GeneralOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOriginalWords", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_GeneralOriginalWords_GeneralOriginalWordG~",
                        column: x => x.GeneralOriginalWordGuid,
                        principalTable: "GeneralOriginalWords",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_LanguageLevels_LanguageLevelGuid",
                        column: x => x.LanguageLevelGuid,
                        principalTable: "LanguageLevels",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_UserTopics_UserTopicGuid",
                        column: x => x.UserTopicGuid,
                        principalTable: "UserTopics",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_UserOriginalWords_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "UserOriginalWordUserTag",
                columns: table => new
                {
                    UserOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTagGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOriginalWordUserTag", x => new { x.UserOriginalWordGuid, x.UserTagGuid });
                    table.ForeignKey(
                        name: "FK_UserOriginalWordUserTag_UserOriginalWords_UserOriginalWordG~",
                        column: x => x.UserOriginalWordGuid,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOriginalWordUserTag_UserTags_UserTagGuid",
                        column: x => x.UserTagGuid,
                        principalTable: "UserTags",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordExamples",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalExample = table.Column<string>(type: "text", nullable: false),
                    TranslatedExample = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordExamples", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserWordExamples_UserOriginalWords_UserOriginalWordGuid",
                        column: x => x.UserOriginalWordGuid,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordForms",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WordForm = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    UserOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordForms", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserWordForms_UserOriginalWords_UserOriginalWordGuid",
                        column: x => x.UserOriginalWordGuid,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordTranslations",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Translation = table.Column<string>(type: "text", nullable: false),
                    UserOriginalWordGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordTranslations", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserWordTranslations_UserOriginalWords_UserOriginalWordGuid",
                        column: x => x.UserOriginalWordGuid,
                        principalTable: "UserOriginalWords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWordTranslations_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWords_GeneralTopicGuid",
                table: "GeneralOriginalWords",
                column: "GeneralTopicGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralOriginalWords_LanguageLevelGuid",
                table: "GeneralOriginalWords",
                column: "LanguageLevelGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordExamples_GeneralOriginalWordGuid",
                table: "GeneralWordExamples",
                column: "GeneralOriginalWordGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordExamples_LanguageGuid",
                table: "GeneralWordExamples",
                column: "LanguageGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordForms_GeneralOriginalWordGuid",
                table: "GeneralWordForms",
                column: "GeneralOriginalWordGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordTranslations_GeneralOriginalWordGuid",
                table: "GeneralWordTranslations",
                column: "GeneralOriginalWordGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralWordTranslations_LanguageGuid",
                table: "GeneralWordTranslations",
                column: "LanguageGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_GeneralExerciseGuid",
                table: "UserExercises",
                column: "GeneralExerciseGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_UserGuid",
                table: "UserExercises",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseUserTag_UserTagGuid",
                table: "UserExerciseUserTag",
                column: "UserTagGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_GeneralOriginalWordGuid",
                table: "UserOriginalWords",
                column: "GeneralOriginalWordGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_LanguageLevelGuid",
                table: "UserOriginalWords",
                column: "LanguageLevelGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_UserGuid",
                table: "UserOriginalWords",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWords_UserTopicGuid",
                table: "UserOriginalWords",
                column: "UserTopicGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserOriginalWordUserTag_UserTagGuid",
                table: "UserOriginalWordUserTag",
                column: "UserTagGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRuleNotes_RuleGuid",
                table: "UserRuleNotes",
                column: "RuleGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRuleNotes_UserGuid",
                table: "UserRuleNotes",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRuleUserTag_UserTagGuid",
                table: "UserRuleUserTag",
                column: "UserTagGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RequiredLanguageLevelGuid",
                table: "Users",
                column: "RequiredLanguageLevelGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TranslatedLanguageGuid",
                table: "Users",
                column: "TranslatedLanguageGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserTags_UserGuid",
                table: "UserTags",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserTagTagTypes_UserTagGuid",
                table: "UserTagTagTypes",
                column: "UserTagGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_GeneralTopicGuid",
                table: "UserTopics",
                column: "GeneralTopicGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_UserGuid",
                table: "UserTopics",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopicUserTag_UserTopicGuid",
                table: "UserTopicUserTag",
                column: "UserTopicGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordExamples_UserOriginalWordGuid",
                table: "UserWordExamples",
                column: "UserOriginalWordGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordForms_UserOriginalWordGuid",
                table: "UserWordForms",
                column: "UserOriginalWordGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordTranslations_UserGuid",
                table: "UserWordTranslations",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordTranslations_UserOriginalWordGuid",
                table: "UserWordTranslations",
                column: "UserOriginalWordGuid");
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
                name: "UserExerciseUserTag");

            migrationBuilder.DropTable(
                name: "UserOriginalWordUserTag");

            migrationBuilder.DropTable(
                name: "UserRuleNotes");

            migrationBuilder.DropTable(
                name: "UserRuleUserTag");

            migrationBuilder.DropTable(
                name: "UserTagTagTypes");

            migrationBuilder.DropTable(
                name: "UserTopicUserTag");

            migrationBuilder.DropTable(
                name: "UserWordExamples");

            migrationBuilder.DropTable(
                name: "UserWordForms");

            migrationBuilder.DropTable(
                name: "UserWordTranslations");

            migrationBuilder.DropTable(
                name: "UserExercises");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "TagTypes");

            migrationBuilder.DropTable(
                name: "UserTags");

            migrationBuilder.DropTable(
                name: "UserOriginalWords");

            migrationBuilder.DropTable(
                name: "GeneralExercises");

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
