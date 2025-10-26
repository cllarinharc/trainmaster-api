using System.ComponentModel.DataAnnotations;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class CourseFeedbackEntity : BaseEntity
    {
        public int CourseId { get; set; }
        public virtual CourseEntity Course { get; set; } = default!;
        public int StudentId { get; set; }
        public virtual UserEntity Student { get; set; } = default!;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}