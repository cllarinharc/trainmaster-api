using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IBadgeService
    {
        Task<Result<BadgeEntity>> GetById(int id);
    }
}