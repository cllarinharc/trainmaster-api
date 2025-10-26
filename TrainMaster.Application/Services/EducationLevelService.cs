using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class EducationLevelService : IEducationLevelService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public EducationLevelService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<EducationLevelEntity>> Add(EducationLevelEntity educationLevelEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidEducationLevel = await IsValidEducationLevelRequest(educationLevelEntity);

                if (!isValidEducationLevel.Success)
                {
                    Log.Error(LogMessages.InvalidEducationLevelInputs());
                    return Result<EducationLevelEntity>.Error(isValidEducationLevel.Message);
                }

                educationLevelEntity.ModificationDate = DateTime.UtcNow;
                var result = await _repositoryUoW.EducationLevelRepository.Add(educationLevelEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<EducationLevelEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingEducationLevelError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new Education Level");
            }
            finally
            {
                Log.Error(LogMessages.AddingEducationLevelSuccess());
                transaction.Dispose();
            }
        }

        public async Task Delete(int educationLevelId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var educationLevelEntity = await _repositoryUoW.EducationLevelRepository.GetById(educationLevelId);
                if (educationLevelEntity is not null)
                    _repositoryUoW.EducationLevelRepository.Update(educationLevelEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeleteEducationLevelError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a Education Level.");
            }
            finally
            {
                Log.Error(LogMessages.DeleteEducationLevelSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<EducationLevelEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<EducationLevelEntity> educationLevelEntities = await _repositoryUoW.EducationLevelRepository.Get();
                _repositoryUoW.Commit();
                return educationLevelEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllEducationLevelError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list Education Level");
            }
            finally
            {
                Log.Error(LogMessages.GetAllEducationLevelSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<EducationLevelEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var result = await _repositoryUoW.EducationLevelRepository.GetById(id);
                if (result == null)
                    return Result<EducationLevelEntity>.Error("Educação não encontrada");

                _repositoryUoW.Commit();

                return Result<EducationLevelEntity>.Okedit(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar educação por ID", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<EducationLevelEntity>> Update(int id, EducationLevelEntity educationLevelEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                educationLevelEntity.StartedAt = educationLevelEntity.StartedAt.HasValue
                    ? DateTime.SpecifyKind(educationLevelEntity.StartedAt.Value, DateTimeKind.Utc)
                    : null;

                educationLevelEntity.EndeedAt = educationLevelEntity.EndeedAt.HasValue
                    ? DateTime.SpecifyKind(educationLevelEntity.EndeedAt.Value, DateTimeKind.Utc)
                    : null;

                var educationLevelById = await _repositoryUoW.EducationLevelRepository.GetById(educationLevelEntity.Id);

                if (educationLevelById is null)
                {
                    educationLevelEntity.ModificationDate = DateTime.UtcNow;
                    educationLevelEntity.ProfessionalProfileId = id;

                    await _repositoryUoW.EducationLevelRepository.Add(educationLevelEntity);
                }
                else
                {
                    educationLevelById.Title = educationLevelEntity.Title;
                    educationLevelById.Institution = educationLevelEntity.Institution;
                    educationLevelById.StartedAt = educationLevelEntity.StartedAt;
                    educationLevelById.EndeedAt = educationLevelEntity.EndeedAt;
                    educationLevelById.ModificationDate = DateTime.UtcNow;

                    _repositoryUoW.EducationLevelRepository.Update(educationLevelById);
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<EducationLevelEntity>.Ok("Educação salva com sucesso.", educationLevelEntity);
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorEducationLevel(ex));
                transaction.Rollback();
                return Result<EducationLevelEntity>.Error("Erro ao salvar dados de Educação.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<EducationLevelEntity>> IsValidEducationLevelRequest(EducationLevelEntity educationLevelEntity)
        {
            var requestValidator = await new EducationLevelRequestValidator().ValidateAsync(educationLevelEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<EducationLevelEntity>.Error(errorMessage);
            }

            return Result<EducationLevelEntity>.Ok();
        }
    }
}