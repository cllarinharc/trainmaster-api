using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class FaqService : IFaqService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public FaqService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<List<FaqEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<FaqEntity> faqEntities = await _repositoryUoW.FaqRepository.Get();
                _repositoryUoW.Commit();
                return faqEntities;
            }
            catch (Exception ex)
            {
                //Log.Error(LogMessages.GetAllUserError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list Faq");
            }
            finally
            {
                //Log.Error(LogMessages.GetAllUserSuccess());
                transaction.Dispose();
            }
        }
    }
}