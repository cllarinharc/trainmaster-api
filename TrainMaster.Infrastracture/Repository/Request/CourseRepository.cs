using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _context;

        public CourseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CourseEntity> Add(CourseEntity courseEntity)
        {
            var result = await _context.CourseEntity.AddAsync(courseEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public CourseEntity Delete(CourseEntity CourseEntity)
        {
            var response = _context.CourseEntity.Remove(CourseEntity);
            return response.Entity;
        }

        public async Task<List<CourseEntity>> Get()
        {
            return await _context.CourseEntity
                .OrderBy(course => course.Name)
                .Select(course => new CourseEntity
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate,
                    IsActive = course.IsActive,
                    Author = course.Author,
                    UserId = course.UserId
                })
                .ToListAsync();
        }

        public async Task<CourseEntity?> GetById(int? id)
        {
            return await _context.CourseEntity.FirstOrDefaultAsync(courseEntity => courseEntity.Id == id);
        }

        public async Task<List<CourseEntity>> GetByName(string name)
        {
            var term = (name ?? string.Empty).Trim();

            return await _context.CourseEntity
                .Where(c => c.IsActive && EF.Functions.ILike(c.Name!, $"%{term}%"))
                .OrderBy(c => c.Name)
                .Select(c => new CourseEntity
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    IsActive = c.IsActive,
                    Author = c.Author,
                    UserId = c.UserId
                })
                .ToListAsync();
        }

        public async Task<List<CourseEntity>> GetByUserId(int? id)
        {
            return await _context.CourseEntity
                .Where(course => course.UserId == id)
                .ToListAsync();
        }

        public CourseEntity Update(CourseEntity courseEntity)
        {
            var response = _context.CourseEntity.Update(courseEntity);
            return response.Entity;
        }

        public async Task<List<CourseEntity>> GetByPeriod(DateOnly start, DateOnly end)
        {
            var startDateTimeUtc = DateTime.SpecifyKind(start.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var endDateTimeUtc = DateTime.SpecifyKind(end.ToDateTime(new TimeOnly(23, 59, 59)), DateTimeKind.Utc);

            return await _context.CourseEntity
                .AsNoTracking()
                .Where(c => c.StartDate <= endDateTimeUtc && c.EndDate >= startDateTimeUtc)
                .OrderBy(c => c.StartDate)
                .Select(c => new CourseEntity
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    IsActive = c.IsActive,
                    Author = c.Author,
                    UserId = c.UserId
                })
                .ToListAsync();
        }
    }
}