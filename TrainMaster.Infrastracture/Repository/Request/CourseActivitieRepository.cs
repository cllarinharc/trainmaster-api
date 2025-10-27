using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CourseActivitieRepository : ICourseActivitieRepository
    {
        private readonly DataContext _context;

        public CourseActivitieRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CourseActivitieEntity>> GetByCourseId(int courseId)
        {
            return await _context.Set<CourseActivitieEntity>()
                                 .AsNoTracking()
                                 .Where(a => a.CourseId == courseId)
                                 .OrderBy(a => a.StartDate)
                                 .ToListAsync();
        }

        public async Task<CourseActivitieEntity> Add(CourseActivitieEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Atividade não pode ser nula.");

            var result = await _context.CourseActivitieEntity.AddAsync(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<CourseActivitieEntity>> Get()
        {
            return await _context.CourseActivitieEntity
                .Include(a => a.Course)
                .AsNoTracking()
                .OrderBy(a => a.StartDate)
                .Select(a => new CourseActivitieEntity
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    StartDate = a.StartDate,
                    DueDate = a.DueDate,
                    MaxScore = a.MaxScore,
                    CourseId = a.Course.Id,
                })
                .ToListAsync();
        }

        public async Task<CourseActivitieEntity?> GetById(int id)
        {
            return await _context.CourseActivitieEntity
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public CourseActivitieEntity Update(CourseActivitieEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Atividade não pode ser nula.");

            var result = _context.CourseActivitieEntity.Update(entity);
            _context.SaveChanges();

            return result.Entity;
        }
    }
}