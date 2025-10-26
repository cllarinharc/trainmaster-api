using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class CourseEntity : BaseEntity
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        public string? Description { get; set; }
        public string? Author { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public UserEntity? User { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual ICollection<CourseFeedbackEntity> Feedbacks { get; set; } = new List<CourseFeedbackEntity>();

        [JsonIgnore]
        public virtual ICollection<CourseAvaliationEntity> Avaliations { get; set; } = new List<CourseAvaliationEntity>();
        [JsonIgnore]
        public virtual ICollection<CourseActivitieEntity> Activities { get; set; } = new List<CourseActivitieEntity>();
        [JsonIgnore]
        public virtual ICollection<NotificationEntity> NotificationEntities { get; set; } = new List<NotificationEntity>();
        [JsonIgnore]
        public ICollection<ExamEntity> Exams { get; set; } = new List<ExamEntity>();
    }
}