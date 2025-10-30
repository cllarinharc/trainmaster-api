using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IExamHistoryRepository
    {
        Task<List<ExamHistoryEntity>> GetByUserId(long userId);
    }
}