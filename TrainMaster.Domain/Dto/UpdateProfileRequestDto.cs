using TrainMaster.Domain.Enums;

namespace TrainMaster.Domain.Dto
{
    public class UpdateProfileRequestDto
    {
        public int UserId { get; set; }
        public PessoalProfilePartDto? perfil { get; set; }
    }
}

