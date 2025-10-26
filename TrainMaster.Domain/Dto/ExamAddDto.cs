namespace TrainMaster.Domain.Dto
{
    public class ExamAddDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Instructions { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public bool IsPublished { get; set; }
        public int CourseId { get; set; }
    }
}