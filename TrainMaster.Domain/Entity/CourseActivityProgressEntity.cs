using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class CourseActivityProgressEntity : BaseEntity
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int ActivityId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? Score { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? LastAccessedDate { get; set; }

        [JsonIgnore]
        public UserEntity? Student { get; set; }

        [JsonIgnore]
        public CourseEntity? Course { get; set; }

        [JsonIgnore]
        public CourseActivitieEntity? Activity { get; set; }
    }
}


