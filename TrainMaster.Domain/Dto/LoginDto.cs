namespace TrainMaster.Domain.Dto
{
    public class LoginDto
    {
        public int Id { get; set; }
        public string? Cpf { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}