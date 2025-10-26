using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Result<AddressEntity>> GetAddressByZipCode(string postalCode);
        Task<Result<AddressEntity>> Add(AddressEntity addressEntity);
        Task Delete(int userId);
        Task<List<AddressEntity>> Get();
        Task<Result<AddressEntity>> Update(int id, AddressEntity addressEntity);
    }
}