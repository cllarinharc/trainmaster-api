using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IExamQuestionRepository
    {
        Task<ExamQuestionEntity> Add(ExamQuestionEntity entity);
        Task<int> AddRange(IEnumerable<ExamQuestionEntity> entities);
        ExamQuestionEntity Update(ExamQuestionEntity entity);
        Task<ExamQuestionEntity> Delete(ExamQuestionEntity entity);
        Task<int> DeleteByExamId(int examId);

        Task<ExamQuestionEntity?> GetById(int id, bool includeRefs = false);
        Task<ExamQuestionEntity?> GetByExamAndQuestion(int examId, int questionId);
        Task<List<ExamQuestionEntity>> GetByExamId(int examId, bool includeQuestion = false);
        Task<List<ExamQuestionEntity>> GetAll();
        Task<List<ExamQuestionEntity>> GetPaginated(int pageNumber, int pageSize);

        Task<bool> Exists(int id);
    }
}