using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public BadgeService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public Task<Result<BadgeEntity>> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}