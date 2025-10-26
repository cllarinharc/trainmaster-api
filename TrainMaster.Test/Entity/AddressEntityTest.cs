using TrainMaster.Domain.Entity;

namespace TrainMaster.Test.Entity
{
    public class AddressEntityTest
    {
        [Fact]
        public void AddressEntity_ShouldSetAndGetPropertiesCorrectly()
        {
            // Arrange
            var address = new AddressEntity
            {
                Id = 1,
                PostalCode = "60170150",
                Street = "Rua Teste",
                Neighborhood = "Bairro Teste",
                City = "Cidade Teste",
                Uf = "CE",
                PessoalProfileId = 2
            };

            // Assert
            Assert.Equal(1, address.Id);
            Assert.Equal("60170150", address.PostalCode);
            Assert.Equal("Rua Teste", address.Street);
            Assert.Equal("Bairro Teste", address.Neighborhood);
            Assert.Equal("Cidade Teste", address.City);
            Assert.Equal("CE", address.Uf);
            Assert.Equal(2, address.PessoalProfileId);
            Assert.Null(address.PessoalProfile);
        }
    }
}
