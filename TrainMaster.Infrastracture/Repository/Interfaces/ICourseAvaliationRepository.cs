using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ICourseAvaliationRepository
    {
        Task<CourseAvaliationEntity> Add(CourseAvaliationEntity entity);
        Task<List<CourseAvaliationEntity>> Get();
        Task<CourseAvaliationEntity?> GetById(int id);
        CourseAvaliationEntity Update(CourseAvaliationEntity entity);
    }
}