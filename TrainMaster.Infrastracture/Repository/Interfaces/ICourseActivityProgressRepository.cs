using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ICourseActivityProgressRepository
    {
        Task<CourseActivityProgressEntity> Add(CourseActivityProgressEntity progressEntity);
        CourseActivityProgressEntity Update(CourseActivityProgressEntity progressEntity);
        Task<CourseActivityProgressEntity?> GetById(int id);
        Task<CourseActivityProgressEntity?> GetByStudentAndActivity(int studentId, int activityId);
        Task<List<CourseActivityProgressEntity>> GetByStudentAndCourse(int studentId, int courseId);
        Task<List<CourseActivityProgressEntity>> GetByActivityId(int activityId);
        Task<bool> Exists(int studentId, int activityId);
    }
}

