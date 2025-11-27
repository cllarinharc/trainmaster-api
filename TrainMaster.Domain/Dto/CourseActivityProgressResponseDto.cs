namespace TrainMaster.Domain.Dto
{
    public class CourseActivityProgressResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int ActivityId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? Score { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? LastAccessedDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        // Informações da atividade
        public string? ActivityTitle { get; set; }
        public string? ActivityDescription { get; set; }
        public int ActivityMaxScore { get; set; }
    }
}


