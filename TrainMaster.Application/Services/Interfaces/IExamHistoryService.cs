using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IExamHistoryService
    {
        Task<List<ExamHistoryEntity>> GetByUserId(long userId);
        Task<Result<ExamHistoryEntity>> Add(ExamHistoryDto dto);
        Task<Result<ExamHistoryResponseDto>> AddWithStatistics(ExamHistoryDto dto);
    }
}