using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using System.Linq;

namespace TrainMaster.Application.Services
{
    public class CourseEnrollmentService : ICourseEnrollmentService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CourseEnrollmentService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<CourseEnrollmentEntity>> EnrollStudent(CourseEnrollmentDto enrollmentDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                // Verificar se o aluno existe
                var student = await _repositoryUoW.UserRepository.GetById(enrollmentDto.StudentId);
                if (student == null)
                {
                    Log.Error("Aluno não encontrado: {StudentId}", enrollmentDto.StudentId);
                    return Result<CourseEnrollmentEntity>.Error("Aluno não encontrado.");
                }

                // Verificar se o curso existe
                var course = await _repositoryUoW.CourseRepository.GetById(enrollmentDto.CourseId);
                if (course == null)
                {
                    Log.Error("Curso não encontrado: {CourseId}", enrollmentDto.CourseId);
                    return Result<CourseEnrollmentEntity>.Error("Curso não encontrado.");
                }

                // Verificar se o curso está ativo
                if (!course.IsActive)
                {
                    Log.Error("Tentativa de matrícula em curso inativo: {CourseId}", enrollmentDto.CourseId);
                    return Result<CourseEnrollmentEntity>.Error("O curso não está ativo.");
                }

                // Verificar se já está matriculado
                var existingEnrollment = await _repositoryUoW.CourseEnrollmentRepository.GetByStudentAndCourse(
                    enrollmentDto.StudentId, enrollmentDto.CourseId);

                if (existingEnrollment != null && existingEnrollment.IsActive)
                {
                    Log.Warning("Aluno já está matriculado no curso: StudentId={StudentId}, CourseId={CourseId}",
                        enrollmentDto.StudentId, enrollmentDto.CourseId);
                    return Result<CourseEnrollmentEntity>.Error("Aluno já está matriculado neste curso.");
                }

                // Se existe mas está inativo, reativar
                if (existingEnrollment != null && !existingEnrollment.IsActive)
                {
                    existingEnrollment.IsActive = true;
                    existingEnrollment.EnrollmentDate = DateTime.UtcNow;
                    existingEnrollment.ModificationDate = DateTime.UtcNow;
                    _repositoryUoW.CourseEnrollmentRepository.Update(existingEnrollment);
                }
                else
                {
                    // Criar nova matrícula
                    var enrollment = new CourseEnrollmentEntity
                    {
                        StudentId = enrollmentDto.StudentId,
                        CourseId = enrollmentDto.CourseId,
                        EnrollmentDate = DateTime.UtcNow,
                        IsActive = true,
                        CreateDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    await _repositoryUoW.CourseEnrollmentRepository.Add(enrollment);
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information("Aluno {StudentId} matriculado com sucesso no curso {CourseId}",
                    enrollmentDto.StudentId, enrollmentDto.CourseId);

                var finalEnrollment = await _repositoryUoW.CourseEnrollmentRepository.GetByStudentAndCourse(
                    enrollmentDto.StudentId, enrollmentDto.CourseId);

                return Result<CourseEnrollmentEntity>.Ok("Aluno matriculado com sucesso.", finalEnrollment);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao matricular aluno no curso");
                await transaction.RollbackAsync();
                return Result<CourseEnrollmentEntity>.Error($"Erro ao matricular aluno: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<Result<CourseEnrollmentEntity>> CancelEnrollment(int enrollmentId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var enrollment = await _repositoryUoW.CourseEnrollmentRepository.GetById(enrollmentId);
                if (enrollment == null)
                {
                    return Result<CourseEnrollmentEntity>.Error("Matrícula não encontrada.");
                }

                enrollment.IsActive = false;
                enrollment.ModificationDate = DateTime.UtcNow;
                _repositoryUoW.CourseEnrollmentRepository.Update(enrollment);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information("Matrícula {EnrollmentId} cancelada com sucesso", enrollmentId);
                return Result<CourseEnrollmentEntity>.Ok("Matrícula cancelada com sucesso.", enrollment);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao cancelar matrícula {EnrollmentId}", enrollmentId);
                await transaction.RollbackAsync();
                return Result<CourseEnrollmentEntity>.Error($"Erro ao cancelar matrícula: {ex.Message}");
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public async Task<List<CourseEnrollmentResponseDto>> GetEnrollmentsByStudent(int studentId)
        {
            try
            {
                var enrollments = await _repositoryUoW.CourseEnrollmentRepository.GetByStudentId(studentId);
                return enrollments.Select(e => new CourseEnrollmentResponseDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollmentDate = e.EnrollmentDate,
                    IsActive = e.IsActive,
                    CreateDate = e.CreateDate,
                    ModificationDate = e.ModificationDate,
                    CourseName = e.Course?.Name,
                    CourseDescription = e.Course?.Description,
                    CourseAuthor = e.Course?.Author,
                    CourseStartDate = e.Course?.StartDate ?? DateTime.MinValue,
                    CourseEndDate = e.Course?.EndDate ?? DateTime.MinValue,
                    CourseIsActive = e.Course?.IsActive ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar matrículas do aluno {StudentId}", studentId);
                throw new InvalidOperationException("Erro ao buscar matrículas do aluno", ex);
            }
        }

        public async Task<List<CourseEnrollmentResponseDto>> GetEnrollmentsByCourse(int courseId)
        {
            try
            {
                var enrollments = await _repositoryUoW.CourseEnrollmentRepository.GetByCourseId(courseId);
                return enrollments.Select(e => new CourseEnrollmentResponseDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollmentDate = e.EnrollmentDate,
                    IsActive = e.IsActive,
                    CreateDate = e.CreateDate,
                    ModificationDate = e.ModificationDate,
                    CourseName = e.Course?.Name,
                    CourseDescription = e.Course?.Description,
                    CourseAuthor = e.Course?.Author,
                    CourseStartDate = e.Course?.StartDate ?? DateTime.MinValue,
                    CourseEndDate = e.Course?.EndDate ?? DateTime.MinValue,
                    CourseIsActive = e.Course?.IsActive ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar matrículas do curso {CourseId}", courseId);
                throw new InvalidOperationException("Erro ao buscar matrículas do curso", ex);
            }
        }

        public async Task<Result<CourseEnrollmentEntity>> GetEnrollmentById(int enrollmentId)
        {
            try
            {
                var enrollment = await _repositoryUoW.CourseEnrollmentRepository.GetById(enrollmentId);
                if (enrollment == null)
                {
                    return Result<CourseEnrollmentEntity>.Error("Matrícula não encontrada.");
                }

                return Result<CourseEnrollmentEntity>.Okedit(enrollment);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar matrícula {EnrollmentId}", enrollmentId);
                return Result<CourseEnrollmentEntity>.Error($"Erro ao buscar matrícula: {ex.Message}");
            }
        }
    }
}

