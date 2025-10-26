using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using TrainMaster.Application.Services;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Infrastracture.Security.Cryptography;

namespace TrainMaster.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            _userService = new UserService(_repositoryUoWMock.Object);
        }

        //[Fact]
        //public async Task Add_ShouldReturnError_WhenCpfAlreadyExists()
        //{
        //    var user = new UserEntity
        //    {
        //        Cpf = "12345678900",
        //        Email = "test@example.com",
        //        Password = "Test1234",
        //        IsActive = true,
        //    };

        //    _userRepositoryMock.Setup(x => x.GetByCpf(It.IsAny<string>()))
        //        .ReturnsAsync(new UserEntity());

        //    var result = await _userService.Add(user);

        //    Assert.False(result.Success);            
        //}

        //[Fact]
        //public async Task Add_ShouldReturnSuccess_WhenCpfDoesNotExist()
        //{
        //    var user = new UserEntity
        //    {
        //        Cpf = "12345678900",
        //        Email = "test@example.com",
        //        Password = "Test1234",
        //        IsActive = true,
        //    };

        //    _userRepositoryMock.Setup(x => x.GetByCpf(It.IsAny<string>())).ReturnsAsync((UserEntity?)null);
        //    _userRepositoryMock.Setup(x => x.Add(It.IsAny<UserEntity>())).ReturnsAsync(user);

        //    var result = await _userService.Add(user);

        //    Assert.True(result.Success);
        //    _userRepositoryMock.Verify(x => x.Add(It.IsAny<UserEntity>()), Times.Once);
        //}

        [Fact]
        public async Task Get_ShouldReturnListOfUsers_WhenCalledSuccessfully()
        {
            var expectedUsers = new List<UserDto>
            {
                new UserDto { Id = 1, Email = "joao@example.com" },
                new UserDto { Id = 2, Email = "maria@example.com" }
            };

            _userRepositoryMock.Setup(x => x.Get()).ReturnsAsync(expectedUsers);

            var result = await _userService.Get();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);            
            _userRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            var emptyList = new List<UserDto>();

            _userRepositoryMock.Setup(x => x.Get()).ReturnsAsync(emptyList);
            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);

            var result = await _userService.Get();

            Assert.NotNull(result);
            Assert.Empty(result);
            _userRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task GetAllActives_ShouldReturnListOfActiveUsers_WhenCalledSuccessfully()
        {
            var activeUsers = new List<UserEntity>
            {
                new UserEntity { Id = 1, Email = "pedro@example.com", IsActive = true },
                new UserEntity { Id = 2, Email = "ana@example.com", IsActive = true }
            };

            _userRepositoryMock.Setup(x => x.GetAllActives()).ReturnsAsync(activeUsers);
            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);

            var result = await _userService.GetAllActives();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, u => Assert.True(u.IsActive));
            _userRepositoryMock.Verify(x => x.GetAllActives(), Times.Once);
        }

        [Fact]
        public async Task GetAllActives_ShouldThrowInvalidOperationException_WhenRepositoryThrowsException()
        {
            _userRepositoryMock
                .Setup(x => x.GetAllActives())
                .ThrowsAsync(new Exception("Erro de acesso ao banco"));

            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetAllActives());

            Assert.Equal("Error to loading the list User Actives", exception.Message);
            _userRepositoryMock.Verify(x => x.GetAllActives(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldDeactivateUser_WhenUserExists()
        {
            var userId = 1;
            var existingUser = new UserEntity { Id = userId, IsActive = true };

            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(x => x.UpdateByActive(userId, existingUser.IsActive)).Verifiable();
            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            await _userService.Delete(userId);

            _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateByActive(userId, existingUser.IsActive), Times.Once);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowInvalidOperationException_WhenExceptionIsThrown()
        {
            var userId = 1;

            _userRepositoryMock
                .Setup(x => x.GetById(userId))
                .ThrowsAsync(new Exception("Erro inesperado"));

            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Delete(userId));

            Assert.Equal("Error to delete a User.", exception.Message);
            _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldUpdateUser_WhenUserExists()
        {
            var userDto = new UserUpdateDto
            {
                Id = 1,
                Cpf = "12345678900",
                Email = "usuario@teste.com"
            };

            var existingUser = new UserEntity
            {
                Id = userDto.Id,
                Cpf = "00000000000",
                Email = "old@email.com",
                ModificationDate = DateTime.MinValue
            };

            _userRepositoryMock.Setup(x => x.GetById(userDto.Id)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<UserEntity>()));
            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _userService.Update(userDto);

            Assert.True(result.Success);
            _userRepositoryMock.Verify(x => x.GetById(userDto.Id), Times.Once);
            _userRepositoryMock.Verify(x => x.Update(It.Is<UserEntity>(
                u => u.Cpf == userDto.Cpf && u.Email == userDto.Email && u.ModificationDate != DateTime.MinValue
            )), Times.Once);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
        {
            var userDto = new UserUpdateDto
            {
                Id = 1,
                Cpf = "12345678900",
                Email = "usuario@teste.com"
            };

            _userRepositoryMock.Setup(x => x.GetById(userDto.Id)).ReturnsAsync((UserEntity?)null);
            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Update(userDto));

            Assert.Equal("Error updating User", exception.Message);
            _userRepositoryMock.Verify(x => x.GetById(userDto.Id), Times.Once);
            _userRepositoryMock.Verify(x => x.Update(It.IsAny<UserEntity>()), Times.Never);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task ChangePassword_ShouldUpdatePassword_WhenCredentialsAreValid()
        {
            var email = "usuario@teste.com";
            var currentPassword = "SenhaAntiga123";
            var newPassword = "SenhaNova123";
            var hashedCurrentPassword = new BCryptoAlgorithm().HashPassword(currentPassword);

            var user = new UserEntity
            {
                Id = 1,
                Email = email,
                Password = hashedCurrentPassword
            };

            _userRepositoryMock.Setup(x => x.GetByEmail(email.ToLower())).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<UserEntity>())).Verifiable();
            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            var result = await _userService.ChangePassword(email, currentPassword, newPassword);

            Assert.True(result.Success);
            _userRepositoryMock.Verify(x => x.GetByEmail(email.ToLower()), Times.Once);
            _userRepositoryMock.Verify(x => x.Update(It.Is<UserEntity>(u =>
                u.Password != hashedCurrentPassword && u.ModificationDate != default
            )), Times.Once);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnError_WhenCurrentPasswordIsInvalid()
        {
            var email = "usuario@teste.com";
            var currentPassword = "SenhaIncorreta";
            var newPassword = "SenhaNova123";
            var senhaCorreta = new BCryptoAlgorithm().HashPassword("SenhaCorreta");

            var user = new UserEntity
            {
                Id = 1,
                Email = email,
                Password = senhaCorreta
            };

            _userRepositoryMock.Setup(x => x.GetByEmail(email.ToLower())).ReturnsAsync(user);
            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);

            var result = await _userService.ChangePassword(email, currentPassword, newPassword);

            Assert.False(result.Success);
            Assert.Equal("Incorrect current password.", result.Message);
            _userRepositoryMock.Verify(x => x.GetByEmail(email.ToLower()), Times.Once);
            _userRepositoryMock.Verify(x => x.Update(It.IsAny<UserEntity>()), Times.Never);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task GetPaginated_ShouldReturnPagedUsers_WhenCalledSuccessfully()
        {
            int pageNumber = 1;
            int pageSize = 2;

            var expectedUsers = new List<UserDto>
            {
                new UserDto { Id = 1, Email = "ana@example.com" },
                new UserDto { Id = 2, Email = "carlos@example.com" }
            };

            _userRepositoryMock
                .Setup(x => x.GetPaginated(pageNumber, pageSize))
                .ReturnsAsync(expectedUsers);

            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.Commit());

            var result = await _userService.GetPaginated(pageNumber, pageSize);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _userRepositoryMock.Verify(x => x.GetPaginated(pageNumber, pageSize), Times.Once);
            _repositoryUoWMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task GetPaginated_ShouldThrowInvalidOperationException_WhenRepositoryThrowsException()
        {
            int pageNumber = 1;
            int pageSize = 10;

            _userRepositoryMock
                .Setup(x => x.GetPaginated(pageNumber, pageSize))
                .ThrowsAsync(new Exception("Erro simulado ao acessar o banco"));

            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.GetPaginated(pageNumber, pageSize));

            Assert.Equal("Error to loading the list of users", exception.Message);

            _userRepositoryMock.Verify(x => x.GetPaginated(pageNumber, pageSize), Times.Once);
            _repositoryUoWMock.Verify(x => x.Commit(), Times.Never);
        }
    }
}