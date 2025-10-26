using TrainMaster.Domain.Entity;

namespace TrainMaster.Test.Entity
{
    public class TeamEntityTest
    {
        [Fact]
        public void TeamEntity_ShouldSetAndGetPropertiesCorrectly()
        {
            // Arrange
            var team = new TeamEntity
            {
                Id = 1,
                Name = "Equipe de Desenvolvimento",
                Description = "Responsável pela criação de software",
                IsActive = true,
                DepartmentId = 42
            };

            // Assert
            Assert.Equal(1, team.Id);
            Assert.Equal("Equipe de Desenvolvimento", team.Name);
            Assert.Equal("Responsável pela criação de software", team.Description);
            Assert.True(team.IsActive);
            Assert.Equal(42, team.DepartmentId);
        }

    }
}