using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class ExamHistoryService : IExamHistoryService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public ExamHistoryService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<List<ExamHistoryEntity>> GetByUserId(long userId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var histories = await _repositoryUoW.ExamHistoryRepository.GetByUserId(userId);
                _repositoryUoW.Commit();
                return histories;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new InvalidOperationException($"Error loading exam histories for user {userId}");
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}