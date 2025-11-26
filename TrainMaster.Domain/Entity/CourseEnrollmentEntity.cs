using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class CourseEnrollmentEntity : BaseEntity
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public UserEntity? Student { get; set; }

        [JsonIgnore]
        public CourseEntity? Course { get; set; }
    }
}

