using System.Text.Json.Serialization;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class BadgeEntity : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<UserBadgeEntity> UserBadges { get; set; } = new List<UserBadgeEntity>();
    }
}