using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface IProfilePessoal
    {
        Task<Result<PessoalProfileEntity>> Add(PessoalProfileEntity pessoalProfileEntity);
        Task<Result<PessoalProfileEntity>> Update(int id, PessoalProfilePartDto pessoalProfileEntity);
        Task Delete(int pessoalProfileEntity);
        Task<List<PessoalProfileEntity>> Get();
    }
}