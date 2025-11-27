using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICourseProgressService
    {
        Task<Result<CourseProgressResponseDto>> GetOrCreateProgress(int studentId, int courseId);
        Task<Result<CourseProgressResponseDto>> GetProgress(int studentId, int courseId);
        Task<Result<CourseProgressResponseDto>> UpdateProgress(int studentId, int courseId);
        Task<List<CourseProgressResponseDto>> GetProgressByStudent(int studentId);
        Task<List<CourseProgressResponseDto>> GetProgressByCourse(int courseId);
    }
}


