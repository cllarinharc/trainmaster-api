using System.Text.Json.Serialization;
using TrainMaster.Domain.Enums;
using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class PessoalProfileEntity : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderStatus Gender { get; set; }
        public MaritalStatus Marital { get; set; }

        [JsonIgnore]
        public UserEntity? User { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public AddressEntity? Address { get; set; }
    }
}