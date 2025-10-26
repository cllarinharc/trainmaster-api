namespace TrainMaster.Domain.Entity
{
    public class ExamEntity
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public string Title { get; set; } = default!;
        public string? Instructions { get; set; }
        public DateTime StartAt { get; set; }          
        public DateTime EndAt { get; set; }            
        public bool IsPublished { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime ModificationDate { get; set; } = DateTime.UtcNow;

        public CourseEntity Course { get; set; } = default!;
        public ICollection<ExamQuestionEntity> ExamQuestions { get; set; } = new List<ExamQuestionEntity>();
    }

    public class ExamQuestionEntity
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }

        public int Order { get; set; }                 
        public decimal Points { get; set; }            

        public ExamEntity Exam { get; set; } = default!;
        public QuestionEntity Question { get; set; } = default!;
    }
}