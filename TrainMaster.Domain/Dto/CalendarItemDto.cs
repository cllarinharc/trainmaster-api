namespace TrainMaster.Domain.Dto
{
    public class CalendarItemDto
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Type { get; set; } = default!;
        public int? CourseId { get; set; }
        public int? ExamId { get; set; }
        public string? Location { get; set; }
    }
}