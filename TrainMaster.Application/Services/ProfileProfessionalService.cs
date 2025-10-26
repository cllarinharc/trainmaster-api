using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class ProfileProfessionalService : IProfileProfessionalService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public ProfileProfessionalService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<ProfessionalProfileEntity>> Add(ProfessionalProfileEntity professionalProfileEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidProfessionalProfile = await IsValidProfessionalProfileRequest(professionalProfileEntity);

                if (!isValidProfessionalProfile.Success)
                {
                    Log.Error(LogMessages.InvalidProfessionalProfileInputs());
                    return Result<ProfessionalProfileEntity>.Error(isValidProfessionalProfile.Message);
                }

                professionalProfileEntity.ModificationDate = DateTime.UtcNow;
                var result = await _repositoryUoW.ProfessionalProfileRepository.Add(professionalProfileEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<ProfessionalProfileEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingProfessionalProfileError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new Professional Profile");
            }
            finally
            {
                Log.Error(LogMessages.AddingProfessionalProfileSuccess());
                transaction.Dispose();
            }
        }

        public async Task Delete(int professionalProfileId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var professionalProfileEntity = await _repositoryUoW.ProfessionalProfileRepository.GetById(professionalProfileId);
                if (professionalProfileEntity is not null)
                    _repositoryUoW.ProfessionalProfileRepository.Update(professionalProfileEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeleteProfessionalProfileError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a professional Profile.");
            }
            finally
            {
                Log.Error(LogMessages.DeleteProfessionalProfileSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<ProfessionalProfileEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<ProfessionalProfileEntity> professionalProfileEntities = await _repositoryUoW.ProfessionalProfileRepository.Get();
                _repositoryUoW.Commit();
                return professionalProfileEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllProfessionalProfileError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list ProfessionalProfile.");
            }
            finally
            {
                Log.Error(LogMessages.GetAllProfessionalProfileSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<ProfessionalProfileEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var result = await _repositoryUoW.ProfessionalProfileRepository.GetById(id);
                if (result == null)
                    return Result<ProfessionalProfileEntity>.Error("Perfil Profissional não encontrado");

                _repositoryUoW.Commit();

                return Result<ProfessionalProfileEntity>.Okedit(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar curso por ID", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<ProfessionalProfileEntity>> Update(int id, ProfessionalProfileEntity professionalProfileEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var professionalProfileById = await _repositoryUoW.ProfessionalProfileRepository.GetById(id);
                professionalProfileEntity.UserId = id;
                
                if (professionalProfileById is null)
                {
                    professionalProfileEntity.ModificationDate = DateTime.UtcNow;

                    await _repositoryUoW.ProfessionalProfileRepository.Add(professionalProfileEntity);
                    await _repositoryUoW.SaveAsync();
                    await transaction.CommitAsync();

                    return Result<ProfessionalProfileEntity>.Ok("Professional profile created successfully.", professionalProfileEntity);
                }

                professionalProfileById.YearsOfExperience = professionalProfileEntity.YearsOfExperience;
                professionalProfileById.Skills = professionalProfileEntity.Skills;
                professionalProfileById.Certifications = professionalProfileEntity.Certifications;
                professionalProfileById.JobTitle = professionalProfileEntity.JobTitle;
                professionalProfileById.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.ProfessionalProfileRepository.Update(professionalProfileById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<ProfessionalProfileEntity>.Ok("Professional profile updated successfully.", professionalProfileById);
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorProfessionalProfile(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error updating Professional Profile", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<ProfessionalProfileEntity>> IsValidProfessionalProfileRequest(ProfessionalProfileEntity professionalProfileEntity)
        {
            var requestValidator = await new ProfessionalProfileRequestValidator().ValidateAsync(professionalProfileEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<ProfessionalProfileEntity>.Error(errorMessage);
            }

            return Result<ProfessionalProfileEntity>.Ok();
        }
    }
}