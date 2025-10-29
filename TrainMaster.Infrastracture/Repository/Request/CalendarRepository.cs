using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly DataContext _context;

        public CalendarRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CalendarEntity>> GetByPeriod(DateOnly start, DateOnly end)
        {
            return await _context.Set<CalendarEntity>()
                .AsNoTracking()
                .Where(c => c.StartDate <= end && c.EndDate >= start)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<List<CalendarEntity>> GetByMonth(int year, int month)
        {
            var firstDay = new DateOnly(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            return await GetByPeriod(firstDay, lastDay);
        }
    }
}