using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IExamHistoryRepository
    {
        Task<List<ExamHistoryEntity>> GetByUserId(long userId);
        Task<ExamHistoryEntity> Add(ExamHistoryEntity entity);
        Task<int> GetNextAttemptNumber(int examId, int studentId);
        Task<List<ExamHistoryEntity>> GetByExamAndStudent(int examId, int studentId);
    }
}