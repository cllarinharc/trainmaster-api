namespace TrainMaster.Domain.Dto
{
    public class QuestionAddDto
    {
        public string Statement { get; set; } = default!;
        public int Order { get; set; }
        public decimal Points { get; set; }
        public int? CourseActivitieId { get; set; }
        public List<QuestionOptionAddDto> Options { get; set; } = new();
    }
}