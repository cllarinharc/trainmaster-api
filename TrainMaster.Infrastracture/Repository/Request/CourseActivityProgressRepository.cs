using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CourseActivityProgressRepository : ICourseActivityProgressRepository
    {
        private readonly DataContext _context;

        public CourseActivityProgressRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CourseActivityProgressEntity> Add(CourseActivityProgressEntity progressEntity)
        {
            var result = await _context.CourseActivityProgressEntity.AddAsync(progressEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public CourseActivityProgressEntity Update(CourseActivityProgressEntity progressEntity)
        {
            var response = _context.CourseActivityProgressEntity.Update(progressEntity);
            return response.Entity;
        }

        public async Task<CourseActivityProgressEntity?> GetById(int id)
        {
            return await _context.CourseActivityProgressEntity
                .Include(p => p.Student)
                .Include(p => p.Course)
                .Include(p => p.Activity)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<CourseActivityProgressEntity?> GetByStudentAndActivity(int studentId, int activityId)
        {
            return await _context.CourseActivityProgressEntity
                .Include(p => p.Activity)
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.ActivityId == activityId);
        }

        public async Task<List<CourseActivityProgressEntity>> GetByStudentAndCourse(int studentId, int courseId)
        {
            return await _context.CourseActivityProgressEntity
                .Include(p => p.Activity)
                .Where(p => p.StudentId == studentId && p.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<List<CourseActivityProgressEntity>> GetByActivityId(int activityId)
        {
            return await _context.CourseActivityProgressEntity
                .Include(p => p.Student)
                .Where(p => p.ActivityId == activityId)
                .ToListAsync();
        }

        public async Task<bool> Exists(int studentId, int activityId)
        {
            return await _context.CourseActivityProgressEntity
                .AnyAsync(p => p.StudentId == studentId && p.ActivityId == activityId);
        }
    }
}

