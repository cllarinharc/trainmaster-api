using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class ExamRepository : IExamRepository
    {
        private readonly DataContext _context;

        public ExamRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ExamEntity> Add(ExamEntity examEntity)
        {
            if (examEntity is null)
                throw new ArgumentNullException(nameof(examEntity), "Exam cannot be null.");

            var result = await _context.ExamEntity.AddAsync(examEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public ExamEntity Update(ExamEntity examEntity)
        {
            var response = _context.ExamEntity.Update(examEntity);
            _context.SaveChanges();
            return response.Entity;
        }

        public async Task<ExamEntity> Delete(ExamEntity examEntity)
        {
            var response = _context.ExamEntity.Remove(examEntity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task<ExamEntity?> GetById(int id, bool includeQuestions = false)
        {
            IQueryable<ExamEntity> query = _context.ExamEntity;

            if (includeQuestions)
                query = query
                    .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question);

            return await query.AsNoTracking()
                              .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<ExamEntity>> GetAll()
        {
            return await _context.ExamEntity
                                 .AsNoTracking()
                                 .OrderBy(e => e.Id)
                                 .ToListAsync();
        }

        public async Task<List<ExamEntity>> GetPaginated(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 50;

            return await _context.ExamEntity
                                 .AsNoTracking()
                                 .OrderBy(e => e.Id)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<List<ExamEntity>> GetByCourseId(int courseId)
        {
            return await _context.ExamEntity
                                 .AsNoTracking()
                                 .Where(e => e.CourseId == courseId)
                                 .OrderBy(e => e.StartAt)
                                 .ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.ExamEntity.AnyAsync(e => e.Id == id);
        }
    }
}