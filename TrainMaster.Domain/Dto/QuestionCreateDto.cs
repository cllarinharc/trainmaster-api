using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class QuestionCreateDto
    {
        [Required] public string Statement { get; set; } = default!;
        [Range(0, int.MaxValue)] public int Order { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Points { get; set; }
        public int? CourseActivitieId { get; set; }
        [MinLength(2)]
        public List<QuestionOptionCreateDto> Options { get; set; } = new();
    }
}