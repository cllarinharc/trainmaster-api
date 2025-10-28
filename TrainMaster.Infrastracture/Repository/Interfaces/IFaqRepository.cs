using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IFaqRepository
    {
        Task<List<FaqEntity>> Get();
    }
}