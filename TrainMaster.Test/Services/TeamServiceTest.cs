using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Test.Services
{
    public class TeamServiceTest
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public TeamServiceTest()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _repositoryUoWMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            _userService = new UserService(_repositoryUoWMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenTeamIsValidAndNameIsUnique()
        {
            var team = new TeamEntity
            {
                Name = "Time de Inovação",
                Description = "Equipe responsável por projetos inovadores",
                DepartmentId = 1
            };

            var mockTeamRepository = new Mock<ITeamRepository>();
            var mockRepositoryUoW = new Mock<IRepositoryUoW>();

            mockTeamRepository.Setup(x => x.GetByName(team.Name)).ReturnsAsync((TeamEntity?)null);
            mockTeamRepository.Setup(x => x.Add(It.IsAny<TeamEntity>())).ReturnsAsync(team);
            mockRepositoryUoW.Setup(x => x.TeamRepository).Returns(mockTeamRepository.Object);
            mockRepositoryUoW.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());
            mockRepositoryUoW.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var service = new Mock<TeamService>(mockRepositoryUoW.Object) { CallBase = true };            

            var result = await service.Object.Add(team);

            Assert.True(result.Success);
            mockTeamRepository.Verify(x => x.GetByName(team.Name), Times.Once);
            mockTeamRepository.Verify(x => x.Add(It.Is<TeamEntity>(t =>
                t.Name == team.Name &&
                t.Description == team.Description &&
                t.IsActive == true &&
                t.ModificationDate != default
            )), Times.Once);
            mockRepositoryUoW.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnError_WhenTeamNameAlreadyExists()
        {
            var team = new TeamEntity
            {
                Name = "Time de Inovação",
                Description = "Duplicado",
                DepartmentId = 1
            };

            var existingTeam = new TeamEntity
            {
                Id = 99,
                Name = "Time de Inovação"
            };

            var mockTeamRepository = new Mock<ITeamRepository>();
            var mockRepositoryUoW = new Mock<IRepositoryUoW>();

            mockTeamRepository.Setup(x => x.GetByName(team.Name)).ReturnsAsync(existingTeam);
            mockRepositoryUoW.Setup(x => x.TeamRepository).Returns(mockTeamRepository.Object);
            mockRepositoryUoW.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            var service = new Mock<TeamService>(mockRepositoryUoW.Object) { CallBase = true };

            var result = await service.Object.Add(team);

            Assert.False(result.Success);
            Assert.Equal($"Já existe um time com o nome \"{team.Name}\".", result.Message);
            mockTeamRepository.Verify(x => x.GetByName(team.Name), Times.Once);
            mockTeamRepository.Verify(x => x.Add(It.IsAny<TeamEntity>()), Times.Never);
            mockRepositoryUoW.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Delete_ShouldSetTeamAsActive_WhenTeamExists()
        {
            int teamId = 1;

            var existingTeam = new TeamEntity
            {
                Id = teamId,
                Name = "Time de Suporte",
                IsActive = false
            };

            var mockTeamRepository = new Mock<ITeamRepository>();
            var mockRepositoryUoW = new Mock<IRepositoryUoW>();

            mockTeamRepository.Setup(x => x.GetById(teamId)).ReturnsAsync(existingTeam);
            mockRepositoryUoW.Setup(x => x.TeamRepository).Returns(mockTeamRepository.Object);
            mockRepositoryUoW.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());
            mockRepositoryUoW.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var service = new TeamService(mockRepositoryUoW.Object);

            await service.Delete(teamId);

            mockTeamRepository.Verify(x => x.GetById(teamId), Times.Once);
            mockTeamRepository.Verify(x => x.Update(It.Is<TeamEntity>(t =>
                t.Id == teamId && t.IsActive == true
            )), Times.Once);
            mockRepositoryUoW.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowInvalidOperationException_WhenRepositoryThrowsException()
        {
            int teamId = 1;

            var mockTeamRepository = new Mock<ITeamRepository>();
            var mockRepositoryUoW = new Mock<IRepositoryUoW>();

            mockTeamRepository
                .Setup(x => x.GetById(teamId))
                .ThrowsAsync(new Exception("Erro simulado"));

            mockRepositoryUoW.Setup(x => x.TeamRepository).Returns(mockTeamRepository.Object);
            mockRepositoryUoW.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            var service = new TeamService(mockRepositoryUoW.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(teamId));

            Assert.Equal("Error to delete a team.", exception.Message);

            mockTeamRepository.Verify(x => x.GetById(teamId), Times.Once);
            mockTeamRepository.Verify(x => x.Update(It.IsAny<TeamEntity>()), Times.Never);
            mockRepositoryUoW.Verify(x => x.SaveAsync(), Times.Never);         
        }
    }
}