using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class PessoalProfileRepository : IPessoalProfileRepository
    {
        private readonly DataContext _context;

        public PessoalProfileRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PessoalProfileEntity> Add(PessoalProfileEntity pessoalProfileEntity)
        {
            var result = await _context.PessoalProfileEntity.AddAsync(pessoalProfileEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public PessoalProfileEntity Delete(PessoalProfileEntity pessoalProfileEntity)
        {
            var response = _context.PessoalProfileEntity.Remove(pessoalProfileEntity);
            return response.Entity;
        }

        public async Task<List<PessoalProfileEntity>> Get()
        {
            return await _context.PessoalProfileEntity
                .OrderBy(pessoalProfile => pessoalProfile.Id)
                .Select(pessoalProfile => new PessoalProfileEntity
                {
                    Id = pessoalProfile.Id,
                    DateOfBirth = pessoalProfile.DateOfBirth,
                    FullName = pessoalProfile.FullName,
                    Gender = pessoalProfile.Gender,
                    Marital = pessoalProfile.Marital
                }).ToListAsync();
        }

        public async Task<PessoalProfileEntity?> GetById(int id)
        {
            return await _context.PessoalProfileEntity
                .FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<PessoalProfileEntity?> GetByFullName(string? fullName)
        {
            return await _context.PessoalProfileEntity.FirstOrDefaultAsync(pessoalProfileEntity => pessoalProfileEntity.FullName == fullName);
        }

        public PessoalProfileEntity Update(PessoalProfileEntity pessoalProfileEntity)
        {
            var response = _context.PessoalProfileEntity.Update(pessoalProfileEntity);
            return response.Entity;
        }
    }
}