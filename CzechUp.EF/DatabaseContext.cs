using CzechUp.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CzechUp.EF
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<GeneralOriginalWord> GeneralOriginalWords { get; set; }
        public DbSet<UserOriginalWord> UserOriginalWords { get; set; }
        public DbSet<GeneralTopic> GeneralTopics { get; set; }
        public DbSet<GeneralWordTranslation> GeneralWordTranslations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageLevel> LanguageLevels { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }
        public DbSet<UserWordTranslation> UserWordTranslations { get; set; }
        public DbSet<GeneralWordForm> GeneralWordForms { get; set; }
        public DbSet<UserWordForm> UserWordForms { get; set; }
        public DbSet<GeneralWordExample> GeneralWordExamples { get; set; }
        public DbSet<UserWordExample> UserWordExamples { get; set; }
        public DbSet<GeneralExercise> GeneralExercises { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<UserRuleNote> UserRuleNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserOriginalWord>()
            .HasMany(w => w.UserTags)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserOriginalWordUserTag", // Имя промежуточной таблицы
                j => j.HasOne<UserTag>().WithMany().HasForeignKey("UserTagId"),  // Связь с UserTag
                j => j.HasOne<UserOriginalWord>().WithMany().HasForeignKey("UserOriginalWordId") // Связь с UserOriginalWord
            );

            modelBuilder.Entity<Rule>()
            .HasMany(w => w.UserTags)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserRuleUserTag", // Имя промежуточной таблицы
                j => j.HasOne<UserTag>().WithMany().HasForeignKey("UserTagId"),  //
                j => j.HasOne<Rule>().WithMany().HasForeignKey("UserRuleId") //
            );
        }

    }
}
