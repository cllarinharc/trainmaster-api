using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
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

        public async Task<ExamHistoryEntity> Add(ExamHistoryEntity entity)
        {
            var result = await _context.Set<ExamHistoryEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<int> GetNextAttemptNumber(int examId, int studentId)
        {
            var lastAttempt = await _context.Set<ExamHistoryEntity>()
                .Where(h => h.ExamId == examId && h.StudentId == studentId)
                .OrderByDescending(h => h.AttemptNumber)
                .FirstOrDefaultAsync();

            return lastAttempt == null ? 1 : lastAttempt.AttemptNumber + 1;
        }

        public async Task<List<ExamHistoryEntity>> GetByExamAndStudent(int examId, int studentId)
        {
            return await _context.Set<ExamHistoryEntity>()
                .Include(h => h.Exam)
                .Where(h => h.ExamId == examId && h.StudentId == studentId)
                .OrderByDescending(h => h.StartedAt)
                .ToListAsync();
        }
    }
}