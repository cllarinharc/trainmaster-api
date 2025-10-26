using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class QuestionOptionCreateDto
    {
        [Required] public string Text { get; set; } = default!;
        public bool IsCorrect { get; set; }
    }
}