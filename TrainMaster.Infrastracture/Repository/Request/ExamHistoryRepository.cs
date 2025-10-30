using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository
{
    public class ExamHistoryRepository : IExamHistoryRepository
    {
        private readonly DataContext _context;

        public ExamHistoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ExamHistoryEntity>> GetByUserId(long userId)
        {
            return await _context.Set<ExamHistoryEntity>()
                                 .AsNoTracking()
                                 .Include(h => h.Exam)
                                 .Where(h => h.StudentId == userId)
                                 .OrderByDescending(h => h.StartedAt)
                                 .ToListAsync();
        }
    }
}