using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<NotificationEntity>> Get(int courseId);
    }
}