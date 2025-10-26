using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CourseService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<CourseDto>> Add(CourseDto courseEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidAddress = await IsValidCourseRequest(courseEntity);

                if (!isValidAddress.Success)
                {
                    Log.Error(LogMessages.InvalidAddressInputs());
                    return Result<CourseDto>.Error(isValidAddress.Message);
                }

                if (courseEntity.EndDate < courseEntity.StartDate)
                {
                    var errorMessage = "End date cannot be earlier than start date.";
                    Log.Error(LogMessages.InvalidDateRangeCourse());
                    return Result<CourseDto>.Error(errorMessage);
                }
                
                var course = new CourseEntity
                {
                    Name = courseEntity.Name,
                    Description = courseEntity.Description,
                    StartDate = DateTime.SpecifyKind(courseEntity.StartDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(courseEntity.EndDate, DateTimeKind.Utc),
                    IsActive = true,
                    UserId = courseEntity.UserId,
                    Author = courseEntity.Author,
                };

                var result = await _repositoryUoW.CourseRepository.Add(course);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<CourseDto>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingCourseError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new course");
            }
            finally
            {
                Log.Error(LogMessages.AddingCourseSuccess());
                transaction.Dispose();
            }
        }

        public async Task Delete(int courseId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var courseEntity = await _repositoryUoW.CourseRepository.GetById(courseId);
                if (courseEntity is not null)
                {
                    courseEntity.IsActive = false;
                    _repositoryUoW.CourseRepository.Delete(courseEntity);
                }                    

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeleteCourseError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a course.");
            }
            finally
            {
                Log.Error(LogMessages.DeleteCourseSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<CourseEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<CourseEntity> courseEntities = await _repositoryUoW.CourseRepository.Get();
                _repositoryUoW.Commit();
                return courseEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllCourseError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list course");
            }
            finally
            {
                Log.Error(LogMessages.GetAllCourseSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<CourseEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var course = await _repositoryUoW.CourseRepository.GetById(id);
                if (course == null)
                    return Result<CourseEntity>.Error("Curso não encontrado");

                _repositoryUoW.Commit();
                
                return Result<CourseEntity>.Okedit(course);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar curso por ID", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<List<CourseEntity>> GetByUserId(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<CourseEntity> courseEntities = await _repositoryUoW.CourseRepository.GetByUserId(id);
                _repositoryUoW.Commit();
                return courseEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllCourseError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list course");
            }
            finally
            {
                Log.Error(LogMessages.GetAllCourseSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<CourseEntity>> GetByName(string name)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<CourseEntity> courseEntities = await _repositoryUoW.CourseRepository.GetByName(name);
                _repositoryUoW.Commit();
                return courseEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllCourseError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list course");
            }
            finally
            {
                Log.Error(LogMessages.GetAllCourseSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<CourseEntity>> Update(CourseEntity courseEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var courseById = await _repositoryUoW.CourseRepository.GetById(courseEntity.Id);
                if (courseById is null)
                    throw new InvalidOperationException("Error updating Course");

                courseById.ModificationDate = DateTime.UtcNow;
                courseById.Name = courseEntity.Name;
                courseById.Description = courseEntity.Description;

                courseById.StartDate = courseEntity.StartDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(courseEntity.StartDate, DateTimeKind.Utc)
                    : courseEntity.StartDate.ToUniversalTime();

                courseById.EndDate = courseEntity.EndDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(courseEntity.EndDate, DateTimeKind.Utc)
                    : courseEntity.EndDate.ToUniversalTime();

                _repositoryUoW.CourseRepository.Update(courseById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<CourseEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorCourse(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error updating course", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<CourseDto>> IsValidCourseRequest(CourseDto courseEntity)
        {
            var requestValidator = await new CourseRequestValidator().ValidateAsync(courseEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<CourseDto>.Error(errorMessage);
            }

            return Result<CourseDto>.Ok();
        }
    }
}