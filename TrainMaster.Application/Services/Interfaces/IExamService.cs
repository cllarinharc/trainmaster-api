using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IExamService
    {
        Task<Result<ExamEntity>> Add(ExamEntity entity);
        Task<List<ExamEntity>> GetAll();
        Task<ExamEntity?> GetById(int id);
    }
}