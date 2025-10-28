using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class FaqEntity : BaseEntity
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
}