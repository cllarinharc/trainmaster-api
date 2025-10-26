using TrainMaster.Domain.Entity;

namespace TrainMaster.Test.Entity
{
    public class EducationLevelEntityTest
    {
        [Fact]
        public void EducationLevelEntity_ShouldSetAndGetPropertiesCorrectly()
        {
            // Arrange
            var startDate = new DateTime(2020, 01, 01);
            var endDate = new DateTime(2023, 12, 31);

            var entity = new EducationLevelEntity
            {
                Id = 1,
                Title = "Graduação em Engenharia",
                Institution = "Universidade XYZ",
                StartedAt = startDate,
                EndeedAt = endDate,
                ProfessionalProfileId = 10
            };

            // Assert
            Assert.Equal(1, entity.Id);
            Assert.Equal("Graduação em Engenharia", entity.Title);
            Assert.Equal("Universidade XYZ", entity.Institution);
            Assert.Equal(startDate, entity.StartedAt);
            Assert.Equal(endDate, entity.EndeedAt);
            Assert.Equal(10, entity.ProfessionalProfileId);
        }
    }
}
