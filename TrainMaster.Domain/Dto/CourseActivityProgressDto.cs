namespace TrainMaster.Domain.Dto
{
    public class CourseActivityProgressDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int ActivityId { get; set; }
        public bool IsCompleted { get; set; }
        public decimal? Score { get; set; }
    }
}


