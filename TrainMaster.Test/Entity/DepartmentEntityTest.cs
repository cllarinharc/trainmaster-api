using TrainMaster.Domain.Entity;

namespace TrainMaster.Test.Entity
{
    public class DepartmentEntityTest
    {
        [Fact]
        public void DepartmentEntity_ShouldSetAndGetPropertiesCorrectly()
        {
            // Arrange
            var department = new DepartmentEntity
            {
                Id = 1,
                Name = "Tecnologia da Informação",
                Description = "Departamento responsável pela TI",
                IsActive = true,
                UserId = 100
            };

            var user = new UserEntity
            {
                Id = 100,
                Cpf = "12345678900",
                Email = "user@empresa.com",
                Password = "Senha123",
                IsActive = true
            };

            var team = new TeamEntity
            {
                Id = 10,
                Name = "Desenvolvimento",
                Description = "Equipe de desenvolvimento",
                DepartmentId = department.Id
            };

            department.User = user;
            department.Teams.Add(team);

            // Assert
            Assert.Equal(1, department.Id);
            Assert.Equal("Tecnologia da Informação", department.Name);
            Assert.Equal("Departamento responsável pela TI", department.Description);
            Assert.True(department.IsActive);
            Assert.Equal(100, department.UserId);
            Assert.Equal(user, department.User);
            Assert.Single(department.Teams);
            Assert.Equal(team, department.Teams[0]);
        }
    }
}
