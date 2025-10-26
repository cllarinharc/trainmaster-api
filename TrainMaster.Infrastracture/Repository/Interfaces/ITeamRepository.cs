using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ITeamRepository
    {
        Task<TeamEntity> Add(TeamEntity teamEntity);
        TeamEntity Update(TeamEntity teamEntity);
        TeamEntity Delete(TeamEntity teamEntity);
        Task<List<TeamEntity>> Get();
        Task<TeamEntity?> GetById(int? id);
        Task<TeamEntity?> GetByName(string? name);
        Task<List<TeamEntity>> GetByDepartmentId(int departmentId);
    }
}