using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class ExamQuestionRepository : IExamQuestionRepository
    {
        private readonly DataContext _context;

        public ExamQuestionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ExamQuestionEntity> Add(ExamQuestionEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "ExamQuestion cannot be null.");

            var result = await _context.ExamQuestionEntity.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<int> AddRange(IEnumerable<ExamQuestionEntity> entities)
        {
            var list = entities?.ToList() ?? new List<ExamQuestionEntity>();
            if (list.Count == 0) return 0;

            await _context.ExamQuestionEntity.AddRangeAsync(list);
            return await _context.SaveChangesAsync();
        }

        public ExamQuestionEntity Update(ExamQuestionEntity entity)
        {
            var response = _context.ExamQuestionEntity.Update(entity);
            _context.SaveChanges();
            return response.Entity;
        }

        public async Task<ExamQuestionEntity> Delete(ExamQuestionEntity entity)
        {
            var response = _context.ExamQuestionEntity.Remove(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task<int> DeleteByExamId(int examId)
        {
            var items = await _context.ExamQuestionEntity
                                      .Where(eq => eq.ExamId == examId)
                                      .ToListAsync();

            if (items.Count == 0) return 0;

            _context.ExamQuestionEntity.RemoveRange(items);
            return await _context.SaveChangesAsync();
        }

        public async Task<ExamQuestionEntity?> GetById(int id, bool includeRefs = false)
        {
            IQueryable<ExamQuestionEntity> query = _context.ExamQuestionEntity;

            if (includeRefs)
                query = query.Include(eq => eq.Exam)
                             .Include(eq => eq.Question);

            return await query.AsNoTracking()
                              .FirstOrDefaultAsync(eq => eq.Id == id);
        }

        public async Task<ExamQuestionEntity?> GetByExamAndQuestion(int examId, int questionId)
        {
            return await _context.ExamQuestionEntity
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(eq => eq.ExamId == examId && eq.QuestionId == questionId);
        }

        public async Task<List<ExamQuestionEntity>> GetByExamId(int examId, bool includeQuestion = false)
        {
            IQueryable<ExamQuestionEntity> query = _context.ExamQuestionEntity
                                                           .Where(eq => eq.ExamId == examId);

            if (includeQuestion)
                query = query.Include(eq => eq.Question)
                             .ThenInclude(q => q.Options);

            return await query.AsNoTracking()
                              .OrderBy(eq => eq.Order)
                              .ToListAsync();
        }

        public async Task<List<ExamQuestionEntity>> GetAll()
        {
            return await _context.ExamQuestionEntity
                                 .AsNoTracking()
                                 .OrderBy(eq => eq.ExamId)
                                 .ThenBy(eq => eq.Order)
                                 .ToListAsync();
        }

        public async Task<List<ExamQuestionEntity>> GetPaginated(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 50;

            return await _context.ExamQuestionEntity
                                 .AsNoTracking()
                                 .OrderBy(eq => eq.Id)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.ExamQuestionEntity.AnyAsync(eq => eq.Id == id);
        }
    }
}