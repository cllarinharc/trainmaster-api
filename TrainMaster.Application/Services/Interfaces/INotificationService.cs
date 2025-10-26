using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationEntity>> Get(int courseId);
    }
}