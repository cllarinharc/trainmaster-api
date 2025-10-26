using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class AttachQuestionsToActivityRequest
    {
        [Required] public int ActivityId { get; set; }
        [Required] public List<int> QuestionIds { get; set; } = new();
    }
}