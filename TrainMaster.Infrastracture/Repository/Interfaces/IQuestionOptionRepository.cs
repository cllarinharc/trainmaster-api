using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IQuestionOptionRepository
    {
        Task<QuestionOptionEntity> Add(QuestionOptionEntity optionEntity);
        QuestionOptionEntity Update(QuestionOptionEntity optionEntity);
        Task<QuestionOptionEntity> Delete(QuestionOptionEntity optionEntity);
        Task<int> DeleteByQuestionId(int questionId);

        Task<QuestionOptionEntity?> GetById(int id);
        Task<List<QuestionOptionEntity>> GetByQuestionId(int questionId);
        Task<QuestionOptionEntity?> GetCorrectByQuestionId(int questionId);
        Task<List<QuestionOptionEntity>> GetAllByQuestionIds(IEnumerable<int> questionIds);

        Task<bool> Exists(int id);
    }
}