using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ICourseActivitieRepository
    {
        Task<CourseActivitieEntity> Add(CourseActivitieEntity entity);
        Task<List<CourseActivitieEntity>> Get();
        Task<CourseActivitieEntity?> GetById(int id);
        CourseActivitieEntity Update(CourseActivitieEntity entity);
        Task<List<CourseActivitieEntity>> GetByCourseId(int courseId);
    }
}