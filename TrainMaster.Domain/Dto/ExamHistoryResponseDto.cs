using TrainMaster.Domain.Enums;

namespace TrainMaster.Domain.Dto
{
    public class ExamHistoryResponseDto
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int AttemptNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public decimal? Score { get; set; }
        public int? DurationSeconds { get; set; }
        public ExamAttemptStatus Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        // Estatísticas do exame
        public int TotalQuestions { get; set; }
        public int AnsweredQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public bool IsApproved { get; set; }
        public decimal? ApprovalPercentage { get; set; } // Porcentagem mínima para aprovação (padrão 70%)
        public decimal? ScorePercentage { get; set; } // Porcentagem de acerto baseada no score

        // Informações do exame
        public string? ExamTitle { get; set; }
        public string? ExamInstructions { get; set; }
    }
}

