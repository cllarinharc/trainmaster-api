using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class UserCreateUpdateDto
    {
        [Required(ErrorMessage = "Preencha todos os campos")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF no formato incorreto")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Preencha todos os campos")]
        [EmailAddress(ErrorMessage = "Email no formato incorreto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha todos os campos")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Password { get; set; }
    }
}