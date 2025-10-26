namespace TrainMaster.Domain.Dto
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}