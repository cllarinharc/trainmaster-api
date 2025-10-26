using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICourseFeedbackService
    {
        Task<List<CourseFeedbackEntity>> GetAll();
        Task<List<CourseFeedbackEntity>> GetByCourseId(int courseId);
    }
}