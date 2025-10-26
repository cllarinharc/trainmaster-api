using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IHistoryPasswordService
    {
        Task<Result<HistoryPasswordEntity>> UpdateOldPassword(int id, string newPassword);
    }
}