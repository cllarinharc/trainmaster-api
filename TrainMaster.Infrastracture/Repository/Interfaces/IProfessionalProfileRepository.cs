using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface IProfessionalProfileRepository
    {
        Task<ProfessionalProfileEntity> Add(ProfessionalProfileEntity professionalProfileEntity);
        ProfessionalProfileEntity Update(ProfessionalProfileEntity professionalProfileEntity);
        ProfessionalProfileEntity Delete(ProfessionalProfileEntity professionalProfileEntity);
        Task<List<ProfessionalProfileEntity>> Get();
        Task<ProfessionalProfileEntity?> GetById(int? id);
    }
}