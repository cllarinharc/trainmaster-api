using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CourseEnrollmentRepository : ICourseEnrollmentRepository
    {
        private readonly DataContext _context;

        public CourseEnrollmentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CourseEnrollmentEntity> Add(CourseEnrollmentEntity enrollmentEntity)
        {
            var result = await _context.CourseEnrollmentEntity.AddAsync(enrollmentEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public CourseEnrollmentEntity Update(CourseEnrollmentEntity enrollmentEntity)
        {
            var response = _context.CourseEnrollmentEntity.Update(enrollmentEntity);
            return response.Entity;
        }

        public async Task<CourseEnrollmentEntity?> GetById(int id)
        {
            return await _context.CourseEnrollmentEntity
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<CourseEnrollmentEntity?> GetByStudentAndCourse(int studentId, int courseId)
        {
            return await _context.CourseEnrollmentEntity
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<List<CourseEnrollmentEntity>> GetByStudentId(int studentId)
        {
            return await _context.CourseEnrollmentEntity
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId && e.IsActive)
                .ToListAsync();
        }

        public async Task<List<CourseEnrollmentEntity>> GetByCourseId(int courseId)
        {
            return await _context.CourseEnrollmentEntity
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId && e.IsActive)
                .ToListAsync();
        }

        public async Task<bool> Exists(int studentId, int courseId)
        {
            return await _context.CourseEnrollmentEntity
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId && e.IsActive);
        }
    }
}


