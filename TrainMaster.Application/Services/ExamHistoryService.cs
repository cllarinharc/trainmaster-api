using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class ExamHistoryService : IExamHistoryService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public ExamHistoryService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<List<ExamHistoryEntity>> GetByUserId(long userId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var histories = await _repositoryUoW.ExamHistoryRepository.GetByUserId(userId);
                _repositoryUoW.Commit();
                return histories;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new InvalidOperationException($"Error loading exam histories for user {userId}");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<ExamHistoryEntity>> Add(ExamHistoryDto dto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                if (dto == null)
                    return Result<ExamHistoryEntity>.Error("Payload não pode ser nulo.");

                // Verificar se o exame existe
                var exam = await _repositoryUoW.ExamRepository.GetById(dto.ExamId);
                if (exam == null)
                {
                    Log.Error("Exame não encontrado: {ExamId}", dto.ExamId);
                    return Result<ExamHistoryEntity>.Error("Exame não encontrado.");
                }

                // Verificar se o aluno existe
                var student = await _repositoryUoW.UserRepository.GetById(dto.StudentId);
                if (student == null)
                {
                    Log.Error("Aluno não encontrado: {StudentId}", dto.StudentId);
                    return Result<ExamHistoryEntity>.Error("Aluno não encontrado.");
                }

                // Obter número da tentativa
                var attemptNumber = await _repositoryUoW.ExamHistoryRepository.GetNextAttemptNumber(dto.ExamId, dto.StudentId);

                // Criar entidade
                var entity = new ExamHistoryEntity
                {
                    ExamId = dto.ExamId,
                    StudentId = dto.StudentId,
                    AttemptNumber = attemptNumber,
                    StartedAt = DateTime.SpecifyKind(dto.StartedAt, DateTimeKind.Utc),
                    FinishedAt = dto.FinishedAt.HasValue ? DateTime.SpecifyKind(dto.FinishedAt.Value, DateTimeKind.Utc) : null,
                    DurationSeconds = dto.DurationSeconds,
                    Status = dto.Status,
                    Score = dto.Score,
                    CreateDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                var result = await _repositoryUoW.ExamHistoryRepository.Add(entity);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information("Histórico de exame registrado: ExamId={ExamId}, StudentId={StudentId}, AttemptNumber={AttemptNumber}, Status={Status}",
                    dto.ExamId, dto.StudentId, attemptNumber, dto.Status);

                return Result<ExamHistoryEntity>.Okedit(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao registrar histórico de exame");
                await transaction.RollbackAsync();
                return Result<ExamHistoryEntity>.Error($"Erro ao registrar histórico de exame: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}