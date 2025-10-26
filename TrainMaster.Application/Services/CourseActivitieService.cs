using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class CourseActivitieService : ICourseActivitieService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CourseActivitieService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<List<QuestionEntity>> GetByActivityId(int activityId)
        {
            using var tx = _repositoryUoW.BeginTransaction();
            try
            {
                if (activityId <= 0) return new List<QuestionEntity>();

                var qs = await _repositoryUoW.QuestionRepository
                                             .GetByActivityId(activityId, includeOptions: true);

                _repositoryUoW.Commit();
                return qs ?? new List<QuestionEntity>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar questões por ActivityId");
                tx.Rollback();
                throw new InvalidOperationException("Erro ao buscar questões por ActivityId.");
            }
            finally
            {
                tx.Dispose();
            }
        }

        public async Task<List<CourseActivitieEntity>> GetByCourseId(int courseId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                if (courseId <= 0)
                    throw new InvalidOperationException("CourseId inválido.");

                var list = await _repositoryUoW.CourseActivitieRepository.GetByCourseId(courseId);
                _repositoryUoW.Commit();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao buscar atividades por CourseId: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar atividades por CourseId.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<CourseActivitieEntity>> Add(CourseActivitieEntity entity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                entity.ModificationDate = DateTime.UtcNow;
                entity.StartDate = DateTime.SpecifyKind(entity.StartDate, DateTimeKind.Utc);
                entity.DueDate = DateTime.SpecifyKind(entity.DueDate, DateTimeKind.Utc);

                await _repositoryUoW.CourseActivitieRepository.Add(entity);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<CourseActivitieEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao adicionar atividade: {ex.Message}");
                transaction.Rollback();
                return Result<CourseActivitieEntity>.Error("Erro ao adicionar atividade.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<List<CourseActivitieEntity>> GetAll()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var list = await _repositoryUoW.CourseActivitieRepository.Get();
                _repositoryUoW.Commit();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao listar atividades: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao listar atividades.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<CourseActivitieEntity?> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var entity = await _repositoryUoW.CourseActivitieRepository.GetById(id);
                _repositoryUoW.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao buscar atividade por ID: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar atividade.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<CourseActivitieEntity>> Update(CourseActivitieEntity entity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var existing = await _repositoryUoW.CourseActivitieRepository.GetById(entity.Id);
                if (existing == null)
                    return Result<CourseActivitieEntity>.Error("Atividade não encontrada.");

                existing.Title = entity.Title;
                existing.Description = entity.Description;
                entity.StartDate = DateTime.SpecifyKind(entity.StartDate, DateTimeKind.Utc);
                entity.DueDate = DateTime.SpecifyKind(entity.DueDate, DateTimeKind.Utc);
                existing.MaxScore = entity.MaxScore;
                existing.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.CourseActivitieRepository.Update(existing);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<CourseActivitieEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao atualizar atividade: {ex.Message}");
                transaction.Rollback();
                return Result<CourseActivitieEntity>.Error("Erro ao atualizar atividade.");
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}