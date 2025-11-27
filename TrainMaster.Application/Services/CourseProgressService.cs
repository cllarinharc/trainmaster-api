using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class CourseProgressService : ICourseProgressService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CourseProgressService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<CourseProgressResponseDto>> GetOrCreateProgress(int studentId, int courseId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var progress = await _repositoryUoW.CourseProgressRepository.GetByStudentAndCourse(studentId, courseId);

                if (progress == null)
                {
                    // Verificar se o aluno está matriculado
                    var enrollment = await _repositoryUoW.CourseEnrollmentRepository.GetByStudentAndCourse(studentId, courseId);
                    if (enrollment == null || !enrollment.IsActive)
                    {
                        return Result<CourseProgressResponseDto>.Error("Aluno não está matriculado neste curso.");
                    }

                    // Buscar total de atividades do curso
                    var activities = await _repositoryUoW.CourseActivitieRepository.GetByCourseId(courseId);
                    var totalActivities = activities.Count;

                    progress = new CourseProgressEntity
                    {
                        StudentId = studentId,
                        CourseId = courseId,
                        ProgressPercentage = 0,
                        CompletedActivitiesCount = 0,
                        TotalActivitiesCount = totalActivities,
                        IsCompleted = false,
                        CreateDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    await _repositoryUoW.CourseProgressRepository.Add(progress);
                    await _repositoryUoW.SaveAsync();
                }

                await transaction.CommitAsync();
                return Result<CourseProgressResponseDto>.Okedit(MapToResponseDto(progress));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao obter ou criar progresso do curso");
                await transaction.RollbackAsync();
                return Result<CourseProgressResponseDto>.Error($"Erro ao obter progresso: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<Result<CourseProgressResponseDto>> GetProgress(int studentId, int courseId)
        {
            try
            {
                var progress = await _repositoryUoW.CourseProgressRepository.GetByStudentAndCourse(studentId, courseId);

                if (progress == null)
                {
                    return Result<CourseProgressResponseDto>.Error("Progresso não encontrado. Use GetOrCreateProgress primeiro.");
                }

                return Result<CourseProgressResponseDto>.Okedit(MapToResponseDto(progress));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar progresso do curso");
                return Result<CourseProgressResponseDto>.Error($"Erro ao buscar progresso: {ex.Message}");
            }
        }

        public async Task<Result<CourseProgressResponseDto>> UpdateProgress(int studentId, int courseId)
        {
            try
            {
                var progress = await _repositoryUoW.CourseProgressRepository.GetByStudentAndCourse(studentId, courseId);

                if (progress == null)
                {
                    return Result<CourseProgressResponseDto>.Error("Progresso não encontrado.");
                }

                // Buscar atividades completadas
                var completedActivities = await _repositoryUoW.CourseActivityProgressRepository
                    .GetByStudentAndCourse(studentId, courseId);

                var completedCount = completedActivities.Count(a => a.IsCompleted);

                // Buscar total de atividades
                var activities = await _repositoryUoW.CourseActivitieRepository.GetByCourseId(courseId);
                var totalActivities = activities.Count;

                // Calcular porcentagem
                var percentage = totalActivities > 0
                    ? (decimal)(completedCount * 100.0 / totalActivities)
                    : 0;

                progress.CompletedActivitiesCount = completedCount;
                progress.TotalActivitiesCount = totalActivities;
                progress.ProgressPercentage = Math.Round(percentage, 2);
                progress.IsCompleted = percentage >= 100;
                progress.ModificationDate = DateTime.UtcNow;

                if (progress.IsCompleted && !progress.CompletedDate.HasValue)
                {
                    progress.CompletedDate = DateTime.UtcNow;
                }

                // Atualizar última atividade acessada
                var lastActivity = completedActivities
                    .Where(a => a.LastAccessedDate.HasValue)
                    .OrderByDescending(a => a.LastAccessedDate)
                    .FirstOrDefault();

                if (lastActivity != null)
                {
                    progress.LastActivityId = lastActivity.ActivityId;
                    progress.LastActivityDate = lastActivity.LastAccessedDate;
                }

                _repositoryUoW.CourseProgressRepository.Update(progress);
                await _repositoryUoW.SaveAsync();

                return Result<CourseProgressResponseDto>.Okedit(MapToResponseDto(progress));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao atualizar progresso do curso");
                return Result<CourseProgressResponseDto>.Error($"Erro ao atualizar progresso: {ex.Message}");
            }
        }

        public async Task<List<CourseProgressResponseDto>> GetProgressByStudent(int studentId)
        {
            try
            {
                var progressList = await _repositoryUoW.CourseProgressRepository.GetByStudentId(studentId);
                return progressList.Select(MapToResponseDto).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar progresso do aluno {StudentId}", studentId);
                throw new InvalidOperationException("Erro ao buscar progresso do aluno", ex);
            }
        }

        public async Task<List<CourseProgressResponseDto>> GetProgressByCourse(int courseId)
        {
            try
            {
                var progressList = await _repositoryUoW.CourseProgressRepository.GetByCourseId(courseId);
                return progressList.Select(MapToResponseDto).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar progresso do curso {CourseId}", courseId);
                throw new InvalidOperationException("Erro ao buscar progresso do curso", ex);
            }
        }

        private CourseProgressResponseDto MapToResponseDto(CourseProgressEntity progress)
        {
            return new CourseProgressResponseDto
            {
                Id = progress.Id,
                StudentId = progress.StudentId,
                CourseId = progress.CourseId,
                ProgressPercentage = progress.ProgressPercentage,
                CompletedActivitiesCount = progress.CompletedActivitiesCount,
                TotalActivitiesCount = progress.TotalActivitiesCount,
                LastActivityDate = progress.LastActivityDate,
                LastActivityId = progress.LastActivityId,
                IsCompleted = progress.IsCompleted,
                CompletedDate = progress.CompletedDate,
                CreateDate = progress.CreateDate,
                ModificationDate = progress.ModificationDate,
                CourseName = progress.Course?.Name,
                CourseDescription = progress.Course?.Description,
                LastActivityTitle = progress.LastActivity?.Title
            };
        }
    }
}


