using TrainMaster.Domain.Enums;

namespace TrainMaster.Domain.Dto
{
    public class PessoalProfilePartDto
    {
        public string? FullName { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderStatus Gender { get; set; }
        public MaritalStatus Marital { get; set; }
    }
}