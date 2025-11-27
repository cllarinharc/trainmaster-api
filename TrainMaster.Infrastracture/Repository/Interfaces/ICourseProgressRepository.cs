using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ICourseProgressRepository
    {
        Task<CourseProgressEntity> Add(CourseProgressEntity progressEntity);
        CourseProgressEntity Update(CourseProgressEntity progressEntity);
        Task<CourseProgressEntity?> GetById(int id);
        Task<CourseProgressEntity?> GetByStudentAndCourse(int studentId, int courseId);
        Task<List<CourseProgressEntity>> GetByStudentId(int studentId);
        Task<List<CourseProgressEntity>> GetByCourseId(int courseId);
        Task<bool> Exists(int studentId, int courseId);
    }
}

