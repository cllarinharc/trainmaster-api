using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IExamHistoryService
    {
        Task<List<ExamHistoryEntity>> GetByUserId(long userId);
    }
}