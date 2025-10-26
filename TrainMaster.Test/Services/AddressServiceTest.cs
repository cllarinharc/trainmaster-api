using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using TrainMaster.Application.Services;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using Xunit;

namespace TrainMaster.Test.Services
{
    public class AddressServiceTests
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IAddressRepository> _addressRepositoryMock;
        private readonly AddressService _addressService;

        public AddressServiceTests()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _addressRepositoryMock = new Mock<IAddressRepository>();

            _repositoryUoWMock.Setup(x => x.AddressRepository).Returns(_addressRepositoryMock.Object);
            _repositoryUoWMock.Setup(x => x.BeginTransaction()).Returns(Mock.Of<IDbContextTransaction>());

            _addressService = new AddressService(_repositoryUoWMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenAddressIsValid()
        {
            var address = new AddressEntity
            {
                PostalCode = "12345678",
                Street = "Rua Teste",
                Neighborhood = "Bairro",
                City = "Cidade",
                Uf = "SP"
            };

            _addressRepositoryMock.Setup(x => x.Add(It.IsAny<AddressEntity>())).ReturnsAsync(address);

            var result = await _addressService.Add(address);

            Assert.True(result.Success);
            _addressRepositoryMock.Verify(x => x.Add(It.IsAny<AddressEntity>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepository_WhenAddressExists()
        {
            var address = new AddressEntity { Id = 1 };

            _addressRepositoryMock.Setup(x => x.GetById(address.Id)).ReturnsAsync(address);

            await _addressService.Delete(address.Id);

            _addressRepositoryMock.Verify(x => x.GetById(address.Id), Times.Once);
            _repositoryUoWMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnListOfAddresses()
        {
            var addresses = new List<AddressEntity>
            {
                new AddressEntity { Id = 1, Street = "Rua A" },
                new AddressEntity { Id = 2, Street = "Rua B" }
            };

            _addressRepositoryMock.Setup(x => x.Get()).ReturnsAsync(addresses);

            var result = await _addressService.Get();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnAddress_WhenAddressExists()
        {
            var address = new AddressEntity { Id = 1, Street = "Rua A" };

            _addressRepositoryMock.Setup(x => x.GetById(address.Id)).ReturnsAsync(address);

            var result = await _addressService.GetById(address.Id);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task Update_ShouldCreate_WhenAddressDoesNotExist()
        {
            var address = new AddressEntity
            {
                PostalCode = "12345678",
                Street = "Rua A",
                Neighborhood = "Bairro",
                City = "Cidade",
                Uf = "SP"
            };

            _addressRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((AddressEntity?)null);

            var result = await _addressService.Update(1, address);

            Assert.True(result.Success);
            _addressRepositoryMock.Verify(x => x.Add(It.IsAny<AddressEntity>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldUpdate_WhenAddressExists()
        {
            var existingAddress = new AddressEntity { Id = 1, Street = "Rua Antiga" };

            _addressRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(existingAddress);

            var updatedAddress = new AddressEntity { Street = "Rua Nova", PostalCode = "12345678" };

            var result = await _addressService.Update(1, updatedAddress);

            Assert.True(result.Success);
            _addressRepositoryMock.Verify(x => x.Update(It.IsAny<AddressEntity>()), Times.Once);
        }
    }
}
