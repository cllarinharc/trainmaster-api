namespace TrainMaster.Domain.Dto
{
    public class CourseProgressResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int CompletedActivitiesCount { get; set; }
        public int TotalActivitiesCount { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public int? LastActivityId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        // Informações do curso
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }

        // Informações da última atividade
        public string? LastActivityTitle { get; set; }
    }
}

