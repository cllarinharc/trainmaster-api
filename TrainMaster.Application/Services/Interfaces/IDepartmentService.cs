using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<Result<DepartmentEntity>> Add(DepartmentEntity departmentEntity);
        Task Delete(int departmentId);
        Task<List<DepartmentEntity>> Get();
        Task<Result<DepartmentEntity>> Update(DepartmentEntity departmentEntity);        
        Task<Result<DepartmentEntity>> GetByUserId(int id);
    }
}