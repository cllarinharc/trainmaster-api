using TrainMaster.Domain.General;

namespace TrainMaster.Domain.Entity
{
    public class LoginEntity : BaseEntity
    {
        public string? Cpf { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }
}