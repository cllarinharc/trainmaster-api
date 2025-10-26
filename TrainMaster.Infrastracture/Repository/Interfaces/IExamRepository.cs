using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IExamRepository
    {
        Task<ExamEntity> Add(ExamEntity examEntity);
        ExamEntity Update(ExamEntity examEntity);
        Task<ExamEntity> Delete(ExamEntity examEntity);

        Task<ExamEntity?> GetById(int id, bool includeQuestions = false);
        Task<List<ExamEntity>> GetAll();
        Task<List<ExamEntity>> GetPaginated(int pageNumber, int pageSize);
        Task<List<ExamEntity>> GetByCourseId(int courseId);

        Task<bool> Exists(int id);
    }
}