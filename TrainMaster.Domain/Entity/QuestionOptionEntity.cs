using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class QuestionOptionEntity : BaseEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }          
        public QuestionEntity Question { get; set; } = null!;
        public string Text { get; set; } = "";
        public bool IsCorrect { get; set; }
    }
}