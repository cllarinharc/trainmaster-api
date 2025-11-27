using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICourseActivityProgressService
    {
        Task<Result<CourseActivityProgressResponseDto>> MarkActivityAsCompleted(CourseActivityProgressDto progressDto);
        Task<Result<CourseActivityProgressResponseDto>> UpdateActivityProgress(CourseActivityProgressDto progressDto);
        Task<Result<CourseActivityProgressResponseDto>> GetActivityProgress(int studentId, int activityId);
        Task<List<CourseActivityProgressResponseDto>> GetActivitiesProgressByCourse(int studentId, int courseId);
        Task<Result<CourseActivityProgressResponseDto>> MarkActivityAsAccessed(int studentId, int activityId);
    }
}

