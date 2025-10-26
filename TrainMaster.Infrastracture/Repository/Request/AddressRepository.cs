using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataContext _context;

        public AddressRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AddressEntity> Add(AddressEntity addressEntity)
        {
            var result = await _context.AddressEntity.AddAsync(addressEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public AddressEntity Delete(AddressEntity addressEntity)
        {
            var response = _context.AddressEntity.Remove(addressEntity);
            return response.Entity;
        }

        public async Task<AddressEntity?> GetById(int? id)
        {
            return await _context.AddressEntity.FirstOrDefaultAsync(addressEntity => addressEntity.Id == id);
        }

        public async Task<List<AddressEntity>> Get()
        {
            return await _context.AddressEntity
             .OrderBy(address => address.Id)
             .Select(address => new AddressEntity
             {
                 Id = address.Id,
                 Neighborhood = address.Neighborhood,
                 City = address.City,
                 PostalCode = address.PostalCode,
                 Street = address.Street,
                 PessoalProfileId = address.PessoalProfileId,
             }).ToListAsync();
        }

        public AddressEntity Update(AddressEntity addressEntity)
        {
            var response = _context.AddressEntity.Update(addressEntity);
            return response.Entity;
        }
    }
}