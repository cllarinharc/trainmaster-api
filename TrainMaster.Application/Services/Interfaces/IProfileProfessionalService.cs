using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IProfileProfessionalService
    {
        Task<Result<ProfessionalProfileEntity>> Add(ProfessionalProfileEntity professionalProfileEntity);
        Task<Result<ProfessionalProfileEntity>> Update(int id, [FromBody] ProfessionalProfileEntity professionalProfileEntity);
        Task Delete(int professionalProfileEntity);
        Task<List<ProfessionalProfileEntity>> Get();
        Task<Result<ProfessionalProfileEntity>> GetById(int id);
    }
}