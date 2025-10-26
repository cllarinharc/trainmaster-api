using System.Text.Json.Serialization;
using TrainMaster.Domain.Entity;
using TrainMaster.Domain.Enums;

namespace TrainMaster.Domain.Dto
{
    public class PessoalProfileDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderStatus Gender { get; set; }
        public MaritalStatus Marital { get; set; }

        [JsonIgnore]
        public UserEntity? User { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public AddressEntity? Address { get; set; }
    }
}