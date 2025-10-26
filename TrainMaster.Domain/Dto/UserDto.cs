namespace TrainMaster.Domain.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
    }
}