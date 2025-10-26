using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class EducationLevelEntity : BaseEntity
    {
        public string? Title { get; set; }
        public string? Institution { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndeedAt { get; set; }

        [JsonIgnore]
        public ProfessionalProfileEntity? ProfessionalProfile { get; set; }
        public int ProfessionalProfileId { get; set; }
    }
}