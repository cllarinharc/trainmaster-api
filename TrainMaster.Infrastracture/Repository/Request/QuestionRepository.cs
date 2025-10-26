using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DataContext _context;

        public QuestionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<QuestionEntity> Add(QuestionEntity questionEntity)
        {
            if (questionEntity is null)
                throw new ArgumentNullException(nameof(questionEntity), "Question cannot be null.");

            var result = await _context.QuestionEntity.AddAsync(questionEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public QuestionEntity Update(QuestionEntity questionEntity)
        {
            var response = _context.QuestionEntity.Update(questionEntity);
            _context.SaveChanges();
            return response.Entity;
        }

        public async Task<QuestionEntity?> GetById(int id, bool includeOptions = true)
        {
            IQueryable<QuestionEntity> query = _context.QuestionEntity;

            if (includeOptions)
                query = query.Include(q => q.Options);

            return await query.AsNoTracking()
                              .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<QuestionEntity>> GetByIds(IEnumerable<int> ids, bool includeOptions = true)
        {
            var idsList = ids.Distinct().ToList();
            IQueryable<QuestionEntity> query = _context.QuestionEntity
                                                       .Where(q => idsList.Contains(q.Id));

            if (includeOptions)
                query = query.Include(q => q.Options);

            return await query.AsNoTracking()
                              .OrderBy(q => q.CourseActivitieId)
                              .ThenBy(q => q.Order)
                              .ToListAsync();
        }

        public async Task<List<QuestionEntity>> GetByActivityId(int activityId, bool includeOptions = true)
        {
            IQueryable<QuestionEntity> query = _context.QuestionEntity
                                                       .Where(q => q.CourseActivitieId == activityId);

            if (includeOptions)
                query = query.Include(q => q.Options);

            return await query.AsNoTracking()
                              .OrderBy(q => q.Order)
                              .ToListAsync();
        }

        public async Task<List<QuestionEntity>> GetPaginated(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 50;

            return await _context.QuestionEntity
                                 .AsNoTracking()
                                 .OrderBy(q => q.Id)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<List<QuestionEntity>> GetAll()
        {
            return await _context.QuestionEntity
                                 .AsNoTracking()
                                 .OrderBy(q => q.Id)
                                 .ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.QuestionEntity.AnyAsync(q => q.Id == id);
        }

        public async Task<QuestionEntity> Delete(QuestionEntity questionEntity)
        {
            var response = _context.QuestionEntity.Remove(questionEntity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }
    }
}