using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class EducationLevelRepository : IEducationLevelRepository
    {
        private readonly DataContext _context;

        public EducationLevelRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<EducationLevelEntity> Add(EducationLevelEntity educationLevelEntity)
        {
            var result = await _context.EducationLevelEntity.AddAsync(educationLevelEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public EducationLevelEntity Delete(EducationLevelEntity educationLevelEntity)
        {
            var response = _context.EducationLevelEntity.Remove(educationLevelEntity);
            return response.Entity;
        }

        public async Task<List<EducationLevelEntity>> Get()
        {
            return await _context.EducationLevelEntity
                .OrderBy(educationLevelEntity => educationLevelEntity.Id)
                .Select(educationLevelEntity => new EducationLevelEntity
                {
                    Id = educationLevelEntity.Id,
                    Institution = educationLevelEntity.Institution,
                    StartedAt = educationLevelEntity.StartedAt,
                    EndeedAt = educationLevelEntity.EndeedAt,
                }).ToListAsync();
        }

        public async Task<EducationLevelEntity?> GetById(int? id)
        {
            return await _context.EducationLevelEntity.FirstOrDefaultAsync(educationLevelEntity => educationLevelEntity.Id == id);
        }

        public EducationLevelEntity Update(EducationLevelEntity educationLevelEntity)
        {
            var response = _context.EducationLevelEntity.Update(educationLevelEntity);
            return response.Entity;
        }
    }
}