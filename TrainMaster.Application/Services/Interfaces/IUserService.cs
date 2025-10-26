using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserEntity>> Add(UserAddDto userEntity);
        Task<Result<UserEntity>> Update(UserUpdateDto userCreateUpdateDto);
        Task Delete(int userId);
        Task<List<UserDto>> Get();
        Task<List<UserDto>> GetPaginated(int pageNumber, int pageSize);
        Task<List<UserEntity>> GetAllActives();
        Task<Result<UserEntity>> UpdatePasswordByEmail(string email, string hashedPassword);
    }
}