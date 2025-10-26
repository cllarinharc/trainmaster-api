using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICourseActivitieService
    {
        Task<Result<CourseActivitieEntity>> Add(CourseActivitieEntity entity);
        Task<List<CourseActivitieEntity>> GetAll();
        Task<CourseActivitieEntity?> GetById(int id);
        Task<Result<CourseActivitieEntity>> Update(CourseActivitieEntity entity);
        Task<List<CourseActivitieEntity>> GetByCourseId(int courseId);
        Task<List<QuestionEntity>> GetByActivityId(int activityId);
    }
}