using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using TrainMaster.Application.Services;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Test.Services
{
    public class EducationLevelServiceTest
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IEducationLevelRepository> _educationRepositoryMock;
        private readonly EducationLevelService _service;

        public EducationLevelServiceTest()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _educationRepositoryMock = new Mock<IEducationLevelRepository>();

            _repositoryUoWMock.Setup(x => x.EducationLevelRepository).Returns(_educationRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            _service = new EducationLevelService(_repositoryUoWMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenValid()
        {
            var education = new EducationLevelEntity
            {
                Title = "Ensino Superior",
                Institution = "Universidade XYZ",
                StartedAt = DateTime.UtcNow.AddYears(-3),
                EndeedAt = DateTime.UtcNow
            };

            _educationRepositoryMock.Setup(x => x.Add(It.IsAny<EducationLevelEntity>())).ReturnsAsync(education);
            _repositoryUoWMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _service.Add(education);

            Assert.True(result.Success);
            _educationRepositoryMock.Verify(x => x.Add(It.IsAny<EducationLevelEntity>()), Times.Once);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnEntity_WhenExists()
        {
            var education = new EducationLevelEntity { Id = 1, Title = "Mestrado" };

            _educationRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(education);

            var result = await _service.GetById(1);

            Assert.True(result.Success);
            Assert.Equal(education.Id, result.Data.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnError_WhenNotExists()
        {
            _educationRepositoryMock.Setup(x => x.GetById(999)).ReturnsAsync((EducationLevelEntity?)null);

            var result = await _service.GetById(999);

            Assert.False(result.Success);
            Assert.Equal("Educação não encontrada", result.Message);
        }

        [Fact]
        public async Task Delete_ShouldExecute_WhenEntityExists()
        {
            var education = new EducationLevelEntity { Id = 1 };

            _educationRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(education);
            _repositoryUoWMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            await _service.Delete(1);

            _educationRepositoryMock.Verify(x => x.GetById(1), Times.Once);
            _educationRepositoryMock.Verify(x => x.Update(education), Times.Once);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnList_WhenEntitiesExist()
        {
            var list = new List<EducationLevelEntity>
            {
                new() { Id = 1, Title = "Ensino Médio" },
                new() { Id = 2, Title = "Graduação" }
            };

            _educationRepositoryMock.Setup(x => x.Get()).ReturnsAsync(list);

            var result = await _service.Get();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
