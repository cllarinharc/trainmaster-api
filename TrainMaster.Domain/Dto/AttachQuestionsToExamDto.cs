namespace TrainMaster.Domain.Dto
{
    public class AttachQuestionsToExamDto
    {
        public int ExamId { get; set; }
        public List<int> QuestionIds { get; set; } = new();
    }
}