namespace TrainMaster.Domain.Dto
{
    public class UserAddDto
    {
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public string? FullName { get; set; }
    }
}