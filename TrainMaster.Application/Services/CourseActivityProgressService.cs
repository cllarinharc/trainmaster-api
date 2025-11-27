using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class CourseActivityProgressService : ICourseActivityProgressService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CourseActivityProgressService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<CourseActivityProgressResponseDto>> MarkActivityAsCompleted(CourseActivityProgressDto progressDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                // Verificar se a atividade existe
                var activity = await _repositoryUoW.CourseActivitieRepository.GetById(progressDto.ActivityId);
                if (activity == null)
                {
                    return Result<CourseActivityProgressResponseDto>.Error("Atividade não encontrada.");
                }

                // Verificar se o aluno está matriculado no curso
                var enrollment = await _repositoryUoW.CourseEnrollmentRepository
                    .GetByStudentAndCourse(progressDto.StudentId, progressDto.CourseId);

                if (enrollment == null || !enrollment.IsActive)
                {
                    return Result<CourseActivityProgressResponseDto>.Error("Aluno não está matriculado neste curso.");
                }

                // Buscar ou criar progresso da atividade
                var activityProgress = await _repositoryUoW.CourseActivityProgressRepository
                    .GetByStudentAndActivity(progressDto.StudentId, progressDto.ActivityId);

                if (activityProgress == null)
                {
                    activityProgress = new CourseActivityProgressEntity
                    {
                        StudentId = progressDto.StudentId,
                        CourseId = progressDto.CourseId,
                        ActivityId = progressDto.ActivityId,
                        IsCompleted = true,
                        CompletedDate = DateTime.UtcNow,
                        Score = progressDto.Score,
                        StartedDate = DateTime.UtcNow,
                        LastAccessedDate = DateTime.UtcNow,
                        CreateDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    await _repositoryUoW.CourseActivityProgressRepository.Add(activityProgress);
                }
                else
                {
                    activityProgress.IsCompleted = true;
                    activityProgress.CompletedDate = DateTime.UtcNow;
                    activityProgress.LastAccessedDate = DateTime.UtcNow;
                    activityProgress.ModificationDate = DateTime.UtcNow;

                    if (progressDto.Score.HasValue)
                    {
                        activityProgress.Score = progressDto.Score;
                    }

                    _repositoryUoW.CourseActivityProgressRepository.Update(activityProgress);
                }

                await _repositoryUoW.SaveAsync();

                // Atualizar progresso geral do curso
                var courseProgressService = new CourseProgressService(_repositoryUoW);
                await courseProgressService.UpdateProgress(progressDto.StudentId, progressDto.CourseId);

                await transaction.CommitAsync();

                var response = await MapToResponseDto(activityProgress);
                return Result<CourseActivityProgressResponseDto>.Okedit(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao marcar atividade como concluída");
                await transaction.RollbackAsync();
                return Result<CourseActivityProgressResponseDto>.Error($"Erro ao marcar atividade como concluída: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<Result<CourseActivityProgressResponseDto>> UpdateActivityProgress(CourseActivityProgressDto progressDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var activityProgress = await _repositoryUoW.CourseActivityProgressRepository
                    .GetByStudentAndActivity(progressDto.StudentId, progressDto.ActivityId);

                if (activityProgress == null)
                {
                    return Result<CourseActivityProgressResponseDto>.Error("Progresso da atividade não encontrado.");
                }

                activityProgress.IsCompleted = progressDto.IsCompleted;
                activityProgress.LastAccessedDate = DateTime.UtcNow;
                activityProgress.ModificationDate = DateTime.UtcNow;

                if (progressDto.Score.HasValue)
                {
                    activityProgress.Score = progressDto.Score;
                }

                if (progressDto.IsCompleted && !activityProgress.CompletedDate.HasValue)
                {
                    activityProgress.CompletedDate = DateTime.UtcNow;
                }

                _repositoryUoW.CourseActivityProgressRepository.Update(activityProgress);
                await _repositoryUoW.SaveAsync();

                // Atualizar progresso geral do curso
                var courseProgressService = new CourseProgressService(_repositoryUoW);
                await courseProgressService.UpdateProgress(progressDto.StudentId, progressDto.CourseId);

                await transaction.CommitAsync();

                var response = await MapToResponseDto(activityProgress);
                return Result<CourseActivityProgressResponseDto>.Okedit(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao atualizar progresso da atividade");
                await transaction.RollbackAsync();
                return Result<CourseActivityProgressResponseDto>.Error($"Erro ao atualizar progresso: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<Result<CourseActivityProgressResponseDto>> GetActivityProgress(int studentId, int activityId)
        {
            try
            {
                var activityProgress = await _repositoryUoW.CourseActivityProgressRepository
                    .GetByStudentAndActivity(studentId, activityId);

                if (activityProgress == null)
                {
                    return Result<CourseActivityProgressResponseDto>.Error("Progresso da atividade não encontrado.");
                }

                var response = await MapToResponseDto(activityProgress);
                return Result<CourseActivityProgressResponseDto>.Okedit(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar progresso da atividade");
                return Result<CourseActivityProgressResponseDto>.Error($"Erro ao buscar progresso: {ex.Message}");
            }
        }

        public async Task<List<CourseActivityProgressResponseDto>> GetActivitiesProgressByCourse(int studentId, int courseId)
        {
            try
            {
                var activitiesProgress = await _repositoryUoW.CourseActivityProgressRepository
                    .GetByStudentAndCourse(studentId, courseId);

                var result = new List<CourseActivityProgressResponseDto>();
                foreach (var progress in activitiesProgress)
                {
                    result.Add(await MapToResponseDto(progress));
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar progresso das atividades do curso");
                throw new InvalidOperationException("Erro ao buscar progresso das atividades", ex);
            }
        }

        public async Task<Result<CourseActivityProgressResponseDto>> MarkActivityAsAccessed(int studentId, int activityId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var activityProgress = await _repositoryUoW.CourseActivityProgressRepository
                    .GetByStudentAndActivity(studentId, activityId);

                if (activityProgress == null)
                {
                    // Buscar atividade para obter CourseId
                    var activity = await _repositoryUoW.CourseActivitieRepository.GetById(activityId);
                    if (activity == null)
                    {
                        return Result<CourseActivityProgressResponseDto>.Error("Atividade não encontrada.");
                    }

                    // Verificar matrícula
                    var enrollment = await _repositoryUoW.CourseEnrollmentRepository
                        .GetByStudentAndCourse(studentId, activity.CourseId);

                    if (enrollment == null || !enrollment.IsActive)
                    {
                        return Result<CourseActivityProgressResponseDto>.Error("Aluno não está matriculado neste curso.");
                    }

                    // Criar novo registro
                    activityProgress = new CourseActivityProgressEntity
                    {
                        StudentId = studentId,
                        CourseId = activity.CourseId,
                        ActivityId = activityId,
                        IsCompleted = false,
                        StartedDate = DateTime.UtcNow,
                        LastAccessedDate = DateTime.UtcNow,
                        CreateDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    await _repositoryUoW.CourseActivityProgressRepository.Add(activityProgress);
                }
                else
                {
                    activityProgress.LastAccessedDate = DateTime.UtcNow;
                    activityProgress.ModificationDate = DateTime.UtcNow;

                    if (!activityProgress.StartedDate.HasValue)
                    {
                        activityProgress.StartedDate = DateTime.UtcNow;
                    }

                    _repositoryUoW.CourseActivityProgressRepository.Update(activityProgress);
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                var response = await MapToResponseDto(activityProgress);
                return Result<CourseActivityProgressResponseDto>.Okedit(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao marcar atividade como acessada");
                await transaction.RollbackAsync();
                return Result<CourseActivityProgressResponseDto>.Error($"Erro ao marcar atividade como acessada: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        private async Task<CourseActivityProgressResponseDto> MapToResponseDto(CourseActivityProgressEntity progress)
        {
            // Buscar atividade se não estiver carregada
            var activity = progress.Activity;
            if (activity == null)
            {
                activity = await _repositoryUoW.CourseActivitieRepository.GetById(progress.ActivityId);
            }

            return new CourseActivityProgressResponseDto
            {
                Id = progress.Id,
                StudentId = progress.StudentId,
                CourseId = progress.CourseId,
                ActivityId = progress.ActivityId,
                IsCompleted = progress.IsCompleted,
                CompletedDate = progress.CompletedDate,
                Score = progress.Score,
                StartedDate = progress.StartedDate,
                LastAccessedDate = progress.LastAccessedDate,
                CreateDate = progress.CreateDate,
                ModificationDate = progress.ModificationDate,
                ActivityTitle = activity?.Title,
                ActivityDescription = activity?.Description,
                ActivityMaxScore = activity?.MaxScore ?? 0
            };
        }
    }
}


