using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Connections
{
    public static class DataModelConfiguration
    {
        public static void ConfigureModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .UseIdentityByDefaultColumn()
                      .ValueGeneratedOnAdd();

                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Cpf).HasMaxLength(14);
                entity.Property(u => u.IsActive).IsRequired();

                entity.HasOne(u => u.PessoalProfile)
                      .WithOne(p => p.User)
                      .HasForeignKey<PessoalProfileEntity>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.ProfessionalProfile)
                      .WithOne(p => p.User)
                      .HasForeignKey<ProfessionalProfileEntity>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Courses)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.Department)
                      .WithOne(d => d.User)
                      .HasForeignKey<DepartmentEntity>(d => d.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.HistoryPasswords)
                      .WithOne(ph => ph.User)
                      .HasForeignKey(ph => ph.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.UserBadges)
                      .WithOne(ub => ub.User)
                      .HasForeignKey(ub => ub.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PessoalProfileEntity>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FullName).HasMaxLength(255);
                entity.Property(p => p.DateOfBirth);
                entity.Property(p => p.Gender).HasMaxLength(50);
                entity.Property(p => p.Marital).HasMaxLength(50);

                entity.HasOne(p => p.Address)
                      .WithOne(a => a.PessoalProfile)
                      .HasForeignKey<AddressEntity>(a => a.PessoalProfileId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AddressEntity>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.PostalCode).HasMaxLength(50);
                entity.Property(a => a.Street).HasMaxLength(255);
                entity.Property(a => a.Neighborhood).HasMaxLength(255);
                entity.Property(a => a.City).HasMaxLength(100);
                entity.Property(a => a.Uf).HasMaxLength(100);
            });

            modelBuilder.Entity<ProfessionalProfileEntity>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.JobTitle).HasMaxLength(255);
                entity.Property(p => p.YearsOfExperience);
                entity.Property(p => p.Skills);
                entity.Property(p => p.Certifications);

                entity.HasOne(p => p.User)
                      .WithOne(u => u.ProfessionalProfile)
                      .HasForeignKey<ProfessionalProfileEntity>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.EducationLevel)
                      .WithOne(e => e.ProfessionalProfile)
                      .HasForeignKey<EducationLevelEntity>(e => e.ProfessionalProfileId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EducationLevelEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.Institution).HasMaxLength(255);
                entity.Property(e => e.StartedAt);
                entity.Property(e => e.EndeedAt);

                entity.HasOne(e => e.ProfessionalProfile)
                      .WithOne(p => p.EducationLevel)
                      .HasForeignKey<EducationLevelEntity>(e => e.ProfessionalProfileId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CourseEntity>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.Description);
                entity.Property(c => c.StartDate).IsRequired();
                entity.Property(c => c.EndDate).IsRequired();

                entity.HasOne(c => c.User)
                      .WithMany(u => u.Courses)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany<CourseFeedbackEntity>()
                      .WithOne(f => f.Course)
                      .HasForeignKey(f => f.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany<ExamEntity>()
                      .WithOne(e => e.Course)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DepartmentEntity>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(255);
                entity.Property(d => d.Description).HasMaxLength(500);

                entity.HasOne(d => d.User)
                      .WithOne(u => u.Department)
                      .HasForeignKey<DepartmentEntity>(d => d.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<HistoryPasswordEntity>(entity =>
            {
                entity.HasKey(ph => ph.Id);
                entity.Property(ph => ph.OldPassword).IsRequired().HasMaxLength(255);

                entity.HasOne(ph => ph.User)
                      .WithMany(u => u.HistoryPasswords)
                      .HasForeignKey(ph => ph.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TeamEntity>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(255);
                entity.Property(t => t.Description).HasMaxLength(500);

                entity.HasOne(t => t.Department)
                      .WithMany(d => d.Teams)
                      .HasForeignKey(t => t.DepartmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CourseAvaliationEntity>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Rating).IsRequired();
                entity.Property(a => a.Comment).HasColumnType("text");
                entity.Property(a => a.ReviewDate).IsRequired();

                entity.HasOne(a => a.Course)
                      .WithMany(c => c.Avaliations)
                      .HasForeignKey(a => a.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CourseActivitieEntity>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Title).IsRequired().HasMaxLength(255);
                entity.Property(a => a.Description).HasColumnType("text");
                entity.Property(a => a.StartDate).IsRequired();
                entity.Property(a => a.DueDate).IsRequired();
                entity.Property(a => a.MaxScore).IsRequired();

                entity.HasOne(a => a.Course)
                      .WithMany(c => c.Activities)
                      .HasForeignKey(a => a.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotificationEntity>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Description).HasMaxLength(500);

                entity.HasOne(n => n.Course)
                      .WithMany()
                      .HasForeignKey(n => n.CourseId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CourseFeedbackEntity>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Rating).IsRequired();
                entity.Property(f => f.Comment).HasMaxLength(1000);

                entity.HasOne(f => f.Course)
                      .WithMany(c => c.Feedbacks)
                      .HasForeignKey(f => f.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Student)
                      .WithMany()
                      .HasForeignKey(f => f.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(f => new { f.CourseId, f.StudentId }).IsUnique();
            });

            modelBuilder.Entity<QuestionEntity>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Statement).IsRequired().HasColumnType("text");
                entity.Property(q => q.Order).IsRequired();
                entity.Property(q => q.Points).IsRequired().HasColumnType("decimal(10,2)");

                entity.HasOne(q => q.CourseActivitie)
                      .WithMany(a => a.Questions)
                      .HasForeignKey(q => q.CourseActivitieId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(q => new { q.CourseActivitieId, q.Order }).IsUnique();
            });

            modelBuilder.Entity<QuestionOptionEntity>(entity =>
            {
                entity.ToTable("QuestionOptionEntity");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Text).IsRequired().HasMaxLength(1000);
                entity.Property(o => o.IsCorrect).IsRequired();

                entity.HasOne(o => o.Question)
                      .WithMany(q => q.Options)
                      .HasForeignKey(o => o.QuestionId)
                      .HasConstraintName("FK_QuestionOption_Question_QuestionId")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(o => new { o.QuestionId, o.IsCorrect })
                      .IsUnique()
                      .HasFilter("\"IsCorrect\" = TRUE");
            });

            modelBuilder.Entity<ExamEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Instructions).HasColumnType("text");
                entity.Property(e => e.StartAt).IsRequired();
                entity.Property(e => e.EndAt).IsRequired();
                entity.Property(e => e.IsPublished).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.ModificationDate).IsRequired();

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Exams)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.CourseId, e.StartAt });
            });

            modelBuilder.Entity<ExamQuestionEntity>(entity =>
            {
                entity.HasKey(eq => eq.Id);
                entity.Property(eq => eq.Order).IsRequired();
                entity.Property(eq => eq.Points).IsRequired().HasColumnType("decimal(10,2)");

                entity.HasOne(eq => eq.Exam)
                      .WithMany(e => e.ExamQuestions)
                      .HasForeignKey(eq => eq.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(eq => eq.Question)
                      .WithMany()
                      .HasForeignKey(eq => eq.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(eq => new { eq.ExamId, eq.QuestionId }).IsUnique();
            });

            modelBuilder.Entity<ExamHistoryEntity>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(h => h.AttemptNumber).IsRequired();
                entity.Property(h => h.StartedAt).IsRequired();
                entity.Property(h => h.FinishedAt);
                entity.Property(h => h.Score).HasColumnType("decimal(10,2)");
                entity.Property(h => h.DurationSeconds);
                entity.Property(h => h.Status).IsRequired();

                entity.HasOne(h => h.Exam)
                      .WithMany()
                      .HasForeignKey(h => h.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(h => h.Student)
                      .WithMany()
                      .HasForeignKey(h => h.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(h => new { h.ExamId, h.StudentId });
                entity.HasIndex(h => new { h.ExamId, h.StudentId, h.AttemptNumber }).IsUnique();
            });

            modelBuilder.Entity<BadgeEntity>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(150);
                entity.Property(b => b.Description).HasMaxLength(500);
                entity.HasMany(b => b.UserBadges)
                      .WithOne(ub => ub.Badge)
                      .HasForeignKey(ub => ub.BadgeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserBadgeEntity>(entity =>
            {
                entity.HasKey(ub => new { ub.UserId, ub.BadgeId });
                entity.Property(ub => ub.EarnedAt).IsRequired();

                entity.HasOne(ub => ub.User)
                      .WithMany(u => u.UserBadges)
                      .HasForeignKey(ub => ub.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ub => ub.Badge)
                      .WithMany(b => b.UserBadges)
                      .HasForeignKey(ub => ub.BadgeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CalendarEntity>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Description).HasMaxLength(500);
                entity.Property(c => c.StartDate).IsRequired();
                entity.Property(c => c.EndDate).IsRequired();
                entity.Property(c => c.Type).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Location).HasMaxLength(200);

                entity.HasOne(c => c.Course)
                      .WithMany()
                      .HasForeignKey(c => c.CourseId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(c => c.Exam)
                      .WithMany()
                      .HasForeignKey(c => c.ExamId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}