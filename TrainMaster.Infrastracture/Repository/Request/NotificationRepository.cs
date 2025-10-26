using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;

        public NotificationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationEntity>> Get(int courseId)
        {
            return await _context.NotificationEntity
                .AsNoTracking()
                .Where(n => n.CourseId == courseId)
                .OrderBy(n => n.Id)
                .ToListAsync();
        }
    }
}