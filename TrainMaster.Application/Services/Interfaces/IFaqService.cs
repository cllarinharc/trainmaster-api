using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IFaqService
    {
        Task<List<FaqEntity>> Get();
    }
}