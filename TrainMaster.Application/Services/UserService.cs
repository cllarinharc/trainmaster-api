using Serilog;
using System.Text.RegularExpressions;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Infrastracture.Security.Cryptography;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public UserService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<UserEntity>> Add(UserAddDto dto)
        {
            await using var tx = _repositoryUoW.BeginTransaction();
            try
            {
                var user = new UserEntity
                {
                    Cpf = dto.Cpf,
                    Email = dto.Email?.Trim().ToLower(),
                    Password = new BCryptoAlgorithm().HashPassword(dto.Password),
                    IsActive = true,
                    CreateDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                var isValid = await IsValidUserRequest(user);
                if (!isValid.Success)
                    return Result<UserEntity>.Error(isValid.Message);

                var cpf = Regex.Replace(dto.Cpf ?? "", @"\D", "");
                var checkCpf = await UniqueCpf(cpf);
                if (checkCpf)
                    return Result<UserEntity>.Error("Repeated CPF");

                await _repositoryUoW.UserRepository.Add(user);
                await _repositoryUoW.SaveAsync();

                var perfil = new PessoalProfileEntity
                {
                    FullName = dto.FullName,
                    UserId = user.Id,
                    Cpf = dto.Cpf,
                    Email =dto.Email
                };
                await _repositoryUoW.PessoalProfileRepository.Add(perfil);

                await _repositoryUoW.SaveAsync();
                await tx.CommitAsync();

                return Result<UserEntity>.Ok();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return Result<UserEntity>.Error($"Error to add a new User: {ex.Message}");
            }
        }

        public async Task Delete(int userId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var userToDelete = await _repositoryUoW.UserRepository.GetById(userId);                
                if (userToDelete is not null)
                    _repositoryUoW.UserRepository.UpdateByActive(userToDelete.Id, userToDelete.IsActive);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeleteUserError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a User.");
            }
            finally
            {
                Log.Error(LogMessages.DeleteUserSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<UserDto>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<UserDto> userEntities = await _repositoryUoW.UserRepository.Get();
                _repositoryUoW.Commit();
                return userEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllUserError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list User");
            }
            finally
            {
                Log.Error(LogMessages.GetAllUserSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<UserDto>> GetPaginated(int pageNumber, int pageSize)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<UserDto> userEntities = await _repositoryUoW.UserRepository.GetPaginated(pageNumber, pageSize);
                _repositoryUoW.Commit();
                return userEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllUserError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list of users");
            }
            finally
            {
                Log.Error(LogMessages.GetAllUserSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<UserEntity>> GetAllActives()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<UserEntity> userEntities = await _repositoryUoW.UserRepository.GetAllActives();
                _repositoryUoW.Commit();
                return userEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllUserError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list User Actives");
            }
            finally
            {
                Log.Error(LogMessages.GetAllUserSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<UserEntity>> Update(UserUpdateDto userCreateUpdateDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var userById = await _repositoryUoW.UserRepository.GetById(userCreateUpdateDto.Id);
                if (userById is null)
                    throw new InvalidOperationException("Error updating User");

                userById.Cpf = Regex.Replace(userCreateUpdateDto.Cpf, "[^0-9]", "");
                userById.Email = userCreateUpdateDto.Email;
                userById.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.UserRepository.Update(userById);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<UserEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorUser(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error updating User", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<UserEntity>> ChangePassword(string email, string currentPassword, string newPassword)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var crypto = new BCryptoAlgorithm();
                var normalizedEmail = email?.Trim().ToLower();

                var user = await _repositoryUoW.UserRepository.GetByEmail(normalizedEmail);

                if (user is null)
                {
                    Log.Error(LogMessages.UserNotFound());
                    return Result<UserEntity>.Error("User not found.");
                }

                var isPasswordValid = crypto.VerifyPassword(currentPassword, user.Password);
                if (!isPasswordValid)
                {
                    Log.Error(LogMessages.PasswordInvalid());
                    return Result<UserEntity>.Error("Incorrect current password.");
                }

                user.Password = crypto.HashPassword(newPassword);
                user.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.UserRepository.Update(user);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information(LogMessages.UpdatingSuccessPassword());
                return Result<UserEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating password: {ex.Message}");
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error updating the user's password.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<UserEntity>> UpdatePasswordByEmail(string email, string hashedPassword)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var normalizedEmail = email?.Trim().ToLower();

                var user = await _repositoryUoW.UserRepository.GetByEmail(normalizedEmail);

                if (user is null)
                {
                    Log.Error(LogMessages.UserNotFound());
                    return Result<UserEntity>.Error("User not found.");
                }

                user.Password = hashedPassword;
                user.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.UserRepository.Update(user);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information($"Password successfully updated for email: {email}");
                return Result<UserEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating password for email: {email} - {ex.Message}");
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error updating password.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<UserEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var user = await _repositoryUoW.UserRepository.GetById(id);
                if (user == null)
                {
                    Log.Error($"Id user {id} not found.");
                    return Result<UserEntity>.Error("User not found.");
                }

                user.Email = user.Email.Trim().ToLower();
                user.Cpf = FormatCpf(user.Cpf);
                _repositoryUoW.Commit();
                return Result<UserEntity>.Okedit(user);
            }
            catch (Exception ex)
            {
                Log.Error($"Error searching for user by ID: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Error searching for user by ID.", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private string FormatCpf(string cpf)
        {
            cpf = cpf?.Trim();

            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
                return cpf;

            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        private async Task<bool> UniqueCpf(string cpf)
        {
            return await _repositoryUoW.UserRepository.GetByCpf(cpf) is not null;
        }

        private async Task<Result<UserEntity>> IsValidUserRequest(UserEntity userEntity)
        {
            var requestValidator = await new UserRequestValidator().ValidateAsync(userEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<UserEntity>.Error(errorMessage);
            }

            return Result<UserEntity>.Ok();
        }
    }
}