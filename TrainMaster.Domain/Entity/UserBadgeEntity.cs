namespace TrainMaster.Domain.Entity
{
    public class UserBadgeEntity
    {
        public int UserId { get; set; }
        public int BadgeId { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        public UserEntity User { get; set; } = null!;
        public BadgeEntity Badge { get; set; } = null!;
    }
}