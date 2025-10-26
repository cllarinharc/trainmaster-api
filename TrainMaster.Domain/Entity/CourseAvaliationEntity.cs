using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class CourseAvaliationEntity : BaseEntity
    {        
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public int? CourseId { get; set; }
        public virtual CourseEntity? Course { get; set; }
    }
}