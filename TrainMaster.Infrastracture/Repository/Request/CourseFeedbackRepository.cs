using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CourseFeedbackRepository : ICourseFeedbackRepository
    {
        private readonly DataContext _context;

        public CourseFeedbackRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CourseFeedbackEntity> Add(CourseFeedbackEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Feedback não pode ser nulo.");

            var result = await _context.CourseFeedbackEntity.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public CourseFeedbackEntity Delete(CourseFeedbackEntity entity)
        {
            var response = _context.CourseFeedbackEntity.Remove(entity);
            return response.Entity;
        }

        public async Task<List<CourseFeedbackEntity>> GetAll()
        {
            return await _context.CourseFeedbackEntity
                .AsNoTracking()
                .OrderBy(f => f.Id)
                .ToListAsync();
        }

        public async Task<List<CourseFeedbackEntity>> GetByCourseId(int courseId)
        {
            return await _context.CourseFeedbackEntity
                .AsNoTracking()
                .Where(f => f.CourseId == courseId)
                .OrderBy(f => f.Id)
                .ToListAsync();
        }
        public async Task<CourseFeedbackEntity?> GetById(int id)
        {
            return await _context.CourseFeedbackEntity
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public CourseFeedbackEntity Update(CourseFeedbackEntity entity)
        {
            var response = _context.CourseFeedbackEntity.Update(entity);
            return response.Entity;
        }

        public async Task<bool> ExistsByCourseAndStudent(int courseId, int studentId)
        {
            return await _context.CourseFeedbackEntity
                .AsNoTracking()
                .AnyAsync(f => f.CourseId == courseId && f.StudentId == studentId);
        }
    }
}