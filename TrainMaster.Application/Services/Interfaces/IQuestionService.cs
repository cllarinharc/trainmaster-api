using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<Result<QuestionEntity>> Add(QuestionAddDto dto);
        Task<Result<bool>> AttachToActivity(AttachQuestionsToActivityDto dto);
        Task<Result<bool>> AttachToExam(AttachQuestionsToExamDto dto);
        Task<List<QuestionEntity>> GetByActivityId(int activityId);
    }
}