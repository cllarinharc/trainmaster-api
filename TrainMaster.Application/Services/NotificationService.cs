using Serilog;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;

namespace TrainMaster.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public NotificationService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<List<NotificationEntity>> Get(int courseId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<NotificationEntity> notificationEntities = await _repositoryUoW.NotificationRepository.Get(courseId);
                _repositoryUoW.Commit();
                return notificationEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllNotificationError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list Notification");
            }
            finally
            {
                Log.Error(LogMessages.GetAllNotificationSuccess());
                transaction.Dispose();
            }
        }
    }
}