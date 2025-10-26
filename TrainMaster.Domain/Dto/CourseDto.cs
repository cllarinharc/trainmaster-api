using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class CourseDto
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        public string? Description { get; set; }
        public string? Author { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }        
        public int UserId { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}