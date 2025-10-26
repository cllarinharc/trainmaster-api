namespace TrainMaster.Domain.Dto
{
    public class AttachQuestionsToActivityDto
    {
        public int ActivityId { get; set; }
        public List<int> QuestionIds { get; set; } = new();
    }
}