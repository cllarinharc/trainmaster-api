using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class HistoryPasswordRepository : IHistoryPasswordRepository
    {
        private readonly DataContext _context;

        public HistoryPasswordRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<HistoryPasswordEntity?> GetById(int? id)
        {
            return await _context.HistoryPasswordEntity.FirstOrDefaultAsync(historyPasswordEntity => historyPasswordEntity.Id == id);
        }

        public DepartmentEntity UpdateOldPassword(HistoryPasswordEntity historyPasswordEntity)
        {
            throw new NotImplementedException();
        }
    }
}