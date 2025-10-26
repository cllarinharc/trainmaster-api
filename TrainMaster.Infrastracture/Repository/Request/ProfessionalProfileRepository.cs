using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class ProfessionalProfileRepository : IProfessionalProfileRepository
    {
        private readonly DataContext _context;

        public ProfessionalProfileRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ProfessionalProfileEntity> Add(ProfessionalProfileEntity professionalProfileEntity)
        {
            var result = await _context.ProfessionalProfileEntity.AddAsync(professionalProfileEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public ProfessionalProfileEntity Delete(ProfessionalProfileEntity professionalProfileEntity)
        {
            var response = _context.ProfessionalProfileEntity.Remove(professionalProfileEntity);
            return response.Entity;
        }

        public async Task<List<ProfessionalProfileEntity>> Get()
        {
            return await _context.ProfessionalProfileEntity
                .OrderBy(professionalProfileEntity => professionalProfileEntity.Id)
                .Select(professionalProfileEntity => new ProfessionalProfileEntity
                {
                    Id = professionalProfileEntity.Id,
                    JobTitle = professionalProfileEntity.JobTitle,
                    YearsOfExperience = professionalProfileEntity.YearsOfExperience,
                }).ToListAsync();
        }

        public async Task<ProfessionalProfileEntity?> GetById(int? id)
        {
            return await _context.ProfessionalProfileEntity.FirstOrDefaultAsync(professionalProfileEntity => professionalProfileEntity.Id == id);
        }

        public ProfessionalProfileEntity Update(ProfessionalProfileEntity professionalProfileEntity)
        {
            var response = _context.ProfessionalProfileEntity.Update(professionalProfileEntity);
            return response.Entity;
        }
    }
}