using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class FaqRepository : IFaqRepository
    {
        private readonly DataContext _context;

        public FaqRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<FaqEntity>> Get()
        {
            return await _context.FaqEntity
                .AsNoTracking()
                .OrderBy(faq => faq.Id)
                .Select(faq => new FaqEntity
                {
                    Id = faq.Id,
                    Question = faq.Question,
                    Answer = faq.Answer,
                })
                .ToListAsync();
        }
    }
}