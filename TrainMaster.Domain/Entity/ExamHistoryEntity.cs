using TrainMaster.Domain.Enums;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class ExamHistoryEntity : BaseEntity
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int AttemptNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public decimal? Score { get; set; }
        public int? DurationSeconds { get; set; }
        public ExamAttemptStatus Status { get; set; }
        public ExamEntity Exam { get; set; } = null!;
        public UserEntity Student { get; set; } = null!;
    }
}

namespace TrainMaster.Domain.Enums
{
    public enum ExamAttemptStatus
    {
        Started = 1,
        Submitted = 2,
        Graded = 3,
        Cancelled = 4,
        Expired = 5
    }
}