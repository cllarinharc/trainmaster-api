using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class QuestionEntity : BaseEntity
    {
        public int? CourseActivitieId { get; set; }
        public string Statement { get; set; } = default!; 
        public int Order { get; set; }                   
        public decimal Points { get; set; }        
        public CourseActivitieEntity CourseActivitie { get; set; } = default!;
        public ICollection<QuestionOptionEntity> Questions { get; set; } = new List<QuestionOptionEntity>();
        public ICollection<QuestionOptionEntity> Options { get; set; } = new List<QuestionOptionEntity>();
    }
}