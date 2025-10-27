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

        public async Task<Result<BadgeEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var badge = await _repositoryUoW.BadgeRepository.GetById(id);
                if (badge == null)
                    return Result<BadgeEntity>.Error("Badge não encontrado");

                _repositoryUoW.Commit();

                return Result<BadgeEntity>.Okedit(badge);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar badge por ID", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}