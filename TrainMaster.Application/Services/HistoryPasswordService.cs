using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Infrastracture.Security.Cryptography;
using TrainMaster.Shared.Logging;

namespace TrainMaster.Application.Services
{
    public class HistoryPasswordService : IHistoryPasswordService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public HistoryPasswordService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<HistoryPasswordEntity>> UpdateOldPassword(int id, string newPassword)
        {
            var crypto = new BCryptoAlgorithm();
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var historyPassById = await _repositoryUoW.HistoryPasswordRepository.GetById(id);
                if (historyPassById is null)
                    throw new InvalidOperationException("Error updating team");

                historyPassById.ModificationDate = DateTime.UtcNow;
                historyPassById.OldPassword = crypto.HashPassword(historyPassById.OldPassword);
                _repositoryUoW.HistoryPasswordRepository.UpdateOldPassword(historyPassById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<HistoryPasswordEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorHistoryPassword(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error updating password history", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}