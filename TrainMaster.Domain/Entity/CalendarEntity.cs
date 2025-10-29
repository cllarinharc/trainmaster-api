using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class CalendarEntity : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Type { get; set; } = default!;
        public int? CourseId { get; set; }
        public int? ExamId { get; set; }
        public string? Location { get; set; }
        
        [JsonIgnore]
        public virtual CourseEntity? Course { get; set; }

        [JsonIgnore]
        public virtual ExamEntity? Exam { get; set; }
    }
}