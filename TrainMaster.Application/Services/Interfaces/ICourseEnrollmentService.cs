using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICourseEnrollmentService
    {
        Task<Result<CourseEnrollmentEntity>> EnrollStudent(CourseEnrollmentDto enrollmentDto);
        Task<Result<CourseEnrollmentEntity>> CancelEnrollment(int enrollmentId);
        Task<List<CourseEnrollmentResponseDto>> GetEnrollmentsByStudent(int studentId);
        Task<List<CourseEnrollmentResponseDto>> GetEnrollmentsByCourse(int courseId);
        Task<Result<CourseEnrollmentEntity>> GetEnrollmentById(int enrollmentId);
        Task<Result<CourseEnrollmentResponseDto>> GetEnrollmentByIdWithProgress(int enrollmentId);
    }
}

