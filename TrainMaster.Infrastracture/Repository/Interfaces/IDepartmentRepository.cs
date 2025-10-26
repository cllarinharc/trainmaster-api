using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<DepartmentEntity> Add(DepartmentEntity departmentEntity);
        DepartmentEntity Update(DepartmentEntity departmentEntity);
        DepartmentEntity Delete(DepartmentEntity departmentEntity);
        Task<List<DepartmentEntity>> Get();
        Task<DepartmentEntity?> GetById(int? id);
        Task<DepartmentEntity?> GetByName(string? name);
        Task<DepartmentEntity> GetByUserId(int userId);
        Task<DepartmentEntity?> GetByUserId(int? id);
    }
}