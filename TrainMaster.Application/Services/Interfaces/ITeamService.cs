using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ITeamService
    {
        Task<Result<TeamEntity>> Add(TeamEntity teamEntity);
        Task Delete(int teamId);
        Task<List<TeamEntity>> Get();
        Task<Result<TeamEntity>> GetById(int id);
        Task<Result<TeamEntity>> Update(TeamEntity teamEntity);
    }
}