using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using TrainMaster.Application.Services;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Test.Services
{
    public class CourseServiceTests
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly CourseService _courseService;

        public CourseServiceTests()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _courseRepositoryMock = new Mock<ICourseRepository>();

            _repositoryUoWMock.Setup(x => x.CourseRepository).Returns(_courseRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            _courseService = new CourseService(_repositoryUoWMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenValidCourse()
        {
            var course = new CourseDto
            {
                Name = "Curso de Teste",
                Description = "Descrição do curso",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                UserId = 1
            };

            _courseRepositoryMock.Setup(x => x.Add(It.IsAny<CourseEntity>()))
                .ReturnsAsync(new CourseEntity { Id = 1, Name = course.Name });

            var result = await _courseService.Add(course);

            Assert.True(result.Success);
            _courseRepositoryMock.Verify(x => x.Add(It.IsAny<CourseEntity>()), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnError_WhenEndDateBeforeStartDate()
        {
            var course = new CourseDto
            {
                Name = "Curso",
                Description = "Teste",
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow,
                UserId = 1
            };

            var result = await _courseService.Add(course);

            Assert.False(result.Success);
            Assert.Equal("End date cannot be earlier than start date.", result.Message);
        }

        [Fact]
        public async Task Delete_ShouldMarkAsInactive_WhenCourseExists()
        {
            var course = new CourseEntity { Id = 1, Name = "Teste", IsActive = true };

            _courseRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(course);

            await _courseService.Delete(1);

            _courseRepositoryMock.Verify(x => x.Delete(It.IsAny<CourseEntity>()), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnListOfCourses()
        {
            var expectedCourses = new List<CourseEntity>
            {
                new CourseEntity { Id = 1, Name = "Curso A" },
                new CourseEntity { Id = 2, Name = "Curso B" }
            };

            _courseRepositoryMock.Setup(x => x.Get()).ReturnsAsync(expectedCourses);

            var result = await _courseService.Get();

            Assert.Equal(2, result.Count);
            _courseRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnCourse_WhenExists()
        {
            var course = new CourseEntity { Id = 1, Name = "Curso Teste" };

            _courseRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(course);

            var result = await _courseService.GetById(1);

            Assert.True(result.Success);
            Assert.Equal(course.Name, result.Data.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnError_WhenNotFound()
        {
            _courseRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((CourseEntity?)null);

            var result = await _courseService.GetById(1);

            Assert.False(result.Success);
            Assert.Equal("Curso não encontrado", result.Message);
        }

        [Fact]
        public async Task GetByUserId_ShouldReturnCourses_ForSpecificUser()
        {
            var expectedCourses = new List<CourseEntity>
            {
                new CourseEntity { Id = 1, UserId = 1 },
                new CourseEntity { Id = 2, UserId = 1 }
            };

            _courseRepositoryMock.Setup(x => x.GetByUserId(1)).ReturnsAsync(expectedCourses);

            var result = await _courseService.GetByUserId(1);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Update_ShouldUpdate_WhenCourseExists()
        {
            var course = new CourseEntity
            {
                Id = 1,
                Name = "Curso Antigo",
                Description = "Descrição antiga",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5)
            };

            _courseRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(course);

            var updateCourse = new CourseEntity
            {
                Id = 1,
                Name = "Curso Atualizado",
                Description = "Descrição atualizada",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            var result = await _courseService.Update(updateCourse);

            Assert.True(result.Success);
            _courseRepositoryMock.Verify(x => x.Update(It.IsAny<CourseEntity>()), Times.Once);
        }
    }
}