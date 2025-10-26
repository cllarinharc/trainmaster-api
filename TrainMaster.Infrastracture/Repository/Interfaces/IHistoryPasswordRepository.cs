using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IHistoryPasswordRepository
    {
        DepartmentEntity UpdateOldPassword(HistoryPasswordEntity historyPasswordEntity);
        Task<HistoryPasswordEntity?> GetById(int? id);
    }
}