using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class CourseAvaliationService : ICourseAvaliationService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CourseAvaliationService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<CourseAvaliationEntity>> Add(CourseAvaliationEntity entity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {                
                entity.ModificationDate = DateTime.UtcNow;
                entity.ReviewDate = DateTime.SpecifyKind(entity.ReviewDate, DateTimeKind.Utc);

                await _repositoryUoW.CourseAvaliationRepository.Add(entity);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<CourseAvaliationEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding Course Avaliation: {ex.Message}");
                transaction.Rollback();
                return Result<CourseAvaliationEntity>.Error("Error adding course evaluation.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<List<CourseAvaliationEntity>> GetAll()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var list = await _repositoryUoW.CourseAvaliationRepository.Get();
                _repositoryUoW.Commit();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving course evaluations: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Error retrieving evaluations.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<CourseAvaliationEntity?> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var entity = await _repositoryUoW.CourseAvaliationRepository.GetById(id);
                _repositoryUoW.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting Course Avaliation by ID: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Error getting evaluation.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<CourseAvaliationEntity>> Update(CourseAvaliationEntity entity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var existing = await _repositoryUoW.CourseAvaliationRepository.GetById(entity.Id);
                if (existing == null)
                    return Result<CourseAvaliationEntity>.Error("Evaluation not found.");

                existing.Rating = entity.Rating;
                existing.Comment = entity.Comment;
                existing.ReviewDate = DateTime.SpecifyKind(entity.ReviewDate, DateTimeKind.Utc);
                existing.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.CourseAvaliationRepository.Update(existing);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<CourseAvaliationEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating Course Avaliation: {ex.Message}");
                transaction.Rollback();
                return Result<CourseAvaliationEntity>.Error("Error updating course evaluation.");
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}