using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class ProfilePessoalService : IProfilePessoal
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public ProfilePessoalService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<PessoalProfileEntity>> Add(PessoalProfileEntity pessoalProfileEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidPessoalProfile = await IsValidPessoalProfileRequest(pessoalProfileEntity);

                if (!isValidPessoalProfile.Success)
                {
                    Log.Error(LogMessages.InvalidPessoalProfileInputs());
                    return Result<PessoalProfileEntity>.Error(isValidPessoalProfile.Message);
                }

                var checkFullNameIsExist = await _repositoryUoW.PessoalProfileRepository.GetByFullName(pessoalProfileEntity.FullName);

                if (checkFullNameIsExist is not null)
                {
                    var errorMessage = "User already exists with that name";
                    Log.Error(LogMessages.FullNameExists());
                    return Result<PessoalProfileEntity>.Error(errorMessage);
                }

                if (pessoalProfileEntity.DateOfBirth > DateTime.Today.AddYears(-16))
                {
                    var errorMessage = "User must be 16 years or older.";
                    Log.Error(LogMessages.AgeBelow16());
                    return Result<PessoalProfileEntity>.Error(errorMessage);
                }

                pessoalProfileEntity.ModificationDate = DateTime.UtcNow;
                var result = await _repositoryUoW.PessoalProfileRepository.Add(pessoalProfileEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<PessoalProfileEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingUserError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new User");
            }
            finally
            {
                Log.Error(LogMessages.AddingUserSuccess());
                transaction.Dispose();
            }
        }

        public async Task Delete(int pessoalProfileEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var pessoalProfileToDelete = await _repositoryUoW.PessoalProfileRepository.GetById(pessoalProfileEntity);
                if (pessoalProfileToDelete is not null)
                    _repositoryUoW.PessoalProfileRepository.Delete(pessoalProfileToDelete);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeletePessoalProfileError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a Pessoal Profile.");
            }
            finally
            {
                Log.Error(LogMessages.DeletePessoalProfileSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<PessoalProfileEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<PessoalProfileEntity> pessoalProfileEntities = await _repositoryUoW.PessoalProfileRepository.Get();
                _repositoryUoW.Commit();
                return pessoalProfileEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllPessoalProfileError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list Pessoal Profile");
            }
            finally
            {
                Log.Error(LogMessages.GetAllPessoalProfileSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<PessoalProfileEntity>> Update(int id, PessoalProfilePartDto pessoalProfileEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var pessoalProfileById = await _repositoryUoW.PessoalProfileRepository.GetById(id);
                pessoalProfileById.FullName = pessoalProfileEntity.FullName;
                pessoalProfileById.Cpf = pessoalProfileEntity.Cpf;
                pessoalProfileById.Email = pessoalProfileEntity.Email;
                pessoalProfileById.ModificationDate = DateTime.UtcNow;
                pessoalProfileById.DateOfBirth = DateTime.SpecifyKind(pessoalProfileEntity.DateOfBirth, DateTimeKind.Utc);
                pessoalProfileById.Gender = pessoalProfileEntity.Gender;
                pessoalProfileById.Marital = pessoalProfileEntity.Marital;

                _repositoryUoW.PessoalProfileRepository.Update(pessoalProfileById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<PessoalProfileEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorPessoalProfile(ex));
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error creating or updating Pessoal Profile", ex);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<Result<PessoalProfileEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var result = await _repositoryUoW.PessoalProfileRepository.GetById(id);
                _repositoryUoW.Commit();
                return Result<PessoalProfileEntity>.Okedit(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar perfil por ID", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<PessoalProfileEntity>> IsValidPessoalProfileRequest(PessoalProfileEntity pessoalProfileEntity)
        {
            var requestValidator = await new PessoalProfileRequestValidator().ValidateAsync(pessoalProfileEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<PessoalProfileEntity>.Error(errorMessage);
            }

            return Result<PessoalProfileEntity>.Ok();
        }
    }
}