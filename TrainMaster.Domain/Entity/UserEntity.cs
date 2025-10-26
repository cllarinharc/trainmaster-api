using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class UserEntity : BaseEntity
    {
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        
        [JsonIgnore]
        public PessoalProfileEntity? PessoalProfile { get; set; }

        [JsonIgnore]
        public ProfessionalProfileEntity? ProfessionalProfile { get; set; }

        [JsonIgnore]
        public virtual ICollection<CourseEntity> Courses { get; set; } = new List<CourseEntity>();

        [JsonIgnore]
        public DepartmentEntity? Department { get; set; }

        [JsonIgnore]
        public virtual ICollection<HistoryPasswordEntity> HistoryPasswords { get; set; } = new List<HistoryPasswordEntity>();

        [JsonIgnore]
        public ICollection<UserBadgeEntity> UserBadges { get; set; } = new List<UserBadgeEntity>();
    }
}