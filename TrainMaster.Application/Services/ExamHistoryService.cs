using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using System.Linq;

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

        public async Task<Result<ExamHistoryResponseDto>> AddWithStatistics(ExamHistoryDto dto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                if (dto == null)
                    return Result<ExamHistoryResponseDto>.Error("Payload não pode ser nulo.");

                // Verificar se o exame existe
                var exam = await _repositoryUoW.ExamRepository.GetById(dto.ExamId);
                if (exam == null)
                {
                    Log.Error("Exame não encontrado: {ExamId}", dto.ExamId);
                    return Result<ExamHistoryResponseDto>.Error("Exame não encontrado.");
                }

                // Verificar se o aluno existe
                var student = await _repositoryUoW.UserRepository.GetById(dto.StudentId);
                if (student == null)
                {
                    Log.Error("Aluno não encontrado: {StudentId}", dto.StudentId);
                    return Result<ExamHistoryResponseDto>.Error("Aluno não encontrado.");
                }

                // Buscar questões do exame
                var examQuestions = await _repositoryUoW.ExamQuestionRepository.GetByExamId(dto.ExamId, includeQuestion: true);
                var totalQuestions = examQuestions.Count;
                var totalPoints = examQuestions.Sum(eq => eq.Points);

                // Obter número da tentativa
                var attemptNumber = await _repositoryUoW.ExamHistoryRepository.GetNextAttemptNumber(dto.ExamId, dto.StudentId);

                // Calcular estatísticas
                var answeredQuestions = dto.AnsweredQuestions ?? 0;
                var correctAnswers = dto.CorrectAnswers ?? 0;
                var wrongAnswers = answeredQuestions - correctAnswers;

                // Calcular porcentagem de aprovação (padrão 70%)
                var approvalPercentage = dto.ApprovalPercentage ?? 70m;
                
                // Calcular porcentagem de score
                decimal? scorePercentage = null;
                if (dto.Score.HasValue && totalPoints > 0)
                {
                    scorePercentage = (dto.Score.Value / totalPoints) * 100;
                }
                else if (totalQuestions > 0 && correctAnswers > 0)
                {
                    // Se não tem score mas tem número de acertos, calcular baseado em acertos
                    scorePercentage = (correctAnswers * 100.0m / totalQuestions);
                }

                // Verificar se foi aprovado
                var isApproved = scorePercentage.HasValue && scorePercentage.Value >= approvalPercentage;

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

                // Criar DTO de resposta com estatísticas
                var responseDto = new ExamHistoryResponseDto
                {
                    Id = result.Id,
                    ExamId = result.ExamId,
                    StudentId = result.StudentId,
                    AttemptNumber = result.AttemptNumber,
                    StartedAt = result.StartedAt,
                    FinishedAt = result.FinishedAt,
                    Score = result.Score,
                    DurationSeconds = result.DurationSeconds,
                    Status = result.Status,
                    CreateDate = result.CreateDate,
                    ModificationDate = result.ModificationDate,
                    TotalQuestions = totalQuestions,
                    AnsweredQuestions = answeredQuestions,
                    CorrectAnswers = correctAnswers,
                    WrongAnswers = wrongAnswers,
                    IsApproved = isApproved,
                    ApprovalPercentage = approvalPercentage,
                    ScorePercentage = scorePercentage,
                    ExamTitle = exam.Title,
                    ExamInstructions = exam.Instructions
                };

                Log.Information("Histórico de exame registrado com estatísticas: ExamId={ExamId}, StudentId={StudentId}, AttemptNumber={AttemptNumber}, " +
                    "TotalQuestions={TotalQuestions}, Answered={Answered}, Correct={Correct}, Approved={Approved}",
                    dto.ExamId, dto.StudentId, attemptNumber, totalQuestions, answeredQuestions, correctAnswers, isApproved);

                return Result<ExamHistoryResponseDto>.Okedit(responseDto);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao registrar histórico de exame com estatísticas");
                await transaction.RollbackAsync();
                return Result<ExamHistoryResponseDto>.Error($"Erro ao registrar histórico de exame: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}