using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public DepartmentService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<DepartmentEntity>> Add(DepartmentEntity departmentEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidDepartment = await IsValidDepartmentRequest(departmentEntity);

                if (!isValidDepartment.Success)
                {
                    Log.Error(LogMessages.InvalidAddressInputs());
                    return Result<DepartmentEntity>.Error(isValidDepartment.Message);
                }

                var checkNameIsExist = await _repositoryUoW.DepartmentRepository.GetByName(departmentEntity.Name);

                if (checkNameIsExist is not null)
                {
                    var errorMessage = "Department already exists with that name";
                    Log.Error(LogMessages.NameExistsDepartment());
                    return Result<DepartmentEntity>.Error(errorMessage);
                }

                departmentEntity.ModificationDate = DateTime.UtcNow;
                departmentEntity.IsActive = true;
                var result = await _repositoryUoW.DepartmentRepository.Add(departmentEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<DepartmentEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingDepartmentError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new department");
            }
            finally
            {
                Log.Error(LogMessages.AddingDepartmentSuccess());
                transaction.Dispose();
            }
        }

        public async Task Delete(int departmentId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var departmentEntity = await _repositoryUoW.DepartmentRepository.GetById(departmentId);
                if (departmentEntity is not null)
                {
                    departmentEntity.IsActive = true;
                    _repositoryUoW.DepartmentRepository.Update(departmentEntity);
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeleteDepartmentError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a department.");
            }
            finally
            {
                Log.Error(LogMessages.DeleteDepartmentSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<DepartmentEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<DepartmentEntity> departments = await _repositoryUoW.DepartmentRepository.Get();
                _repositoryUoW.Commit();
                return departments;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllDepartmentError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list department");
            }
            finally
            {
                Log.Error(LogMessages.GetAllDepartmentSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<DepartmentEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var result = await _repositoryUoW.DepartmentRepository.GetById(id);
                if (result == null)
                    return Result<DepartmentEntity>.Error("Department not found.");

                _repositoryUoW.Commit();

                return Result<DepartmentEntity>.Okedit(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Error searching for department by ID.", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<DepartmentEntity>> GetByUserId(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var result = await _repositoryUoW.DepartmentRepository.GetByUserId(id);
                if (result == null)
                    return Result<DepartmentEntity>.Error("Department not found.");

                _repositoryUoW.Commit();

                return Result<DepartmentEntity>.OkDepartment(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Error searching for department by ID.", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<DepartmentEntity>> Update(DepartmentEntity departmentEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var departmentById = await _repositoryUoW.DepartmentRepository.GetById(departmentEntity.Id);
                if (departmentById is null)
                    throw new InvalidOperationException("Error updating department.");

                departmentById.ModificationDate = DateTime.UtcNow;
                departmentById.Name = departmentEntity.Name;
                departmentById.Description = departmentEntity.Description;
                departmentById.IsActive = departmentEntity.IsActive;

                _repositoryUoW.DepartmentRepository.Update(departmentById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<DepartmentEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorDepartment(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error updating department.", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private async Task<Result<DepartmentEntity>> IsValidDepartmentRequest(DepartmentEntity departmentEntity)
        {
            var requestValidator = await new DepartmentRequestValidator().ValidateAsync(departmentEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<DepartmentEntity>.Error(errorMessage);
            }

            return Result<DepartmentEntity>.Ok();
        }
    }
}