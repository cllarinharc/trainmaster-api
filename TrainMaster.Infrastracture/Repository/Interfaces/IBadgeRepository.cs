using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IBadgeRepository
    {
        Task<BadgeEntity?> GetById(int? id);
    }
}