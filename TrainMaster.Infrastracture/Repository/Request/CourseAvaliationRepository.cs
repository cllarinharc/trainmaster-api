using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CourseAvaliationRepository : ICourseAvaliationRepository
    {
        private readonly DataContext _context;

        public CourseAvaliationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CourseAvaliationEntity> Add(CourseAvaliationEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Avaliation cannot be null");

            var result = await _context.CourseAvaliationEntity.AddAsync(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<CourseAvaliationEntity>> Get()
        {
            return await _context.CourseAvaliationEntity
                .Include(a => a.Course)
                .AsNoTracking()
                .OrderBy(a => a.ReviewDate)
                .ToListAsync();
        }

        public async Task<CourseAvaliationEntity?> GetById(int id)
        {
            return await _context.CourseAvaliationEntity
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public CourseAvaliationEntity Update(CourseAvaliationEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Avaliation cannot be null");

            var result = _context.CourseAvaliationEntity.Update(entity);
            _context.SaveChanges();
            return result.Entity;
        }
    }
}