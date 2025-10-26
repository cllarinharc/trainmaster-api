using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class NotificationEntity : BaseEntity
    {
        public string? Description { get; set; }
        public int? CourseId { get; set; }
        public virtual CourseEntity Course { get; set; } = null!;
    }
}