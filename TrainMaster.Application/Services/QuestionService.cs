using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public QuestionService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<List<QuestionEntity>> GetByActivityId(int activityId)
        {
            using var tx = _repositoryUoW.BeginTransaction();
            try
            {
                if (activityId <= 0)
                    return new List<QuestionEntity>();

                var questions = await _repositoryUoW.QuestionRepository
                                                    .GetByActivityId(activityId, includeOptions: true);

                _repositoryUoW.Commit();
                return questions ?? new List<QuestionEntity>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar questões por ActivityId {ActivityId}", activityId);
                tx.Rollback();
                throw new InvalidOperationException("Erro ao buscar questões por ActivityId.");
            }
            finally
            {
                tx.Dispose();
            }
        }

        public async Task<Result<QuestionEntity>> Add(QuestionAddDto dto)
        {
            await using var tx = _repositoryUoW.BeginTransaction();
            try
            {
                if (dto is null)
                    return Result<QuestionEntity>.Error("Payload cannot be null.");

                if (string.IsNullOrWhiteSpace(dto.Statement))
                    return Result<QuestionEntity>.Error("Statement is required.");

                if (dto.Options is null || dto.Options.Count < 2)
                    return Result<QuestionEntity>.Error("Provide at least 2 options.");

                var correctCount = dto.Options.Count(o => o.IsCorrect);
                if (correctCount != 1)
                    return Result<QuestionEntity>.Error("Exactly 1 option must be marked as correct.");

                if (dto.CourseActivitieId.HasValue && dto.CourseActivitieId.Value > 0)
                {
                    var activity = await _repositoryUoW.CourseActivitieRepository.GetById(dto.CourseActivitieId.Value);
                    if (activity is null)
                        return Result<QuestionEntity>.Error("Course activity not found.");
                }

                var question = new QuestionEntity
                {
                    Statement = dto.Statement.Trim(),
                    Order = dto.Order,
                    Points = dto.Points,
                    CourseActivitieId = dto.CourseActivitieId ?? 0
                };

                await _repositoryUoW.QuestionRepository.Add(question);
                await _repositoryUoW.SaveAsync();

                foreach (var opt in dto.Options)
                {
                    var option = new QuestionOptionEntity
                    {
                        QuestionId = question.Id,
                        Text = opt.Text?.Trim() ?? string.Empty,
                        IsCorrect = opt.IsCorrect
                    };
                    await _repositoryUoW.QuestionOptionRepository.Add(option);
                }

                await _repositoryUoW.SaveAsync();
                await tx.CommitAsync();

                Log.Information("Question created successfully (Id: {Id})", question.Id);
                return Result<QuestionEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error("Error creating question: {Message}", ex.Message);
                await tx.RollbackAsync();
                return Result<QuestionEntity>.Error("Error creating question.");
            }
        }

        public async Task<Result<bool>> AttachToActivity(AttachQuestionsToActivityDto dto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                if (dto is null)
                    return Result<bool>.Error("Payload cannot be null.");

                if (dto.ActivityId <= 0)
                    return Result<bool>.Error("Invalid ActivityId.");

                if (dto.QuestionIds is null || dto.QuestionIds.Count == 0)
                    return Result<bool>.Error("Send at least one QuestionId.");

                var activity = await _repositoryUoW.CourseActivitieRepository.GetById(dto.ActivityId);
                if (activity is null)
                    return Result<bool>.Error("Course activity not found.");

                foreach (var qid in dto.QuestionIds.Distinct())
                {
                    var q = await _repositoryUoW.QuestionRepository.GetById(qid);
                    if (q is null)
                        return Result<bool>.Error($"Question {qid} not found.");

                    q.CourseActivitieId = dto.ActivityId;
                    _repositoryUoW.QuestionRepository.Update(q);
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information("Questions attached to activity {ActivityId}", dto.ActivityId);
                return Result<bool>.Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<bool>.Error("Error attaching questions to activity.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<bool>> AttachToExam(AttachQuestionsToExamDto dto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                if (dto is null)
                    return Result<bool>.Error("Payload cannot be null.");

                if (dto.ExamId <= 0)
                    return Result<bool>.Error("Invalid ExamId.");

                if (dto.QuestionIds is null || dto.QuestionIds.Count == 0)
                    return Result<bool>.Error("Send at least one QuestionId.");

                var exam = await _repositoryUoW.ExamRepository.GetById(dto.ExamId);
                if (exam is null)
                    return Result<bool>.Error("Exam not found.");

                foreach (var qid in dto.QuestionIds.Distinct())
                {
                    var q = await _repositoryUoW.QuestionRepository.GetById(qid);
                    if (q is null)
                        return Result<bool>.Error($"Question {qid} not found.");
                }

                foreach (var qid in dto.QuestionIds.Distinct())
                {
                    var exists = await _repositoryUoW.ExamQuestionRepository.GetByExamAndQuestion(dto.ExamId, qid);
                    if (exists is null)
                    {
                        var link = new ExamQuestionEntity
                        {
                            ExamId = dto.ExamId,
                            QuestionId = qid
                        };
                        await _repositoryUoW.ExamQuestionRepository.Add(link);
                    }
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information("Questions attached to exam {ExamId}", dto.ExamId);
                return Result<bool>.Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result<bool>.Error("Error attaching questions to exam.");
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}