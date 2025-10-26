using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Result<CourseDto>> Add(CourseDto courseEntity);
        Task Delete(int userId);
        Task<List<CourseEntity>> Get();
        Task<List<CourseEntity>> GetByUserId(int id);
        Task<List<CourseEntity>> GetByName(string name);
        Task<Result<CourseEntity>> Update(CourseEntity courseEntity);
    }
}