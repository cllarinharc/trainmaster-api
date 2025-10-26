using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly DataContext _context;

        public BadgeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<BadgeEntity?> GetById(int? id)
        {
            return await _context.BadgeEntity.FirstOrDefaultAsync(badgeEntity => badgeEntity.Id == id);
        }
    }
}