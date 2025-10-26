using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ICourseFeedbackRepository
    {
        Task<CourseFeedbackEntity> Add(CourseFeedbackEntity entity);
        CourseFeedbackEntity Delete(CourseFeedbackEntity entity);

        Task<List<CourseFeedbackEntity>> GetAll();
        Task<List<CourseFeedbackEntity>> GetByCourseId(int courseId);

        Task<CourseFeedbackEntity?> GetById(int id);
        CourseFeedbackEntity Update(CourseFeedbackEntity entity);

        Task<bool> ExistsByCourseAndStudent(int courseId, int studentId);
    }
}