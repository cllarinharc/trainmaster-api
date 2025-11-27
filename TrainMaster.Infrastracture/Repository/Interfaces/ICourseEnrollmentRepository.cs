using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ICourseEnrollmentRepository
    {
        Task<CourseEnrollmentEntity> Add(CourseEnrollmentEntity enrollmentEntity);
        CourseEnrollmentEntity Update(CourseEnrollmentEntity enrollmentEntity);
        Task<CourseEnrollmentEntity?> GetById(int id);
        Task<CourseEnrollmentEntity?> GetByStudentAndCourse(int studentId, int courseId);
        Task<List<CourseEnrollmentEntity>> GetByStudentId(int studentId);
        Task<List<CourseEnrollmentEntity>> GetByCourseId(int courseId);
        Task<bool> Exists(int studentId, int courseId);
    }
}


