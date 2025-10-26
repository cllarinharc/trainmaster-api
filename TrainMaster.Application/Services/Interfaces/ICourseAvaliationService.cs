using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICourseAvaliationService
    {
        Task<Result<CourseAvaliationEntity>> Add(CourseAvaliationEntity entity);
        Task<List<CourseAvaliationEntity>> GetAll();
        Task<CourseAvaliationEntity?> GetById(int id);
        Task<Result<CourseAvaliationEntity>> Update(CourseAvaliationEntity entity);
    }
}