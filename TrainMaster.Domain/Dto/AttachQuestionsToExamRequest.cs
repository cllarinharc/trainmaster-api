using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class AttachQuestionsToExamRequest
    {
        [Required] public int ExamId { get; set; }
        [Required] public List<int> QuestionIds { get; set; } = new();
    }
}