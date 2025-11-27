using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CourseProgressRepository : ICourseProgressRepository
    {
        private readonly DataContext _context;

        public CourseProgressRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CourseProgressEntity> Add(CourseProgressEntity progressEntity)
        {
            var result = await _context.CourseProgressEntity.AddAsync(progressEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public CourseProgressEntity Update(CourseProgressEntity progressEntity)
        {
            var response = _context.CourseProgressEntity.Update(progressEntity);
            return response.Entity;
        }

        public async Task<CourseProgressEntity?> GetById(int id)
        {
            return await _context.CourseProgressEntity
                .Include(p => p.Student)
                .Include(p => p.Course)
                .Include(p => p.LastActivity)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<CourseProgressEntity?> GetByStudentAndCourse(int studentId, int courseId)
        {
            return await _context.CourseProgressEntity
                .Include(p => p.Student)
                .Include(p => p.Course)
                .Include(p => p.LastActivity)
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.CourseId == courseId);
        }

        public async Task<List<CourseProgressEntity>> GetByStudentId(int studentId)
        {
            return await _context.CourseProgressEntity
                .Include(p => p.Course)
                .Include(p => p.LastActivity)
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<CourseProgressEntity>> GetByCourseId(int courseId)
        {
            return await _context.CourseProgressEntity
                .Include(p => p.Student)
                .Include(p => p.LastActivity)
                .Where(p => p.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<bool> Exists(int studentId, int courseId)
        {
            return await _context.CourseProgressEntity
                .AnyAsync(p => p.StudentId == studentId && p.CourseId == courseId);
        }
    }
}

