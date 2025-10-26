using System.ComponentModel.DataAnnotations;

namespace TrainMaster.Domain.Dto
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Por favor, preencha o campo de e-mail.")]
        [EmailAddress(ErrorMessage = "Email no formato incorreto")]
        public string Email { get; set; }
    }
}