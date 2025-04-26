using CzechUp.EF.Models;
using CzechUp.EF.Models.Absract;
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

        //Další DbSet pro další modely...

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
        public DbSet<TagType> TagTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
        .Property(e => e.Guid)
        .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<GeneralOriginalWord>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserOriginalWord>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<GeneralTopic>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<GeneralWordTranslation>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Language>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<LanguageLevel>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Rule>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserTopic>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserWordTranslation>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<GeneralWordForm>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserWordForm>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<GeneralWordExample>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserWordExample>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<GeneralExercise>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserExercise>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserTag>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserRuleNote>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<TagType>()
                .Property(e => e.Guid)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<UserOriginalWord>()
            .HasMany(w => w.UserTags)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserOriginalWordUserTag",
                j => j.HasOne<UserTag>().WithMany().HasForeignKey("UserTagGuid"),
                j => j.HasOne<UserOriginalWord>().WithMany().HasForeignKey("UserOriginalWordGuid")
            );

            modelBuilder.Entity<Rule>()
            .HasMany(w => w.UserTags)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserRuleUserTag",
                j => j.HasOne<UserTag>().WithMany().HasForeignKey("UserTagGuid"),
                j => j.HasOne<Rule>().WithMany().HasForeignKey("UserRuleGuid")
            );

            modelBuilder.Entity<UserExercise>()
            .HasMany(w => w.UserTags)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserExerciseUserTag",
                j => j.HasOne<UserTag>().WithMany().HasForeignKey("UserTagGuid"),
                j => j.HasOne<UserExercise>().WithMany().HasForeignKey("UserExerciseGuid")
            );

            modelBuilder.Entity<UserTag>()
            .HasMany(w => w.TagTypes)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserTagTagTypes",
                j => j.HasOne<TagType>().WithMany().HasForeignKey("TagTypeGuid"),
                j => j.HasOne<UserTag>().WithMany().HasForeignKey("UserTagGuid")
            );

            modelBuilder.Entity<GeneralOriginalWord>()
            .HasMany(w => w.GeneralTopics)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "GeneralOriginalWordGeneralTopics",
                j => j.HasOne<GeneralTopic>().WithMany().HasForeignKey("GeneralTopicGuid"),
                j => j.HasOne<GeneralOriginalWord>().WithMany().HasForeignKey("GeneralOriginalWordGuid")
            );

            modelBuilder.Entity<UserOriginalWord>()
            .HasMany(w => w.UserTopics)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserOriginalWordUserTopic",
                j => j.HasOne<UserTopic>().WithMany().HasForeignKey("UserTopicGuid"),
                j => j.HasOne<UserOriginalWord>().WithMany().HasForeignKey("UserOriginalWordGuid")
            );
        }

    }
}
