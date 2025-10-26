using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class CourseActivitieAddDto
    {
        [Required] public string Title { get; set; } = default!;
        public string? Description { get; set; }

        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime DueDate { get; set; }

        [Required] public int MaxScore { get; set; }  
        [Required] public int CourseId { get; set; }
    }
}