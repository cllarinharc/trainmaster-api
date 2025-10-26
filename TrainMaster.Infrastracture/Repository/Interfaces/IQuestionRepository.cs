using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IQuestionRepository
    {
        Task<QuestionEntity> Add(QuestionEntity questionEntity);
        QuestionEntity Update(QuestionEntity questionEntity);
        Task<QuestionEntity?> GetById(int id, bool includeOptions = true);
        Task<List<QuestionEntity>> GetByIds(IEnumerable<int> ids, bool includeOptions = true);
        Task<List<QuestionEntity>> GetByActivityId(int activityId, bool includeOptions = true);
        Task<List<QuestionEntity>> GetPaginated(int pageNumber, int pageSize);
        Task<List<QuestionEntity>> GetAll();
        Task<bool> Exists(int id);
        Task<QuestionEntity> Delete(QuestionEntity questionEntity);
    }
}