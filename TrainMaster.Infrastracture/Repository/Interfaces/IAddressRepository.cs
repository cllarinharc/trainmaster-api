using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IAddressRepository
    {
        Task<AddressEntity> Add(AddressEntity addressEntity);
        AddressEntity Update(AddressEntity addressEntity);
        AddressEntity Delete(AddressEntity addressEntity);
        Task<List<AddressEntity>> Get();
        Task<AddressEntity?> GetById(int? id);
    }
}