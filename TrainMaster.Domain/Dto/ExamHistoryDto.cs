using TrainMaster.Domain.Enums;

namespace TrainMaster.Domain.Dto
{
    public class ExamHistoryDto
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int? DurationSeconds { get; set; }
        public ExamAttemptStatus Status { get; set; }
        public decimal? Score { get; set; }
        public int? AnsweredQuestions { get; set; } // Quantas questões foram respondidas
        public int? CorrectAnswers { get; set; } // Quantas foram corretas
        public decimal? ApprovalPercentage { get; set; } // Porcentagem mínima para aprovação (padrão 70%)
    }
}

