using Serilog;
using System.Text.RegularExpressions;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Security.Cryptography;
using TrainMaster.Infrastracture.Security.Token.Access;

namespace TrainMaster.Application.Services
{
    public class AuthService
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly BCryptoAlgorithm _crypto;

        public AuthService(IUserService userService, IUserRepository userRepository, TokenService tokenService, BCryptoAlgorithm crypto)
        {
            _userService = userService;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _crypto = crypto;
        }

        public async Task<Result<LoginDto>> Login(string cpf, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(password))
                    return Result<LoginDto>.Error("CPF e senha são obrigatórios.");
                var Cpf = Regex.Replace(cpf, "[^0-9]", "");
                var user = await _userRepository.GetByCpf(Cpf);

                if (user == null || !_crypto.VerifyPassword(password, user.Password))
                {
                    Log.Error("CPF ou senha incorretos.");
                    return Result<LoginDto>.Error("CPF ou senha incorretos.");
                }

                if (user.Id <= 0 || string.IsNullOrWhiteSpace(user.Email))
                {
                    Log.Error("Dados do usuário inválidos para geração de token.");
                    return Result<LoginDto>.Error("Erro ao autenticar usuário.");
                }

                var token = _tokenService.GenerateToken(user.Id.ToString(), user.Email);

                Log.Information($"Usuário {user.Email} autenticado com sucesso.");

                var loginEntity = new LoginDto
                {
                    Cpf = user.Cpf,
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.PessoalProfile?.FullName,
                    DateOfBirth = user.PessoalProfile?.DateOfBirth
                };

                var result = Result<LoginDto>.OkLogin(loginEntity);
                result.AccessToken = token;
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro inesperado durante login.");
                return Result<LoginDto>.Error("Ocorreu um erro ao processar o login.");
            }
        }

        public async Task<Result<string>> ResetPassword(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Result<string>.Error("O e-mail não pode estar vazio.");

                var user = await _userRepository.GetByEmail(email);
                if (user == null)
                {
                    Log.Warning($"Tentativa de redefinir senha para e-mail não encontrado: {email}");
                    return Result<string>.Error("Usuário não encontrado com o e-mail fornecido.");
                }

                var novaSenha = GerarSenhaAleatoria();

                var senhaCriptografada = _crypto.HashPassword(novaSenha);

                user.Password = senhaCriptografada;
                await _userService.UpdatePasswordByEmail(email, senhaCriptografada);

                Log.Information($"Senha redefinida com sucesso para o e-mail: {email}");

                return Result<string>.OkWithData(novaSenha);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Erro ao redefinir senha para o e-mail: {email}");
                return Result<string>.Error("Erro ao redefinir a senha.");
            }
        }

        private string GerarSenhaAleatoria(int tamanho = 10)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%&*";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteres, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}