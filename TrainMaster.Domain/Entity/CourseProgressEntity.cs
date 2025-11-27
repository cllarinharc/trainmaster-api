using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class CourseProgressEntity : BaseEntity
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal ProgressPercentage { get; set; } // 0-100
        public int CompletedActivitiesCount { get; set; }
        public int TotalActivitiesCount { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public int? LastActivityId { get; set; } // Última atividade acessada
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }

        [JsonIgnore]
        public UserEntity? Student { get; set; }

        [JsonIgnore]
        public CourseEntity? Course { get; set; }

        [JsonIgnore]
        public CourseActivitieEntity? LastActivity { get; set; }
    }
}

