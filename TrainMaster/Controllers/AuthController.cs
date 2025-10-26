using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.Services;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Cpf) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "CPF e senha são obrigatórios." });

            var result = await _authService.Login(request.Cpf, request.Password);
            if (result.Success)
                return Ok(result);

            return BadRequest(new { message = result.Message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordDto request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { message = "O e-mail não pode estar vazio." });

            var result = await _authService.ResetPassword(request.Email);

            if (result.Success)
                return Ok(new { message = result.Data });

            return BadRequest(new { message = result.Message });
        }
    }
}