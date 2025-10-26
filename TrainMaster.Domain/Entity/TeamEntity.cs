using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class TeamEntity : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public DepartmentEntity? Department { get; set; }
        public int DepartmentId { get; set; }
    }
}