using TrainMaster.Domain.Entity;

namespace TrainMaster.Test.Entity
{
    public class UserEntityTest
    {
        [Fact]
        public void UserEntity_ShouldSetAndGetPropertiesCorrectly()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = 1,
                Cpf = "12345678900",
                Email = "teste@example.com",
                Password = "Senha123",
                IsActive = true
            };

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("12345678900", user.Cpf);
            Assert.Equal("teste@example.com", user.Email);
            Assert.Equal("Senha123", user.Password);
            Assert.True(user.IsActive);
        }
    }
}