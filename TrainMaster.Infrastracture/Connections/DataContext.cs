using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Connections
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<UserEntity> UserEntity { get; set; }
        public DbSet<PessoalProfileEntity> PessoalProfileEntity { get; set; }
        public DbSet<ProfessionalProfileEntity> ProfessionalProfileEntity { get; set; }
        public DbSet<AddressEntity> AddressEntity { get; set; }
        public DbSet<EducationLevelEntity> EducationLevelEntity { get; set; }
        public DbSet<CourseEntity> CourseEntity { get; set; }
        public DbSet<CourseFeedbackEntity> CourseFeedbackEntity { get; set; }
        public DbSet<DepartmentEntity> DepartmentEntity { get; set; }
        public DbSet<TeamEntity> TeamEntity { get; set; }
        public DbSet<HistoryPasswordEntity> HistoryPasswordEntity { get; set; }
        public DbSet<CourseAvaliationEntity> CourseAvaliationEntity { get; set; }
        public DbSet<CourseActivitieEntity> CourseActivitieEntity { get; set; }
        public DbSet<NotificationEntity> NotificationEntity { get; set; }
        public DbSet<QuestionEntity> QuestionEntity { get; set; }
        public DbSet<QuestionOptionEntity> QuestionOptionEntity { get; set; }
        public DbSet<ExamEntity> ExamEntity { get; set; }
        public DbSet<ExamQuestionEntity> ExamQuestionEntity { get; set; }
        public DbSet<ExamHistoryEntity> ExamHistoryEntity { get; set; }
        public DbSet<BadgeEntity> BadgeEntity { get; set; }
        public DbSet<UserBadgeEntity> UserBadgeEntity { get; set; }
        public DbSet<FaqEntity> FaqEntity { get; set; }
        public DbSet<CourseEnrollmentEntity> CourseEnrollmentEntity { get; set; }
        public DbSet<CourseProgressEntity> CourseProgressEntity { get; set; }
        public DbSet<CourseActivityProgressEntity> CourseActivityProgressEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            DataModelConfiguration.ConfigureModels(modelBuilder);
        }
    }
}