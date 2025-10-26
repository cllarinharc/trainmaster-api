using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public TeamService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<TeamEntity>> Add(TeamEntity teamEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidTeam = await IsValidTeamRequest(teamEntity);

                if (!isValidTeam.Success)
                {
                    Log.Error(LogMessages.InvalidTeamInputs());
                    return Result<TeamEntity>.Error(isValidTeam.Message);
                }

                TeamEntity? IsDuplicateName = await _repositoryUoW.TeamRepository.GetByName(teamEntity.Name);

                if (IsDuplicateName is not null)
                {
                    string errorMessage = $"Já existe um time com o nome \"{teamEntity.Name}\".";
                    Log.Error(LogMessages.DuplicateName());
                    return Result<TeamEntity>.Error(errorMessage);
                }

                teamEntity.ModificationDate = DateTime.UtcNow;
                teamEntity.IsActive = true;
                var result = await _repositoryUoW.TeamRepository.Add(teamEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<TeamEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingTeamError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new team");
            }
            finally
            {
                Log.Error(LogMessages.AddingTeamSuccess());
                transaction.Dispose();
            }
        }

        public async Task Delete(int teamId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var teamEntity = await _repositoryUoW.TeamRepository.GetById(teamId);
                if (teamEntity is not null)
                {
                    teamEntity.IsActive = true;
                    _repositoryUoW.TeamRepository.Update(teamEntity);
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeleteTeamError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a team.");
            }
            finally
            {
                Log.Error(LogMessages.DeleteTeamSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<TeamEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<TeamEntity> teamEntities = await _repositoryUoW.TeamRepository.Get();
                _repositoryUoW.Commit();
                return teamEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllTeamError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list team");
            }
            finally
            {
                Log.Error(LogMessages.GetAllTeamSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<TeamEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var result = await _repositoryUoW.TeamRepository.GetById(id);
                if (result == null)
                    return Result<TeamEntity>.Error("Time não encontrado");

                _repositoryUoW.Commit();

                return Result<TeamEntity>.Okedit(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar o time por ID", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<List<TeamEntity>> GetByUserId(int userId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var departmentResult = await _repositoryUoW.DepartmentRepository.GetByUserId(userId);

                if (departmentResult == null)
                    throw new InvalidOperationException("Departamento não encontrado para este usuário.");

                var departmentId = departmentResult.Id;
                var teams = await _repositoryUoW.TeamRepository.GetByDepartmentId(departmentId);

                _repositoryUoW.Commit();

                return teams;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error($"Erro ao buscar times: {ex.Message}");
                throw new InvalidOperationException("Erro ao buscar times", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<TeamEntity>> Update(TeamEntity teamEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var teamById = await _repositoryUoW.TeamRepository.GetById(teamEntity.Id);
                if (teamById is null)
                    throw new InvalidOperationException("Error updating team");

                teamById.ModificationDate = DateTime.UtcNow;
                teamById.Name = teamEntity.Name;
                teamById.Description = teamEntity.Description;

                _repositoryUoW.TeamRepository.Update(teamById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<TeamEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorTeam(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error updating team", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<TeamEntity>> IsValidTeamRequest(TeamEntity teamEntity)
        {
            var requestValidator = await new TeamRequestValidator().ValidateAsync(teamEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<TeamEntity>.Error(errorMessage);
            }

            return Result<TeamEntity>.Ok();
        }
    }
}