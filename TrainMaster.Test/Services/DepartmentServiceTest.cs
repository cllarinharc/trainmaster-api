using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using TrainMaster.Application.Services;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Test.Services
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IDepartmentRepository> _departmentRepositoryMock;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _departmentRepositoryMock = new Mock<IDepartmentRepository>();

            _repositoryUoWMock.Setup(x => x.DepartmentRepository).Returns(_departmentRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            _departmentService = new DepartmentService(_repositoryUoWMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenValidDepartment()
        {
            var department = new DepartmentEntity { Name = "TI", Description = "Tecnologia", UserId = 1 };

            _departmentRepositoryMock.Setup(x => x.GetByName(department.Name)).ReturnsAsync((DepartmentEntity?)null);
            _departmentRepositoryMock.Setup(x => x.Add(It.IsAny<DepartmentEntity>())).ReturnsAsync(department);

            var result = await _departmentService.Add(department);

            Assert.True(result.Success);
            _departmentRepositoryMock.Verify(x => x.Add(It.IsAny<DepartmentEntity>()), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnError_WhenDepartmentNameAlreadyExists()
        {
            var department = new DepartmentEntity { Name = "TI", Description = "Tecnologia", UserId = 1 };

            _departmentRepositoryMock.Setup(x => x.GetByName(department.Name)).ReturnsAsync(department);

            var result = await _departmentService.Add(department);

            Assert.False(result.Success);
            Assert.Equal("Department already exists with that name", result.Message);
        }

        [Fact]
        public async Task Delete_ShouldDeactivateDepartment_WhenDepartmentExists()
        {
            var department = new DepartmentEntity { Id = 1, Name = "TI", IsActive = true };

            _departmentRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(department);

            await _departmentService.Delete(1);

            _departmentRepositoryMock.Verify(x => x.Update(It.Is<DepartmentEntity>(d => d.IsActive == true)), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnListOfDepartments()
        {
            var departments = new List<DepartmentEntity>
            {
                new DepartmentEntity { Id = 1, Name = "TI" },
                new DepartmentEntity { Id = 2, Name = "RH" }
            };

            _departmentRepositoryMock.Setup(x => x.Get()).ReturnsAsync(departments);

            var result = await _departmentService.Get();

            Assert.Equal(2, result.Count);
            _departmentRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnDepartment_WhenExists()
        {
            var department = new DepartmentEntity { Id = 1, Name = "TI" };

            _departmentRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(department);

            var result = await _departmentService.GetById(1);

            Assert.True(result.Success);
            Assert.Equal(department.Name, result.Data.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnError_WhenNotFound()
        {
            _departmentRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((DepartmentEntity?)null);

            var result = await _departmentService.GetById(1);

            Assert.False(result.Success);
            Assert.Equal("departamento não encontrado", result.Message);
        }

        [Fact]
        public async Task GetByUserId_ShouldReturnDepartment_WhenExists()
        {
            var department = new DepartmentEntity { Id = 1, Name = "TI", UserId = 1 };

            _departmentRepositoryMock.Setup(x => x.GetByUserId(1)).ReturnsAsync(department);

            var result = await _departmentService.GetByUserId(1);

            Assert.True(result.Success);
            Assert.Equal(department.Name, result.Data.Name);
        }

        [Fact]
        public async Task GetByUserId_ShouldReturnError_WhenNotFound()
        {
            _departmentRepositoryMock.Setup(x => x.GetByUserId(1)).ReturnsAsync((DepartmentEntity?)null);

            var result = await _departmentService.GetByUserId(1);

            Assert.False(result.Success);
            Assert.Equal("departamento não encontrado", result.Message);
        }

        [Fact]
        public async Task Update_ShouldUpdate_WhenDepartmentExists()
        {
            var department = new DepartmentEntity { Id = 1, Name = "TI", Description = "Old Desc" };

            _departmentRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(department);

            var updatedDepartment = new DepartmentEntity { Id = 1, Name = "TI Updated", Description = "New Desc" };

            var result = await _departmentService.Update(updatedDepartment);

            Assert.True(result.Success);
            _departmentRepositoryMock.Verify(x => x.Update(It.Is<DepartmentEntity>(
                d => d.Name == "TI Updated" && d.Description == "New Desc"
            )), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldThrow_WhenDepartmentDoesNotExist()
        {
            _departmentRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((DepartmentEntity?)null);

            var updatedDepartment = new DepartmentEntity { Id = 1, Name = "TI Updated" };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _departmentService.Update(updatedDepartment));

            Assert.Equal("Error updating department", exception.Message);
        }
    }
}
