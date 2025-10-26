using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DataContext _context;

        public TeamRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TeamEntity> Add(TeamEntity teamEntity)
        {
            var result = await _context.TeamEntity.AddAsync(teamEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public TeamEntity Delete(TeamEntity teamEntity)
        {
            var response = _context.TeamEntity.Remove(teamEntity);
            return response.Entity;
        }

        public async Task<List<TeamEntity>> Get()
        {
            return await _context.TeamEntity
                .OrderBy(team => team.Id)
                .Select(team => new TeamEntity
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description,
                    CreateDate = team.CreateDate,  
                    IsActive = team.IsActive,
                }).ToListAsync();
        }

        public async Task<TeamEntity?> GetById(int? id)
        {
            return await _context.TeamEntity.FirstOrDefaultAsync(teamEntity => teamEntity.Id == id);
        }

        public async Task<TeamEntity?> GetByName(string? name)
        {
            return await _context.TeamEntity.FirstOrDefaultAsync(teamEntity => teamEntity.Name == name);
        }

        public TeamEntity Update(TeamEntity teamEntity)
        {
            var response = _context.TeamEntity.Update(teamEntity);
            return response.Entity;
        }

        public async Task<List<TeamEntity>> GetByDepartmentId(int departmentId)
        {
            return await _context.TeamEntity
                .Where(t => t.DepartmentId == departmentId)
                .ToListAsync();
        }
    }
}