using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class QuestionOptionRepository : IQuestionOptionRepository
    {
        private readonly DataContext _context;

        public QuestionOptionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<QuestionOptionEntity> Add(QuestionOptionEntity optionEntity)
        {
            if (optionEntity is null)
                throw new ArgumentNullException(nameof(optionEntity), "Option cannot be null.");

            var result = await _context.QuestionOptionEntity.AddAsync(optionEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public QuestionOptionEntity Update(QuestionOptionEntity optionEntity)
        {
            var response = _context.QuestionOptionEntity.Update(optionEntity);
            _context.SaveChanges();
            return response.Entity;
        }

        public async Task<QuestionOptionEntity> Delete(QuestionOptionEntity optionEntity)
        {
            var response = _context.QuestionOptionEntity.Remove(optionEntity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task<int> DeleteByQuestionId(int questionId)
        {
            var items = await _context.QuestionOptionEntity
                                      .Where(o => o.QuestionId == questionId)
                                      .ToListAsync();

            if (items.Count == 0) return 0;

            _context.QuestionOptionEntity.RemoveRange(items);
            return await _context.SaveChangesAsync();
        }

        public async Task<QuestionOptionEntity?> GetById(int id)
        {
            return await _context.QuestionOptionEntity
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<QuestionOptionEntity>> GetByQuestionId(int questionId)
        {
            return await _context.QuestionOptionEntity
                                 .AsNoTracking()
                                 .Where(o => o.QuestionId == questionId)
                                 .OrderBy(o => o.Id)
                                 .ToListAsync();
        }

        public async Task<QuestionOptionEntity?> GetCorrectByQuestionId(int questionId)
        {
            return await _context.QuestionOptionEntity
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(o => o.QuestionId == questionId && o.IsCorrect);
        }

        public async Task<List<QuestionOptionEntity>> GetAllByQuestionIds(IEnumerable<int> questionIds)
        {
            var ids = questionIds?.Distinct().ToList() ?? new List<int>();
            if (ids.Count == 0) return new List<QuestionOptionEntity>();

            return await _context.QuestionOptionEntity
                                 .AsNoTracking()
                                 .Where(o => ids.Contains(o.QuestionId))
                                 .OrderBy(o => o.QuestionId)
                                 .ThenBy(o => o.Id)
                                 .ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.QuestionOptionEntity.AnyAsync(o => o.Id == id);
        }
    }
}